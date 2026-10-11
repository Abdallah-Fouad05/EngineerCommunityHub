using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class RoleData
    {
        public static async Task<List<RoleDTO>> GetAllRoles()
        {
            List<RoleDTO> Roles = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_Roles", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Roles.Add(new RoleDTO
                        (
                            roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                            roleName: reader.GetString(reader.GetOrdinal("RoleName")),
                            description: reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                null : reader.GetString(reader.GetOrdinal("Description")) 
                        )
                    );
                }
                return Roles;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Roles  : ", ex);
            }
        }
        public static async Task<RoleDTO?> GetRoleById(int id)
        {

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Role_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@RoleId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new RoleDTO(
                             roleId: reader.GetInt32(reader.GetOrdinal("RoleId")),
                            roleName: reader.GetString(reader.GetOrdinal("RoleName")),
                            description: reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                null : reader.GetString(reader.GetOrdinal("Description"))
                     );

                }
                return null;

            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get Role By RoleId : ", ex);
            }


        }
        public static async Task<(bool, int)> CreateRole(Role_Created role)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Create_Role", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@RoleName", string.IsNullOrEmpty(role.RoleName) ? DBNull.Value :  role.RoleName);
            comd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(role.Description)? DBNull.Value : role.Description);
            SqlParameter outputParam = new SqlParameter("@NewRoleId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            comd.Parameters.Add(outputParam);

            try
            {
                await connection.OpenAsync();

                int rowsAffected = await comd.ExecuteNonQueryAsync();

                int newId = outputParam.Value == DBNull.Value ? -1 : Convert.ToInt32(outputParam.Value);

                return (newId > 0, newId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint
                {
                    return (false, -2); // Duplicate
                }
                else
                {
                    throw new Exception("DB Error Create Role.", ex);
                }
            }
        }
        public static async Task<int> UpdateRole(RoleDTO role)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Update_Role", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RoleId",role.RoleId);

            command.Parameters.AddWithValue("@RoleName",
               string.IsNullOrEmpty(role.RoleName)? DBNull.Value : role.RoleName);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(role.Description)? DBNull.Value : role.Description);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint
                {
                    return (-2); // Duplicate
                }
                else
                {
                    throw new Exception("DB Error Update Role", ex);
                }
            }
        }
        public static async Task<int> DeleteRole(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Delete_Role", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RoleId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Delete Role", ex);
            }
        }
    }
}
