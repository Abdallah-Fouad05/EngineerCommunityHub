using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{

    public class AchievementDTO
    {
        public int AchievementId { get; set; }
        public SubTeam SubTeam { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public string? CompetitionName { get; set; }
        public int? ProjectId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Videos { get; set; }
        public string? Images { get; set; }

        public DateTime? CreatedAt { get; set; }

        public user CreatedBy { get; set; }
    }

    public class Achievement_Created
    {
        public int SubTeamId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public string? CompetitionName { get; set; }
        public int? ProjectId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Videos { get; set; }
        public string? Images { get; set; }

        public int CreatedBy { get; set; }
    }

    public class Achievement_Updated
    {
        public int AchievementId { get; set; }

        public int SubTeamId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public string? CompetitionName { get; set; }
        public int? ProjectId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Videos { get; set; }
        public string? Images { get; set; }

    }

    public class Achievement_Created_Request
    {
        public int SubTeamId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public string? CompetitionName { get; set; }
        public int? ProjectId { get; set; }

        public DateTime? CompletedAt { get; set; }

        // لو هتعمل Upload ملفات
        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Videos { get; set; }
    }

    public class Achievement_Updated_Request
    {
        public int AchievementId { get; set; }

        public int SubTeamId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public string? CompetitionName { get; set; }
        public int? ProjectId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Videos { get; set; }
    }
}