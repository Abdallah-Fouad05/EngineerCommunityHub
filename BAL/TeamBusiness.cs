using DAL;
using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BAL
{
    public static class TeamBusiness
    {
    
        public static async Task<List<TeamDTO>> GetAllTeams()
        {
            try
            {
                return await TeamData.GetAllTeams();
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetAllTeams", ex);
            }
        }
        public static async Task<List<TeamDTO>> GetAllBranchTeams(int BranchId)
        {
            try
            {
                return await TeamData.GetAllBranchTeams(BranchId);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetAllTeams", ex);
            }
        }


        public static async Task<TeamDTO?> GetTeamById(int id)
        {
            try
            {
                return await TeamData.GetTeamById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetTeamById", ex);
            }
        }

        
        public static async Task<(bool, int)> CreateTeam(Team_Created_Request request)
        {
            try
            {
              
                List<string> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "Teams");

                var team = new Team_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    BranchId = request.BranchId,
                    DirectorId = request.DirectorId,
                    Images = savedImages.Count > 0 ? string.Join(",", savedImages) : null
                };

                // 4️⃣ Call DAL
                return await TeamData.CreateTeam(team);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error CreateTeam", ex);
            }
        }

        public static async Task<int> UpdateTeam(Team_Updated_Request request)
        {
            try
            {
                List<string> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "Teams");


            
                var team = new Team_Updated
                {
                    TeamId = request.TeamId,
                    Title = request.Title,
                    Description = request.Description,
                    BranchId = request.BranchId,
                    DirectorId = request.DirectorId,
                    Images = savedImages.Count > 0 ? string.Join(",", savedImages) : null
                };

                return await TeamData.UpdateTeam(team);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error UpdateTeam", ex);
            }
        }

        public static async Task<int> DeleteTeam(int id)
        {
            try
            {
                return await TeamData.DeleteTeam(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error DeleteTeam", ex);
            }
        }
    }
}