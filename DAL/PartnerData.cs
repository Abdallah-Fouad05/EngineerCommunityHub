using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class PartnerData
    {
        public static async Task<List<PartnerDTO>> GetAllPartners()
        {
            List<PartnerDTO> partners = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_Partners", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    partners.Add(new PartnerDTO
                    {
                        PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                        PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Status = new StatusDTO
                        (
                            statusId : reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        )
                    });
                }

                return partners;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPartners : ", ex);
            }
        }
        public static async Task<List<PartnerDTO>> GetAllActivePartners()
        {
            List<PartnerDTO> partners = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_ActivePartners", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    partners.Add(new PartnerDTO
                    {
                        PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                        PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        )
                    });
                }

                return partners;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPartners : ", ex);
            }
        }
        public static async Task<PartnerDTO?> GetPartnerById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Get_Partner_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnerId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new PartnerDTO
                    {
                        PartnerId = reader.GetInt32(reader.GetOrdinal("PartnerId")),
                        PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        )
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetPartnerById : ", ex);
            }
        }
        public static async Task<(bool, int)> CreatePartner(Partner_Created partner)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Create_Partner", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PartnerName",
                string.IsNullOrEmpty(partner.PartnerName) ? DBNull.Value : partner.PartnerName);

            comd.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(partner.Description) ? DBNull.Value : partner.Description);

            comd.Parameters.AddWithValue("@Link",
                string.IsNullOrEmpty(partner.Link) ? DBNull.Value : partner.Link);

            comd.Parameters.AddWithValue("@Image",
                string.IsNullOrEmpty(partner.Image) ? DBNull.Value : partner.Image);

            SqlParameter outputParam = new SqlParameter("@NewPartnerId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            comd.Parameters.Add(outputParam);

            try
            {
                await connection.OpenAsync();

                await comd.ExecuteNonQueryAsync();

                int newId = outputParam.Value == DBNull.Value ? -1 : Convert.ToInt32(outputParam.Value);

                return (newId > 0, newId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return (false, -2);
                }
                else
                {
                    throw new Exception("DB Error Create Partner.", ex);
                }
            }
        }
        public static async Task<int> UpdatePartner(Partner_Updated partner)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Update_Partner", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@PartnerId", partner.PartnerId);

            command.Parameters.AddWithValue("@PartnerName",
                string.IsNullOrEmpty(partner.PartnerName) ? DBNull.Value : partner.PartnerName);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(partner.Description) ? DBNull.Value : partner.Description);

            command.Parameters.AddWithValue("@Link",
                string.IsNullOrEmpty(partner.Link) ? DBNull.Value : partner.Link);

            command.Parameters.AddWithValue("@Image",
                string.IsNullOrEmpty(partner.Image) ? DBNull.Value : partner.Image);

            command.Parameters.AddWithValue("@StatusId", partner.StatusId);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1
                    : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return (-2);
                }
                else
                {
                    throw new Exception("DB Error Update Partner.", ex);
                }
            }
        }
        public static async Task<int> DeletePartner(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Delete_Partner", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@PartnerId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1
                    : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeletePartner", ex);
            }
        }
    }
}