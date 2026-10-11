using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{

    public class PartnershipDTO
    {
        public int PartnershipId { get; set; }
        public Partner Partner { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public StatusDTO? Status { get; set; }

        public DateTime? CreatedAt
        {
            get; set;
        }

        public user CreatedBy { get; set; }
    }
        public class PartnershipDetails
        {
            public int PartnershipId { get; set; }
            public PartnerDTO Partner { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string? Image { get; set; }
            public StatusDTO? Status { get; set; }
            public DateTime? CreatedAt { get; set; }
        }

        // ============================
        // CREATE DTO (للـ DB)
        // ============================
        public class Partnership_Created
        {
            public int PartnerId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string? Image { get; set; }

            public int CreatedBy { get; set; }
        }
        public class Partnership_Updated
        {
            public int PartnershipId { get; set; }
            public int PartnerId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string? Image { get; set; }
            public int StatusId { get; set; }
        }

        public class Partnership_Created_Request
        {
            public int PartnerId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public IFormFile? Image { get; set; }
        }



        public class Partnership_Updated_Request
        {
            public int PartnershipId { get; set; }
            public int PartnerId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int StatusId { get; set; }

            public IFormFile? Image { get; set; }
        }
}
