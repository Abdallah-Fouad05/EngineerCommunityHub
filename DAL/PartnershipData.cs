using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class PartnershipData
    {
        //SHOW PARTNERSHIP FOR ADMIN
        public static async Task<List<PartnershipDTO>> GetAllPartnerships()
        {
            List<PartnershipDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_Partnerships", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new PartnershipDTO
                    {
                        PartnershipId = reader.GetInt32(reader.GetOrdinal("PartnershipId")),
                        Partner = new Partner
                        {
                            PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                            PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                            PartnerImage = reader.IsDBNull(reader.GetOrdinal("PartnerImage")) ? null : reader.GetString(reader.GetOrdinal("PartnerImage")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),

                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        CreatedBy = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
  : new CommunityRoleDTO(
                                                reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                    ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                null),

                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }

                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPartnerships", ex);
            }
        }
        //SHOW PARTNERSHIP FOR USER
        public static async Task<List<PartnershipDTO>> GetAllActivePartnerships()
        {
            List<PartnershipDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_ActivePartnershipsDetails", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new PartnershipDTO
                    {
                        PartnershipId = reader.GetInt32(reader.GetOrdinal("PartnershipId")),
                        Partner = new Partner
                        {
                            PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                            PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                            PartnerImage = reader.IsDBNull(reader.GetOrdinal("PartnerImage")) ? null : reader.GetString(reader.GetOrdinal("PartnerImage")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),

                        Status = new StatusDTO
                        (
                            statusId : reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName : reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        CreatedBy = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
  : new CommunityRoleDTO(
                                                reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                    ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }

                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPartnerships", ex);
            }
        }
        public static async Task<List<PartnershipDTO>> GetAllPartnershipsByUserId(int UserId)
        {
            List<PartnershipDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_Partnerships_ByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@UserId", UserId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new PartnershipDTO
                    {
                        PartnershipId = reader.GetInt32(reader.GetOrdinal("PartnershipId")),
                        Partner = new Partner
                        {
                            PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                            PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                            PartnerImage = reader.IsDBNull(reader.GetOrdinal("PartnerImage")) ? null : reader.GetString(reader.GetOrdinal("PartnerImage")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),

                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        CreatedBy = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
  : new CommunityRoleDTO(
                                                reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                    ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }

                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPartnerships", ex);
            }
        }
        public static async Task<PartnershipDTO?> GetPartnershipById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Get_Partnership_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnershipId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new PartnershipDTO
                    {
                        PartnershipId = reader.GetInt32(reader.GetOrdinal("PartnershipId")),
                        Partner = new Partner
                        {
                            PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                            PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                            PartnerImage = reader.IsDBNull(reader.GetOrdinal("PartnerImage")) ? null : reader.GetString(reader.GetOrdinal("PartnerImage")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),

                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        CreatedBy = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
                                               : new CommunityRoleDTO(
                                                reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                    ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }
                    };
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetPartnershipById", ex);
            }
        }      
        public static async Task<(bool, int)> CreatePartnership(Partnership_Created p)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Create_Partnership", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnerId", p.PartnerId);
            comd.Parameters.AddWithValue("@Title", p.Title);
            comd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(p.Description) ? DBNull.Value : p.Description);
            comd.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(p.Image) ? DBNull.Value : p.Image);
            comd.Parameters.AddWithValue("@UserId",p.CreatedBy);
            SqlParameter output = new SqlParameter("@NewPartnershipId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            comd.Parameters.Add(output);

            try
            {
                await connection.OpenAsync();
                await comd.ExecuteNonQueryAsync();

                int newId = output.Value == DBNull.Value ? -1 : Convert.ToInt32(output.Value);

                return (newId > 0, newId);
            }
            catch (Exception ex)
            {
                throw new Exception("DB Error CreatePartnership", ex);
            }
        }
        public static async Task<int> UpdatePartnership(Partnership_Updated p)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Update_Partnership", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnershipId", p.PartnershipId);
            comd.Parameters.AddWithValue("@PartnerId", p.PartnerId);
            comd.Parameters.AddWithValue("@Title", p.Title);
            comd.Parameters.AddWithValue("@Description", p.Description);
            comd.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(p.Image) ? DBNull.Value : p.Image);
            comd.Parameters.AddWithValue("@StatusId", p.StatusId);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await comd.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdatePartnership", ex);
            }
        }
        public static async Task<int> DeletePartnership(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Delete_Partnership", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnershipId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await comd.ExecuteScalarAsync();

                return resultObj == null || resultObj == DBNull.Value ? -1 : Convert.ToInt32(resultObj);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeletePartnership", ex);
            }
        }
    }
}