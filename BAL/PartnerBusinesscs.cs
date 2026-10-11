using DAL;
using DAL.DTOs;

namespace BAL
{
    public class PartnerBusiness
    {
        public static async Task<List<PartnerDTO>> GetAllPartners()
        {
            try
            {
                return await PartnerData.GetAllPartners();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all partners.", ex);
            }
        }

        public static async Task<List<PartnerDTO>> GetAllActivePartners()
        {
            try
            {
                return await PartnerData.GetAllActivePartners();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all partners.", ex);
            
            }
        }
        public static async Task<PartnerDTO?> GetPartnerById(int id)
        {
            try
            {
                return await PartnerData.GetPartnerById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching partner with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int partnerId)> CreatePartner(Partner_Created partner)
        {
            try
            {
                return await PartnerData.CreatePartner(partner);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating partner.", ex);
            }
        }

        public static async Task<int> UpdatePartner(Partner_Updated partner)
        {
            try
            {
                return await PartnerData.UpdatePartner(partner);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating partner with ID {partner.PartnerId}.", ex);
            }
        }

        public static async Task<int> DeletePartner(int partnerId)
        {
            try
            {
                return await PartnerData.DeletePartner(partnerId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting partner with ID {partnerId}.", ex);
            }
        }
    }
}