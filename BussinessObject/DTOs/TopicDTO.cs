using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTOs
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TeachingType teachingType { get; set; }
        public string? Description { get; set; }
        public string? StudentTask { get; set; }
        public int CourseId { get; set; }
        public int MeterialId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; } = false;
        public MaterialDTO? MaterialDTO { get; set; }

    }
}

