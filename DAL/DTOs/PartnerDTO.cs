using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    // ============================
    // MAIN DTO (OUTPUT)
    // ============================
    public class PartnerDTO
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public StatusDTO? Status { get; set; }
    }

   
    public class Partner
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string? PartnerImage { get; set; }
        public string? Link { get; set; }
    }

    public class Partner_Created
    {
        public string PartnerName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
    }

    // ============================
    // UPDATE DTO (للـ DB)
    // ============================
    public class Partner_Updated
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int StatusId { get; set; }
    }

    // ============================
    // CREATE REQUEST (API)
    // ============================
    public class Partner_Created_Request
    {
        public string PartnerName { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public IFormFile? Image { get; set; }
    }

    // ============================
    // UPDATE REQUEST (API)
    // ============================
    public class Partner_Updated_Request
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int StatusId { get; set; }
        public IFormFile? Image { get; set; }
    }
}