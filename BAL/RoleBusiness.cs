using DAL;
using DAL.DTOs;

namespace BAL
{
    public class RoleBusiness
    {
        public static async Task<List<RoleDTO>> GetAllRoles()
        {
            try
            {
                return await RoleData.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all Roles.", ex);
            }
        }

        public static async Task<RoleDTO?> GetRoleById(int id)
        {
            try
            {
                return await RoleData.GetRoleById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching Role with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int RoleId)> CreateRole(Role_Created role)
        {
            try
            {
                return await RoleData.CreateRole(role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating role.", ex);
            }
        }

        public static async Task<int> UpdateRole(RoleDTO role)
        {
            try
            {
                return await RoleData.UpdateRole(role);
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
                return await RoleData.DeleteRole(RoleId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting category with ID {RoleId}.", ex);
            }
        }
    }
}