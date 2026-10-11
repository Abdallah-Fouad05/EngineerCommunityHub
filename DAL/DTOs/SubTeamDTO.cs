using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    public class SubTeamDTO
    {
        public int SubTeamId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Team Team { get; set; }
        public user? Head { get; set; }
        public string? Images { get; set; }
    }

    public class SubTeamDetails
    {
        public int SubTeamId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public int TeamId { get; set; }

        public UserDTO? Head { get; set; }

        public string? Images { get; set; }
    }

    public class SubTeam
    {
        public int SubTeamId { get; set; }
        public string Title { get; set; }
    }
        public class SubTeam_Created
        {
            public string Title { get; set; }
            public string? Description { get; set; }

            public int TeamId { get; set; }
            public int HeadId { get; set; }

            public string? Images { get; set; }
        }

        public class SubTeam_Updated
        {
            public int SubTeamId { get; set; }

            public string Title { get; set; }
            public string? Description { get; set; }

            public int TeamId { get; set; }
            public int? HeadId { get; set; }

            public string? Images { get; set; }
        }


        public class SubTeam_Created_Request
        {
            public string Title { get; set; }
            public string? Description { get; set; }

            public int HeadId { get; set; }
            public int TeamId { get; set; }

            public List<IFormFile>? Images { get; set; }
        }

        public class SubTeam_Updated_Request
        {
            public int SubTeamId { get; set; }

            public string Title { get; set; }
            public string? Description { get; set; }

            public int HeadId { get; set; }
            public int TeamId { get; set; }

            public List<IFormFile>? Images { get; set; }
        }

        public class SubTeamMembersDTO
        {
            public int Id { get; set; }
            public user Member { get; set; }
            public SubTeam SubTeam { get; set; }
            public DateTime? JoinAt { get; set; }
        }

        public class SubTeamMembers_Created
        {
            public int MemberId { get; set; }
            public int SubTeamId { get; set; }
            public DateTime? JoinAt { get; set; }
        }

        public class SubTeamMembers_Deleted
        {
            public int MemberId { get; set; }
            public int SubTeamId { get; set; }
        }
    }
