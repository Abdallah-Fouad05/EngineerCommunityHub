using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL
{
    public static class CategoryData
    {
        public static async Task<List<CategoryDTO>> GetAllCategories()
        {
            List<CategoryDTO> Categories = new();

            using SqlConnection connection =  new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_Categories", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Categories.Add(new CategoryDTO
                        (
                            categoryId: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            categoryName: reader.GetString(reader.GetOrdinal("CategoryName")),
                            description: reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                null : reader.GetString(reader.GetOrdinal("Description")),
                            image: reader.IsDBNull(reader.GetOrdinal("Image")) ?
                                null : reader.GetString(reader.GetOrdinal("Image"))
                        )
                    );
                }
                return Categories;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllCategories  : ", ex);
            }
        }
        public static async Task<CategoryDTO?> GetCategoryById(int id)
        {

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Category_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@CategoryId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if(await reader.ReadAsync())
                {
                    return new CategoryDTO(
                            categoryId: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            categoryName: reader.GetString(reader.GetOrdinal("CategoryName")),
                            description: reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                null : reader.GetString(reader.GetOrdinal("Description")),
                            image: reader.IsDBNull(reader.GetOrdinal("Image")) ?
                                null : reader.GetString(reader.GetOrdinal("Image"))
                     );

                }
                return null;

            }
            catch(SqlException ex)
            {
                throw new Exception("DB Error GetCategoryByCategoryId : ", ex);
            }


        }
        public static async Task<(bool,int)> CreateCategory(Category_Created category)
        {
            await using SqlConnection connection = new SqlConnection (Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Create_Category", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@CategoryName", string.IsNullOrEmpty(category.CategoryName) ? DBNull.Value : category.CategoryName);
            comd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(category.Description) ? DBNull.Value : category.Description);
            comd.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(category.Image) ? DBNull.Value : category.Image);
            SqlParameter outputParam = new SqlParameter("@NewCategoryId", SqlDbType.Int)
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
                    throw new Exception("DB Error CreateCategory.", ex);
                }
            }
        }
        public static async Task<int> UpdateCategory(CategoryDTO category)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Update_Category", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryId", category.CategoryId);

            command.Parameters.AddWithValue("@CategoryName",
                string.IsNullOrEmpty(category.CategoryName) ? DBNull.Value  : category.CategoryName);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(category.Description) ?  DBNull.Value : category.Description);

            command.Parameters.AddWithValue("@Image",
                string.IsNullOrEmpty(category.Image) ? DBNull.Value : category.Image);

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
                    return -2; // Duplicate
                }
                else
                {
                    throw new Exception("DB Error Update Category.", ex);
                }
            }
        }
        public static async Task<int> DeleteCategory(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand command = new SqlCommand("SP_Delete_Category", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteCategory", ex);
            }
        }
    }
}
