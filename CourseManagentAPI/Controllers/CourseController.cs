using AutoMapper;
using BussinessObject.DTOs;
using BussinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace CourseManagmentAPI.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : Controller
    {
        private ICourseRepository repository = new CourseRepository();
        private readonly IMapper mapper;

        public CourseController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDTO>> GetCourses()
        {
            List<CourseDTO> courses = repository.getCourses().Select(mapper.Map<Course, CourseDTO>).ToList();
            if (courses.Count == 0) {
                return NotFound("NOT FOUND ANY COURSE !!!");
            }
            return Ok(courses);
        }

        [HttpGet("id")]
        public ActionResult<IEnumerable<CourseDTO>> GetCourse(int id)
        {
            Course course = repository.getCourse(id);
            if (course == null)
            {
                return NotFound("COURSE NOT FOUND !!!");
            }
            CourseDTO courseDTO = mapper.Map<CourseDTO>(course);
            if (courseDTO.Topics != null || courseDTO.Topics.Count != 0)
            {
                foreach(var topic in course.Topics)
                {
                    MaterialDTO materialDTO = mapper.Map<MaterialDTO>(topic.Meterial);
                    var topicDto = courseDTO.Topics.First(t => t.Id == topic.Id);
                    topicDto.MaterialDTO= materialDTO;
                }
            }
            return Ok(courseDTO);
        }
        [HttpPost]
        public IActionResult AddCourse([FromBody] CourseDTO courseDTO)
        {
            Course course = mapper.Map<Course>(courseDTO);
            repository.addCourse(course);
            return Ok("Add Successfully");
        }
        [HttpPut]
        public IActionResult UpdateCourse([FromBody] CourseDTO courseDTO)
        {
            var course = repository.getCourse(courseDTO.Id);
            if (courseDTO == null)
            {
                return NotFound("COURSE NOT FOUND !!!");
            }
            course = mapper.Map<Course>(courseDTO);
            repository.updateCourse(course);
            return Ok("Update Successfully");
        }
        [HttpDelete("id")]
        public IActionResult DeleteCourse(int id)
        {
            var course = repository.getCourse(id);
            if (course == null)
            {
                return NotFound("COURSE NOT FOUND !!!");
            }
            repository.deleteCourse(course.Id);
            return Ok("Delete Successfully");
        }
    }
}
