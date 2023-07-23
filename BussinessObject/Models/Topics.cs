using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace BussinessObject.Models
{
    public class Topics
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public TeachingType teachingType { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? StudentTask { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDelete { get; set; }
        [Required]
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public int? MeterialId { get; set; }

        [NotMapped]
        public Meterial? Meterial { get; set; }
    }
}
