using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.DTOs
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Location { get; set; }

        public user CreatedBy { get; set; }
        public StatusDTO Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class Event_Created
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Location { get; set; }

        public int CreatedBy { get; set; }
    }

    public class Event_Updated
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
    }
    public class Event_Created_Request
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Location { get; set; }
    }

    public class Event_Updated_Request
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
    }
}
