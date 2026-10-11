using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    public class BranchDTO
    {
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public user Leader {  get; set; }
        public string? Images {  get; set; }
    }

    public class Branch
    {
          public int BranchId { get; set; }
          public string Title { get; set; }
    }

    public class BranchDetails
    {
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public UserDTO Leader { get; set; }
        public string? Images
        {
            get; set;
        }
    }
    public class Branch_Created
    {
        
        public string Title { get; set; }
        public string? Description { get; set; }
        public int LeaderId { get; set; }
        public string? Images { get; set; }
    }

    public class Branch_Updated
    {
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int LeaderId { get; set; }
        public string? Images { get; set; }
    }

    public class Branch_Created_Request
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int LeaderId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class Branch_Updated_Request
    {
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int LeaderId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }



}
