using DAL;
using DAL.DTOs;

namespace BAL
{
    public class CourseBusiness
    {
        public static async Task<List<CourseDTO>> GetAllCourses()
        {
            try { return await CourseData.GetAllCourses(); }
            catch (Exception ex) { throw new ApplicationException("Error fetching courses", ex); }
        }

        public static async Task<List<CourseDTO>> GetCoursesByCategoryId(int categoryId)
        {
            try { return await CourseData.GetCoursesByCategoryId(categoryId); }
            catch (Exception ex) { throw new ApplicationException("Error fetching courses", ex); }
        }

        public static async Task<CourseDTO?> GetCourseById(int id)
        {
            try { return await CourseData.GetCourseById(id); }
            catch (Exception ex) { throw new ApplicationException("Error fetching course", ex); }
        }

        public static async Task<(bool, int)> CreateCourse(Course_Created c)
        {
            try { return await CourseData.CreateCourse(c); }
            catch (Exception ex) { throw new ApplicationException("Error creating course", ex); }
        }

        public static async Task<int> UpdateCourse(Course_Updated c)
        {
            try { return await CourseData.UpdateCourse(c); }
            catch (Exception ex) { throw new ApplicationException("Error updating course", ex); }
        }

        public static async Task<int> DeleteCourse(int id)
        {
            try { return await CourseData.DeleteCourse(id); }
            catch (Exception ex) { throw new ApplicationException("Error deleting course", ex); }
        }

        public static async Task<List<CourseDTO>> GetAllCoursesByUserId(int userid)
        {
            try { return await CourseData.GetAllCoursesByUserId(userid); }
            catch (Exception ex) { throw new ApplicationException("Error fetching courses", ex); }
        }
    }
}