using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class CommunityRoleDTO
    {
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public CommunityRoleDTO(int? roleId, string? roleName, string? description)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
        }
    }

    public class CommunityRole_Created
    {
        public string RoleName { get; set; }
        public string? Description { get; set; }

        public CommunityRole_Created(string roleName, string? description)
        {
            RoleName = roleName;
            Description = description;
        }
    }
}
