using BussinessObject.DTOs;
using CourseManagementWebClient.DataHelper;
using CourseManagementWebClientWebClient.Controllers;
using CourseManagementWebClientWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourseManagementWebClient.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient client;
        private string CourseApiUrl = "";

        public CourseController(UserManager<AppUser> userManager)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CourseApiUrl = "https://localhost:7167/api/courses";
        }

        [Authorize(Roles = "TEACHER,STUDENT")]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage reponse = await client.GetAsync(CourseApiUrl);
            var courses = new List<CourseDTO>();
            try
            {
                courses = reponse.Content.ReadFromJsonAsync<List<CourseDTO>>().Result;
            } catch (Exception ex)
            {

            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<String> userRoles = HttpContext.Session.GetObjectFromJson<List<String>>("UserRoles");
            ViewData["UserRoles"] = userRoles;

            if (courses == null)
            {
                return Content("NOTHING TO SHOW");

            }

            return View(courses.Where(c => c.IsActive).ToList());
        }

        [Authorize(Roles = "TEACHER")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseDTO courseDTO)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsJsonAsync(CourseApiUrl, courseDTO))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Redirect("/Course/Index");
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var course = await GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(courseDTO);
            }
            HttpResponseMessage response = await client.PutAsJsonAsync(CourseApiUrl, courseDTO);
            return Redirect("/Course/Index");
        }

        [Authorize(Roles = "TEACHER,STUDENT")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await GetCourseById(id);
            if (product == null)
            {
                return NotFound();
            }
            List<String> userRoles = HttpContext.Session.GetObjectFromJson<List<String>>("UserRoles");
            ViewData["UserRoles"] = userRoles;
            return View(product);
        }

        private async Task<CourseDTO?> GetCourseById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            HttpResponseMessage response = await client.GetAsync(CourseApiUrl + "/id?id=" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content.ReadFromJsonAsync<CourseDTO>().Result;
            }
            return null;
        }


        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await GetCourseById(id);
            if (product == null)
            {
                return NotFound();
            }
            await client.DeleteAsync(CourseApiUrl + "/id?id=" + id);
            return Redirect("/Course/Index");
        }

        public IActionResult Topic(int? id)
        {
            ViewBag.CaseId = id;
            return View();
        }
    }
}
