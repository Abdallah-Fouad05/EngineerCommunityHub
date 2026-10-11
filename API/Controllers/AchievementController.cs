using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Achievements")]

    public class AchievementsController : ControllerBase
    {
        /// <summary>
        /// Retrieves all achievements.
        /// </summary>
        ///
        /// <returns>
        /// Returns a list of achievements.
        /// Returns an empty list if no achievements exist.
        /// </returns>
        ///
        /// <response code="200">
        /// Achievements retrieved successfully.
        /// </response>
        ///
        /// <response code="500">
        /// Internal server error.
        /// </response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<AchievementDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllAchievements()
        {
            try
            {
                var data = await AchievementBusiness.GetAllAchievements();
                return Ok(data);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{AchievementId}")]
        [ProducesResponseType(typeof(AchievementDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAchievementById(int AchievementId)
        {
            try
            {
                if (AchievementId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var item = await AchievementBusiness.GetAchievementById(AchievementId);

                if (item == null)
                    return NotFound(new { message = "Achievement not found." });

                return Ok(item);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles ="SuperAdmin,Admin")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> CreateAchievement([FromForm] Achievement_Created_Request request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (!Helper.IsActive(userId))
                {
                    return Forbid();
                }
                    
                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Achievements");

                List<string> videoNames = await BAL.Services.Services.SaveVideosAsync(request.Videos, "Achievements");

                var model = new Achievement_Created
                {
                    SubTeamId = request.SubTeamId,
                    Title = request.Title,
                    Description = request.Description,
                    Link = request.Link,
                    CompetitionName = request.CompetitionName,
                    ProjectId = request.ProjectId,
                    CompletedAt = request.CompletedAt,
                    CreatedBy = userId,
                     
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null,
                    Videos = videoNames.Count > 0 ? string.Join(",", videoNames) : null
                };

                var result = await AchievementBusiness.CreateAchievement(model);

                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId, result.achievementId, "Achievement");

                    return CreatedAtAction(
                        nameof(GetAchievementById),
                        new { AchievementId = result.achievementId },
                        new { AchievementId = result.achievementId }
                    );
                }

                return BadRequest(new { message = "Failed to create achievement." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateAchievement([FromForm] Achievement_Updated_Request request , [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (request.AchievementId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                AchievementDTO? Ac = await BAL.AchievementBusiness.GetAchievementById(request.AchievementId);

                if (Ac == null)
                    return NotFound(new { message = "Achievement not found." });

                var authResult = await authorizationService.AuthorizeAsync(User,Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Achievements");

                List<string> videoNames = await BAL.Services.Services.SaveVideosAsync(request.Videos, "Achievements");

                var model = new Achievement_Updated
                {
                    AchievementId = request.AchievementId,
                    SubTeamId = request.SubTeamId,
                    Title = request.Title,
                    Description = request.Description,
                    Link = request.Link,
                    CompetitionName = request.CompetitionName,
                    ProjectId = request.ProjectId,
                    CompletedAt = request.CompletedAt,
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null,
                    Videos = videoNames.Count > 0 ? string.Join(",", videoNames) : null
                };

                var result = await AchievementBusiness.UpdateAchievement(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update",userId, Ac.AchievementId, "Achievement");

                    return Ok(new { message = "Achievement updated successfully." });
                }

                if (result == -1)
                    return NotFound(new { message = "Achievement not found." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
  
        [HttpDelete("{AchievementId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteAchievement(int AchievementId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (AchievementId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                AchievementDTO? Ac = await BAL.AchievementBusiness.GetAchievementById(AchievementId);

                if (Ac == null)
                    return NotFound(new { message = "Achievement not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await AchievementBusiness.DeleteAchievement(AchievementId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Delete",userId, Ac.AchievementId, $"Achievement | Achievement Title : {Ac.Title}");
                    return Ok(new { message = "Achievement deleted successfully." });
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