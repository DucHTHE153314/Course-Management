using BussinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CourseRepository : ICourseRepository
    {
        public void addCourse(Course course)=> CourseDAO.addCourse(course);
        public void deleteCourse(int id) => CourseDAO.deleteCourse(id);
        public Course getCourse(int id) => CourseDAO.getCourseById(id);
        public List<Course> getCourses() => CourseDAO.getCourses();
        public void updateCourse(Course course) => CourseDAO.updateCourse(course);
    }
}
