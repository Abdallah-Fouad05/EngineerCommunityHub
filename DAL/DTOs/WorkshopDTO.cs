using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{

    public class WorkshopDTO
    {
        public int WorkshopId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Topics { get; set; }
        public DateTime WorkshopDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string Location { get; set; }

        public string? Link {  get; set; }

        public user CreatedBy { get; set; }
        public string? Images { get; set; }

        public StatusDTO Status { get; set; }
    }

   
    public class WorkshopDetails
    {
        public int WorkshopId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Topics { get; set; }
        public string? Link { get; set; }

        public DateTime WorkshopDate { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string Location { get; set; }

        public UserDTO CreatedBy { get; set; }
        public string? Images { get; set; }
    }


    public class Workshop_Created
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Topics { get; set; }

        public string? Link { get; set; }


        public DateTime WorkshopDate { get; set; }


        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string? Location { get; set; }

        public int CreatedBy { get; set; }
        public string? Images { get; set; }
    }


    public class Workshop_Updated
    {
        public int WorkshopId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? Topics { get; set; }
        public DateTime WorkshopDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Location { get; set; }
        public string? Images { get; set; }
    }

  
    public class Workshop_Created_Request
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Topics { get; set; }
        public DateTime WorkshopDate { get; set; }
        public string? Link { get; set; }


        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string? Location { get; set; }

        public List<IFormFile>? Images { get; set; }
    }

    public class Workshop_Updated_Request
    {
        public int WorkshopId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Topics { get; set; }
        public string? Link { get; set; }
        public DateTime WorkshopDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Location { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}