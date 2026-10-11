using DAL;
using DAL.DTOs;

namespace BAL
{
    public class WorkshopBusiness
    {
      
        public static async Task<List<WorkshopDTO>> GetAllWorkshops()
        {
            try
            {
                return await WorkshopData.GetAllWorkshops();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all workshops.", ex);
            }
        }

        public static async Task<List<WorkshopDTO>> GetAllWorkshopsByStatusId(int StatusId)
        {
            try
            {
                return await WorkshopData.GetAllWorkshopsByStatusId(StatusId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all workshops.", ex);
            }
        }

        public static async Task<List<WorkshopDTO>> GetAllWorkshopsByUserId(int UserId)
        {
            try
            {
                return await WorkshopData.GetAllWorkshopsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all workshops.", ex);
            }
        }


        public static async Task<WorkshopDTO?> GetWorkshopById(int id)
        {
            try
            {
                return await WorkshopData.GetWorkshopById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching workshop with ID {id}.", ex);
            }
        }

        
        public static async Task<(bool success, int workshopId)> CreateWorkshop(Workshop_Created workshop)
        {
            try
            {
                return await WorkshopData.CreateWorkshop(workshop);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating workshop.", ex);
            }
        }


        public static async Task<int> UpdateWorkshop(Workshop_Updated workshop)
        {
            try
            {
                return await WorkshopData.UpdateWorkshop(workshop);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating workshop with ID {workshop.WorkshopId}.", ex);
            }
        }

        // ============================
        // DELETE
        // ============================
        public static async Task<int> DeleteWorkshop(int id)
        {
            try
            {
                return await WorkshopData.DeleteWorkshop(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting workshop with ID {id}.", ex);
            }
        }
    }
}