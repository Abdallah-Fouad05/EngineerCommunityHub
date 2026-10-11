using System;

namespace DAL.DTOs
{
    public class InstructorCourseDTO
    {
        public int Id { get; set; }
        public user Instructor { get; set; }
        public int CourseId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class InstructorCourse
    {
        public int InstructorCourseId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Course Course { get; set; }
    }
    public class InstructorCourse_Created
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
    }

    public class InstructorCourse_Updated
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
    }
}