using BussinessObject.Context;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CourseDAO
    {
        //todo
        public static List<Course> getCourses()
        {
            var courses = new List<Course>();
            try
            {
                using (var context = new MyDbContext())
                {
                    courses = context.Course.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return courses;
        }
        //todo
        public static Course getCourseById(int id)
        {
            Course course = new Course();
            try
            {
                using (var context = new MyDbContext())
                {
                    course = context.Course
                        .Include(topic => topic.Topics)
                        .Where(o => o.Id == id).FirstOrDefault();
                    foreach(var topic in course.Topics)
                    {
                        int masterialId = topic.MeterialId.Value;
                        if (masterialId != null || masterialId != 0)
                        {
                            var masterial = context.Meterials.FirstOrDefault(m => m.Id == masterialId);
                            topic.Meterial = masterial;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return course;
        }

        //todo
        public static void addCourse(Course course)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Course.Add(course);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //todo
        public static void updateCourse(Course course)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Course>(course).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //todo
        public static void deleteCourse(int id)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var course = context.Course.SingleOrDefault(c => c.Id == id);
                    context.Course.Remove(course);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
