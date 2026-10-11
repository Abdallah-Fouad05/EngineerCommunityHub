using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{

    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public SubTeam SubTeam { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public StatusDTO Status { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Images { get; set; }

        public DateTime? CreatedAt { get; set; }

        public user CreatedBy { get; set; }
    }

    public class Project()
    {
        public int ProjectId { get; set; }
        public String Title { get; set; }

        public string? Description { get; set; } = string.Empty;
    }
    public class Project_Created
    {
        public int SubTeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int StatusId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Images { get; set; }

        public int CreatedAt { get; set; }

        public int CreatedBy { get; set; }
    }

    // ============================
    // UPDATE DTO (DB)
    // ============================
    public class Project_Updated
    {
        public int ProjectId { get; set; }

        public int SubTeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int StatusId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string? Images { get; set; }
    }

    // ============================
    // CREATE REQUEST (API)
    // ============================
    public class Project_Created_Request
    {
        public int SubTeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int StatusId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public List<IFormFile>? Images { get; set; }

    }

    // ============================
    // UPDATE REQUEST (API)
    // ============================
    public class Project_Updated_Request
    {
        public int ProjectId { get; set; }

        public int SubTeamId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public int StatusId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public List<IFormFile>? Images { get; set; }
    }

    public class ProjectMemberDTO
    {
        public int ProjectMemberId { get; set; }
        public int ProjectId { get; set; }
        public user Member { get; set; }
        public string? RoleInProject { get; set; }
        public DateTime JoinedAt { get; set; }
    }
    public class ProjectMember_Created
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string? RoleInProject { get; set; }
    }

    public class MemberProjects
    {
        public int ProjectMemberId { get; set; }
        public Project Project { get; set; }

        public SubTeam SubTeam { get; set; }

        public DateTime? JoinAt { get; set; }

        public string? RoleInProject { get; set; } = "Member";
    }
}