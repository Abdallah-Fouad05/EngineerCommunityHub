using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class InstructorCourseData
    {
        public static async Task<List<UserDTO>> GetAllInstructors()
        {
            List<UserDTO> users = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_All_ActiveInstructors", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("UserImage")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: reader.IsDBNull(reader.GetOrdinal("CommunityRoleId"))? null
                                              : new CommunityRoleDTO(
                                                    reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                     reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                       ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                     null
                                               )
                     ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Instructors", ex);
            }
        }
        public static async Task<(bool, int)> Create(InstructorCourse_Created model)
        {
            await using SqlConnection conn = new SqlConnection(Setting.Connection);

            await using SqlCommand cmd = new SqlCommand("SP_Add_Instructor_To_Course", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@InstructorId", model.InstructorId);
            cmd.Parameters.AddWithValue("@CourseId", model.CourseId);

            SqlParameter output = new SqlParameter("@NewId", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            try
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int result = output.Value == DBNull.Value ? -1 : Convert.ToInt32(output.Value);

                return (result > 0, result);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return (false, -2);
                }
                else
                {
                    throw new Exception("DB Error Create InstructorCourse", ex);
                }
            }   
        }
        public static async Task<List<InstructorCourseDTO?>> GetInstructorsByCourseId(int id)
        {
            List<InstructorCourseDTO?> result = new();
            await using SqlConnection conn = new SqlConnection(Setting.Connection);

            await using SqlCommand cmd = new SqlCommand("SP_Get_All_InstructorsCourse", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CourseId", id);

            await conn.OpenAsync();
            await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new InstructorCourseDTO
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Instructor = new user
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
  :                                     new CommunityRoleDTO(
                                                reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                                reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                                    ? null
                                                    : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                                null),
                        Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                    },
                    CourseId = reader.GetInt32(reader.GetOrdinal("CourseId")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return result;

           
        }
        public static async Task<List<InstructorCourse?>> GetCoursesByInstructorId(int id)
        {
            List<InstructorCourse?> result = new();
            await using SqlConnection conn = new SqlConnection(Setting.Connection);

            await using SqlCommand cmd = new SqlCommand("SP_Get_All_CoursesByInstructorId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@InstructorId", id);

            await conn.OpenAsync();
            await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new InstructorCourse
                {
                    InstructorCourseId = reader.GetInt32(reader.GetOrdinal("Id")),
                    Course = new Course
                    {
                        CourseId = reader.GetInt32(reader.GetOrdinal("CourseId")),
                        CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
                        Category = new category
                            (
                                reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                reader.GetString(reader.GetOrdinal("CategoryName"))
                            ),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null : reader.GetString(reader.GetOrdinal("Image")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ?
                                null : reader.GetString(reader.GetOrdinal("Link"))
                    },
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            return result;


        }
        public static async Task<int> Delete(InstructorCourse_Created ct)
        {
            await using SqlConnection conn = new SqlConnection(Setting.Connection);

            await using SqlCommand cmd = new SqlCommand("SP_Remove_InstructorCourse", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CourseId", ct.CourseId);
            cmd.Parameters.AddWithValue("InstructorId", ct.InstructorId);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            return result == null ? -1 : Convert.ToInt32(result);
        }
    }
}