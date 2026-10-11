using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL;

namespace BAL
{
    public class CommunityRoleBusiness
    {
        public static async Task<List<CommunityRoleDTO>> GetAllRoles()
        {
            try
            {
                return await CommunityRoleData.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all Roles.", ex);
            }
        }

        public static async Task<CommunityRoleDTO?> GetRoleById(int id)
        {
            try
            {
                return await CommunityRoleData.GetRoleById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching Role with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int RoleId)> CreateRole(CommunityRole_Created role)
        {
            try
            {
                return await CommunityRoleData.CreateRole(role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating role.", ex);
            }
        }

        public static async Task<int> UpdateRole(CommunityRoleDTO role)
        {
            try
            {
                return await CommunityRoleData.UpdateRole(role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating Role with ID {role.RoleId}.", ex);
            }
        }

        public static async Task<int> DeleteRole(int RoleId)
        {
            try
            {
                return await CommunityRoleData.DeleteRole(RoleId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting category with ID {RoleId}.", ex);
            }
        }
    }
}
