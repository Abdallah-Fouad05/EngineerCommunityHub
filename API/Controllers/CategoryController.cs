using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Categories")]
    public class CategoriesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await CategoryBusiness.GetAllCategories();
                return Ok(categories);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{CategoryId}/Courses")]
        public async Task<IActionResult> GetAllCoursesByCategory(int CategoryId)
        {
            if (CategoryId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var data = await CourseBusiness.GetCoursesByCategoryId(CategoryId);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("{CategoryId}")]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetCategoryById(int CategoryId)
        {
            try
            {
                if (CategoryId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var category = await CategoryBusiness.GetCategoryById(CategoryId);

                if (category == null)
                    return NotFound(new { message = "Category not found." });

                return Ok(category);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> CreateCategory([FromForm] Category_Created_Request request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (!Helper.IsActive(userId)) //Not Active
                    return Forbid();

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Categories");

                var model = new Category_Created(
                    categoryName: request.CategoryName,
                    description: request.Description,
                    image: fileName
                );
                var result = await CategoryBusiness.CreateCategory(model);
                
                if(result.categoryId == -2)
                {
                    return BadRequest(new
                    {
                        message = "Category already exists"
                    });
                }
                
                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId, result.categoryId, "Category");

                    return CreatedAtAction(
                        nameof(GetCategoryById),
                        new { CategoryId = result.categoryId },
                        new { CategoryId = result.categoryId }
                    );
                }
              
                    return BadRequest(new { message = "Failed to create category." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateCategory([FromForm] Category_Updated_Request request,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (request.CategoryId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var authResult = await authorizationService.AuthorizeAsync(User,userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Categories");

                var model = new CategoryDTO(
                    categoryId:request.CategoryId,
                    categoryName: request.CategoryName,
                    description: request.Description,
                    image: fileName
                );

                var result = await CategoryBusiness.UpdateCategory(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update", userId,request.CategoryId, "Category");

                    return Ok(new { message = "Category updated successfully." });
                }
                if (result == -1)
                    return NotFound(new { message = "Category not found." });

                if (result == -2)
                    return BadRequest(new { message = "Category already exists." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{CategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteCategory(int CategoryId,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (CategoryId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                CategoryDTO? Ac = await BAL.CategoryBusiness.GetCategoryById(CategoryId);

                if(Ac == null)
                    return NotFound(new { message = "Category not found."});

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await CategoryBusiness.DeleteCategory(CategoryId);

                if (result == 1) {

                    await BAL.Services.AuditService.LogAsync("Delete",userId,CategoryId, $"Category | Category Name : {Ac.CategoryName} ");

                    return Ok(new { message = "Category deleted successfully." });
                }

                return BadRequest(new { message = "Delete failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}