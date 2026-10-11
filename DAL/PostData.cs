using System.Data;
using Microsoft.Data.SqlClient;
using DAL.DTOs;

namespace DAL
{
    public static class PostData
    {    
        public static async Task<List<PostDTO>> GetAllPosts()
        {
            List<PostDTO> posts = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_All_Posts", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(new PostDTO
                    {
                        PostId = reader.GetInt32(reader.GetOrdinal("PostID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos"))
                            ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
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
                        },
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    });
                }

                return posts;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPosts : ", ex);
            }
        }    
        public static async Task<PostDTO?> GetPostById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Post_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@PostID", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new PostDTO { 
                        PostId = reader.GetInt32(reader.GetOrdinal("PostID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos"))
                            ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
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
                        },
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),

                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetPostById : ", ex);
            }
        }
        public static async Task<List<PostDTO?>> GetPostsByUserId(int id)
        {
            List<PostDTO> posts = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Posts_ByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@UserId",id);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(new PostDTO
                    {
                        PostId = reader.GetInt32(reader.GetOrdinal("PostID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos"))
                            ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
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
                        },
                         CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),

                    });
                }

                return posts;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllPosts : ", ex);
            }
        }
        public static async Task<(bool, int)> CreatePost(Post_Created post)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Create_Post", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@Title", post.Title);
            comd.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(post.Description) ? DBNull.Value : post.Description);
            comd.Parameters.AddWithValue("@Videos",
                string.IsNullOrEmpty(post.Videos) ? DBNull.Value : post.Videos);
            comd.Parameters.AddWithValue("@Link",
                string.IsNullOrEmpty(post.Link) ? DBNull.Value : post.Link);
            comd.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(post.Images) ? DBNull.Value : post.Images);
            comd.Parameters.AddWithValue("@UserId", post.CreatedBy);

            SqlParameter outputParam = new SqlParameter("@NewPostId", SqlDbType.Int)
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
            catch (Exception ex)
            {
                throw new Exception("DB Error CreatePost.", ex);
            }
        }  
        public static async Task<int> UpdatePost(Post_Updated post)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_Post", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@PostID", post.PostId);
            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(post.Description) ? DBNull.Value : post.Description);
            command.Parameters.AddWithValue("@Videos",
                string.IsNullOrEmpty(post.Videos) ? DBNull.Value : post.Videos);
            command.Parameters.AddWithValue("@Link",
                string.IsNullOrEmpty(post.Link) ? DBNull.Value : post.Link);
            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(post.Images) ? DBNull.Value : post.Images);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdatePost", ex);
            }
        }
        public static async Task<int> DeletePost(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_Post", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@PostID", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeletePost", ex);
            }
        }
    }
}