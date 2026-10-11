using DAL;
using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BAL.SubTeamBusiness;

namespace BAL
{
    public static class SubTeamBusiness
    {

        public static async Task<List<SubTeamDTO>> GetAllSubTeams()
        {
            try
            {
                return await SubTeamData.GetAllSubTeams();
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetAllSubTeams", ex);
            }
        }

        public static async Task<List<SubTeamDTO>> GetAllTeamSubTeams(int TeamId)
        {
            try
            {
                return await SubTeamData.GetAllTeamSubTeams(TeamId);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetAllSubTeams", ex);
            }
        }



        public static async Task<SubTeamDTO?> GetSubTeamById(int id)
        {
            try
            {
                return await SubTeamData.GetSubTeamById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetSubTeamById", ex);
            }
        }

        public static async Task<(bool, int)> CreateSubTeam(SubTeam_Created_Request request)
        {
            try
            {
                List<string> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "SubTeams");
                    

              
                var subTeam = new SubTeam_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    TeamId = request.TeamId,
                    HeadId = request.HeadId,
                    Images = savedImages.Count > 0 ? string.Join(",", savedImages): null
                };

               
                return await SubTeamData.CreateSubTeam(subTeam);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error CreateSubTeam", ex);
            }
        }

        public static async Task<int> UpdateSubTeam(SubTeam_Updated_Request request)
        {
            try
            {
                List<string> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "SubTeams");

                var subTeam = new SubTeam_Updated
                {
                    SubTeamId = request.SubTeamId,
                    Title = request.Title,
                    Description = request.Description,
                    TeamId = request.TeamId,
                    HeadId = request.HeadId,
                    Images = savedImages.Count > 0 ? string.Join(",", savedImages) : null
                };

                return await SubTeamData.UpdateSubTeam(subTeam);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error UpdateSubTeam", ex);
            }
        }
        public static async Task<int> DeleteSubTeam(int id)
        {
            try
            {
                return await SubTeamData.DeleteSubTeam(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error DeleteSubTeam", ex);
            }
        }

        
            public static async Task<List<SubTeamMembersDTO>> GetSubTeamsByUserId(int userId)
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid UserId");

                return await SubTeamData.GetAllSubTeamsByUserId(userId);
            }

            public static async Task<List<SubTeamMembersDTO>> GetSubTeamMembersBySubTeamId(int subTeamId)
            {
                if (subTeamId <= 0)
                    throw new ArgumentException("Invalid SubTeamId");

                return await SubTeamData.GetAllSubTeamMembersBySubTeamId(subTeamId);
            }

            public static async Task<(bool, int)> Add(SubTeamMembers_Created member)
            {
                if (member == null)
                    throw new ArgumentNullException(nameof(member));

                if (member.MemberId <= 0 || member.SubTeamId <= 0)
                    throw new ArgumentException("Invalid Data");

                return await SubTeamData.AddMemberToSubTeam(member);
            }

            public static async Task<int> Remove(SubTeamMembers_Deleted member)
            {
                if (member == null)
                    throw new ArgumentNullException(nameof(member));

                if (member.MemberId <= 0 || member.SubTeamId <= 0)
                    throw new ArgumentException("Invalid Data");

                return await SubTeamData.RemoveSubTeamMember(member);
            }
        }
    }
