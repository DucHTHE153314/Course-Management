using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementWebClientWebClient.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]

        public string? Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? Address { get; set; }
    
    }
}
