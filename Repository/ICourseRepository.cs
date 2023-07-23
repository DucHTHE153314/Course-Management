using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICourseRepository
    {
        void addCourse(Course course);
        void deleteCourse(int id);
        void updateCourse(Course course);
        Course getCourse(int id);
        List<Course> getCourses();
    }
}
