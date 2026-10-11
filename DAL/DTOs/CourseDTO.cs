using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    // ============================
    // MAIN DTO (OUTPUT)
    // ============================
    public class CourseDTO
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }

        public string? Requirements { get; set; }
        public string? Skills { get; set; }

        public string? Link { get; set; }

        public category? Category { get; set; }

        public user? CreatedBy { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public category? Category { get; set; }

    }

    public class Course_Created
    {
        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }

        public string? Requirements { get; set; }
        public string? Skills { get; set; }

        public string? Link { get; set; }

        public int CategoryId { get; set; }

        public int CreatedBy { get; set; }
    }


    public class Course_Updated
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }

        public string? Requirements { get; set; }
        public string? Skills { get; set; }

        public string? Link { get; set; }

        public int CategoryId { get; set; }
    }

    public class Course_Created_Request
    {
        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }

        public string? Requirements { get; set; }
        public string? Skills { get; set; }

        public string? Link { get; set; }

        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; }

    }

    public class Course_Updated_Request
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }

        public string? Requirements { get; set; }
        public string? Skills { get; set; }

        public string? Link { get; set; }

        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }
}