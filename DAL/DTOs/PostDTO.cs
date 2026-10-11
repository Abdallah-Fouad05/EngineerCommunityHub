using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.DTOs
{
    public class PostDTO
    {
     
        public int PostId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Videos {  get; set; }
        public string? Link { get; set; }
        public string? Images { get; set; }
        public user CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

    }

    public class Post_Created
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Videos { get; set; }
        public string? Link { get; set; }
        public string? Images { get; set; }

        public int CreatedBy { get; set; }
    }

    public class Post_Updated
    {
        public int PostId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Videos { get; set; }
        public string? Link { get; set; }
        public string? Images { get; set; }
    }
    public class Post_Created_Request
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Link { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Videos { get; set; }

    }

    public class Post_Updated_Request
    {
        public int PostId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Link { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Videos { get; set; }
    }

}
