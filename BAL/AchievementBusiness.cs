using DAL;
using DAL.DTOs;

namespace BAL
{
    public class AchievementBusiness
    {
      
        public static async Task<List<AchievementDTO>> GetAllAchievements()
        {
            try
            {
                return await AchievementData.GetAllAchievements();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all achievements.", ex);
            }
        }

        public static async Task<List<AchievementDTO>> GetAllAchievementsByUserId(int UserId)
        {
            try
            {
                return await AchievementData.GetAllAchievementsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all achievements.", ex);
            }
        }

        
        public static async Task<AchievementDTO?> GetAchievementById(int id)
        {
            try
            {
                return await AchievementData.GetAchievementById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching achievement with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int achievementId)> CreateAchievement(Achievement_Created achievement)
        {
            try
            {
                return await AchievementData.CreateAchievement(achievement);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating achievement.", ex);
            }
        }

       
        public static async Task<int> UpdateAchievement(Achievement_Updated achievement)
        {
            try
            {
                return await AchievementData.UpdateAchievement(achievement);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating achievement with ID {achievement.AchievementId}.", ex);
            }
        }

    
        public static async Task<int> DeleteAchievement(long achievementId)
        {
            try
            {
                return await AchievementData.DeleteAchievement(achievementId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting achievement with ID {achievementId}.", ex);
            }
        }
    }
}