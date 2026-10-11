using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set;}
        public string? Description { get; set; }
        public RoleDTO(int roleId, string roleName,string? description)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
        }
    }

    public class Role_Created
    {
        public string RoleName { get; set; }
        public string? Description { get; set; }

        public Role_Created(string roleName,string? description) 
        {
            RoleName = roleName;
            Description = description;
        }
    }
}
