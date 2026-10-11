using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
   
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public Branch Branch { get; set; }

        public user Director { get; set; }

        public string? Images { get; set; }
    }

    public class Team
    {
        public int TeamId { get; set; }
        public string Title { get; set; }
    }
    public class TeamDetails
    {
        public int TeamId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }

        public UserDTO? Director { get; set; }

        public string? Images { get; set; }
    }


    public class Team_Created
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }
        public int DirectorId { get; set; }

        public string? Images { get; set; }
    }


    public class Team_Updated
    {
        public int TeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }
        public int? DirectorId { get; set; }

        public string? Images { get; set; }
    }


    public class Team_Created_Request
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public int DirectorId { get; set; }
        public int BranchId { get; set; }

        public List<IFormFile>? Images { get; set; }
    }

 
    public class Team_Updated_Request
    {
        public int TeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int DirectorId { get; set; }
        public int BranchId { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}