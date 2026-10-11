using DAL;
using DAL.DTOs;

namespace BAL
{
    public class InstructorCourseBusiness
    {
        public static async Task<(bool, long)> Create(InstructorCourse_Created model)
        {
            try
            {
                return await InstructorCourseData.Create(model);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating InstructorCourse", ex);
            }
        }
        public static async Task<List<UserDTO?>> GetAllInstructors()
        {
            try
            {
                return await InstructorCourseData.GetAllInstructors();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting Instructors", ex);
            }
        }
        public static async Task<List<InstructorCourseDTO?>> GetInstructorsByCourseId(int id)
        {
            try
            {
                return await InstructorCourseData.GetInstructorsByCourseId(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting InstructorCourses", ex);
            }
        }

        
        public static async Task<List<InstructorCourse?>> GetCoursesByInstructorId(int id)
        {
            try
            {
                return await InstructorCourseData.GetCoursesByInstructorId(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting InstructorCourses", ex);
            }
        }

        public static async Task<long> Delete(InstructorCourse_Created ct)
        {
            try
            {
                return await InstructorCourseData.Delete(ct);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting InstructorCourse", ex);
            }
        }
    }
}