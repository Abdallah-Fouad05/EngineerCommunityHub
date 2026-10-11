using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Workshops")]
    public class WorkshopsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<WorkshopDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllWorkshops()
        {
            try
            {
                var workshops = await WorkshopBusiness.GetAllWorkshops();
                return Ok(workshops);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("All/{StatusId}")]
        [ProducesResponseType(typeof(WorkshopDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetWorkshopsByStatusId(int StatusId)
        {
            try
            {
                if (StatusId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var workshop = await WorkshopBusiness.GetAllWorkshopsByStatusId(StatusId);

                if (workshop == null)
                    return NotFound(new { message = "Workshop not found." });

                return Ok(workshop);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Status")]
        [ProducesResponseType(typeof(List<StatusDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetWorkShopStatus()
        {
            try
            {
                var status = await EventBusiness.GetEventStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{WorkshopId}")]
        [ProducesResponseType(typeof(WorkshopDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetWorkshopById(int WorkshopId)
        {
            try
            {
                if (WorkshopId <= 0)
                    return BadRequest(new { message = "Invalid Workshop Id" });

                var workshop = await WorkshopBusiness.GetWorkshopById(WorkshopId);

                if (workshop == null)
                    return NotFound(new { message = "Workshop not found." });

                return Ok(workshop);
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
        public async Task<IActionResult> CreateWorkshop([FromForm] Workshop_Created_Request request)
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


                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Workshops");

                
                if (request.EndTime.HasValue && request.EndTime <= request.StartTime)
                    return BadRequest(new { message = "EndDate must be after StartDate." });

                var model = new Workshop_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    StartTime = request.StartTime,
                    Topics = request.Topics,
                    WorkshopDate = request.WorkshopDate,
                    EndTime = request.EndTime,
                    Location = request.Location,
                    CreatedBy = userId,
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null
                    ,Link = request.Link
                };

                var result = await WorkshopBusiness.CreateWorkshop(model);

                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync($"Create", userId, result.workshopId, "Workshop");

                    return CreatedAtAction(
                        nameof(GetWorkshopById),
                        new { WorkshopId = result.workshopId },
                        new { WorkshopId = result.workshopId }
                    );
                }

                return BadRequest(new { message = "Failed to create workshop." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateWorkshop([FromForm] Workshop_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.WorkshopId <= 0)
                    return BadRequest(new { message = "Invalid Workshop Id" });

                WorkshopDTO? Ac = await BAL.WorkshopBusiness.GetWorkshopById(request.WorkshopId);

                if (Ac == null)
                    return NotFound(new { message = "Workshop not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Workshops");

                
                if (request.EndTime.HasValue && request.EndTime < request.StartTime)
                    return BadRequest(new { message = "EndDate must be after StartDate." });

                var model = new Workshop_Updated
                {
                    WorkshopId = request.WorkshopId,
                    Title = request.Title,
                    Description = request.Description,
                    Topics = request.Topics,
                    WorkshopDate = request.WorkshopDate,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Location = request.Location,
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null,
                    Link = request.Link,
                };

                var result = await WorkshopBusiness.UpdateWorkshop(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Update", userId,Ac.WorkshopId, "Workshop");

                    return Ok(new { message = "Workshop updated successfully." });
                }
                if (result == -1)
                    return NotFound(new { message = "Workshop not found." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{WorkshopId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteWorkshop(int WorkshopId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (WorkshopId <= 0)
                    return BadRequest(new {message = "Invalid Workshop Id" });

                WorkshopDTO? Ac = await BAL.WorkshopBusiness.GetWorkshopById(WorkshopId);

                if (Ac == null)
                    return NotFound(new { message = "Workshop not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await WorkshopBusiness.DeleteWorkshop(WorkshopId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Delete", userId, Ac.WorkshopId, $"Workshop | Workshop Title : {Ac.Title}");

                    return Ok(new { message = "Workshop deleted successfully." });
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