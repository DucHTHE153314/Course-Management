// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CourseManagementWebClientWebClient.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementWebClientWebClient.Areas.Identity.Pages.Role
{
    [Authorize(Roles = "SUPERADMIN")]
    public class SetRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;


        public SetRoleModel(RoleManager<IdentityRole> roleManager,
                            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string Name { set; get; }

            public string[] RoleNames { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        //[BindProperty]
        //public bool isConfirmed { set; get; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public string CurId { set; get; }

        private async Task LoadAsync(AppUser user)
        {
            var id = await _userManager.GetUserIdAsync(user);
            CurId = id;

            Input = new InputModel
            {
                ID = CurId,
                Name = user.UserName
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("NotFound");
            }
            var allroles = await _roleManager.Roles.ToListAsync();

            allroles.ForEach((r) =>
            {
                AllRoles.Add(r.Name);
            });
            await LoadAsync(user);
            return Page();
        }

        public List<string> AllRoles { set; get; } = new List<string>();

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByIdAsync(Input.ID);
            if (user == null)
            {
                return NotFound("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allroles = await _roleManager.Roles.ToListAsync();

            allroles.ForEach((r) =>
            {
                AllRoles.Add(r.Name);
            });

            //if (!isConfirmed)
            //{
            //    Input.RoleNames = roles.ToArray();
            //    isConfirmed = true;
            //    StatusMessage = "";
            //    ModelState.Clear();
            //}
            //else
            //{
                // Update add and remove
                StatusMessage = "Vừa cập nhật";
                if (Input.RoleNames == null) Input.RoleNames = new string[] { };
                foreach (var rolename in Input.RoleNames)
                {
                    if (roles.Contains(rolename)) continue;
                    await _userManager.AddToRoleAsync(user, rolename);
                }
                foreach (var rolename in roles)
                {
                    if (Input.RoleNames.Contains(rolename)) continue;
                    await _userManager.RemoveFromRoleAsync(user, rolename);
                }

            //}

            Input.Name = user.UserName;
            return Page();
        }
    }
}
