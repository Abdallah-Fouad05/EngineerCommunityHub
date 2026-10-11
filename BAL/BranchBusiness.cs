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
    public static class BranchBusiness
    {
        public static async Task<List<BranchDTO>> GetAllBranches()
        {
            try
            {
                return await BranchData.GetAllBranches();
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetAllBranches", ex);
            }
        }

        public static async Task<BranchDTO?> GetBranchById(int id)
        {
            try
            {
                return await BranchData.GetBranchById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error GetBranchById", ex);
            }
        }
        public static async Task<(bool, int)> CreateBranch(Branch_Created_Request request)
        {
            try
            {
              
                List<string?> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "Branches");
                

                // 3️⃣ Map to DAL model
                var branch = new Branch_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    LeaderId = request.LeaderId,
                    Images = savedImages.Count > 0 ?  string.Join(",", savedImages) : null
                };

                // 4️⃣ Call DAL
                return await BranchData.CreateBranch(branch);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error CreateBranch", ex);
            }
        }

        // ============================
        // Update Branch
        // ============================
        public static async Task<int> UpdateBranch(Branch_Updated_Request request)
        {
            try
            {
                List<string?> savedImages = await BAL.Services.Services.SaveImagesAsync(request.Images, "Branches");


                // 2️⃣ Map to DAL model
                var branch = new Branch_Updated
                {
                    BranchId = request.BranchId,
                    Title = request.Title,
                    Description = request.Description,
                    LeaderId = request.LeaderId,
                    Images = savedImages.Count > 0 ? string.Join(",", savedImages) : null
                };

                return await BranchData.UpdateBranch(branch);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error UpdateBranch", ex);
            }
        }
        public static async Task<int> DeleteBranch(int id)
        {
            try
            {
                return await BranchData.DeleteBranch(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL Error DeleteBranch", ex);
            }
        }
    }
}