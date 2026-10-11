using DAL;
using DAL.DTOs;

namespace BAL
{
    public class CategoryBusiness
    {
        public static async Task<List<CategoryDTO>> GetAllCategories()
        {
            try
            {
                return await CategoryData.GetAllCategories();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all categories.", ex);
            }
        }

        public static async Task<CategoryDTO?> GetCategoryById(int id)
        {
            try
            {
                return await CategoryData.GetCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching category with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int categoryId)> CreateCategory(Category_Created category)
        {
            try
            {
                return await CategoryData.CreateCategory(category);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating category.", ex);
            }
        }

        public static async Task<int> UpdateCategory(CategoryDTO category)
        {
            try
            {
                return await CategoryData.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating category with ID {category.CategoryId}.", ex);
            }
        }

        public static async Task<int> DeleteCategory(int categoryId)
        {
            try
            {
                return await CategoryData.DeleteCategory(categoryId);
               
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting category with ID {categoryId}.", ex);
            }
        }
    }
}