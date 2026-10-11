using DAL;
using DAL.DTOs;

namespace BAL
{
    public class PartnershipBusiness
    {
        //admin
        public static async Task<List<PartnershipDTO>> GetAllPartnerships()
        {
            try
            {
                return await PartnershipData.GetAllPartnerships();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching partnerships.", ex);
            }
        }

        //user
        public static async Task<List<PartnershipDTO>> GetAllActivePartnerships()
        {
            try
            {
                return await PartnershipData.GetAllActivePartnerships();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching partnerships.", ex);
            }
        }

        public static async Task<List<PartnershipDTO?>> GetAllPartnershipsByUserId(int UserId)
        {
            try
            {
                return await PartnershipData.GetAllPartnershipsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching partnerships", ex);
            }
        }
        public static async Task<PartnershipDTO?> GetPartnershipById(int id)
        {
            try
            {
                return await PartnershipData.GetPartnershipById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching partnership {id}", ex);
            }
        }

        public static async Task<(bool, int)> CreatePartnership(Partnership_Created p)
        {
            try
            {
                return await PartnershipData.CreatePartnership(p);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating partnership", ex);
            }
        }

        public static async Task<int> UpdatePartnership(Partnership_Updated p)
        {
            try
            {
                return await PartnershipData.UpdatePartnership(p);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating partnership", ex);
            }
        }

        public static async Task<int> DeletePartnership(int id)
        {
            try
            {
                return await PartnershipData.DeletePartnership(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting partnership", ex);
            }
        }
    }
}