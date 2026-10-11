using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Events")]
    public class EventsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<EventDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await EventBusiness.GetAllEvents();
                return Ok(events);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("All/{StatusId}")]
        [ProducesResponseType(typeof(List<EventDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllEvents(int StatusId)
        {
            try
            {
                if (StatusId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var events = await EventBusiness.GetAllEvents(StatusId);
                return Ok(events);
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
        public async Task<IActionResult> GetEventStatus()
        {
            try
            {
                var users = await EventBusiness.GetEventStatus();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{EventId}")]
        [ProducesResponseType(typeof(EventDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetEventById(int EventId)
        {
            try
            {
                if (EventId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var ev = await EventBusiness.GetEventById(EventId);

                if (ev == null)
                    return NotFound(new { message = "Event not found." });

                return Ok(ev);
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
        public async Task<IActionResult> CreateEvent([FromForm] Event_Created_Request request)
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

                // Upload Image
                string? filePath = await BAL.Services.Services.SaveImageAsync(request.Image, "Events");

                // Validation Dates
                if (request.StartDate == DateTime.MinValue)
                    return BadRequest(new { message = "StartDate is required." });

                if (request.EndDate.HasValue && request.EndDate <= request.StartDate)
                    return BadRequest(new { message = "EndDate must be after StartDate." });

                var ev = new Event_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    Image = filePath,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Location = request.Location,
                    CreatedBy = userId
                };

                var result = await EventBusiness.CreateEvent(ev);

                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId, result.eventId,"Event");

                    return CreatedAtAction(
                        nameof(GetEventById),
                        new { EventId = result.eventId },
                        new { EventId = result.eventId }
                    );
                }

                return BadRequest(new { message = "Failed to create event." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }   

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateEvent([FromForm] Event_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                EventDTO? En = await BAL.EventBusiness.GetEventById(request.EventId);

                if (En == null)
                    return NotFound(new { message = "Event not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, En.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                // Upload Image
                string? filePath = await BAL.Services.Services.SaveImageAsync(request.Image, "Events");

                // Validation Dates
                if (request.EndDate.HasValue && request.EndDate <= request.StartDate)
                    return BadRequest(new { message = "EndDate must be after StartDate." });

                var model = new Event_Updated
                {
                    EventId = request.EventId,
                    Title = request.Title,
                    Description = request.Description,
                    Image =filePath,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Location = request.Location
                };

                var result = await EventBusiness.UpdateEvent(model);

                if (result == 1) 
                {
                    await BAL.Services.AuditService.LogAsync("Update",userId,En.EventId, "Event");

                    return Ok(new { message = "Event updated successfully." });
                }

                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{EventId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteEvent(int EventId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); // أو BadRequest حسب السيناريو
                }

                if (EventId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                EventDTO? Ac = await BAL.EventBusiness.GetEventById(EventId);

                if (Ac == null)
                    return NotFound(new { message = "Event not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await EventBusiness.DeleteEvent(EventId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Delete",userId, Ac.EventId, $"Event | Event Title : {Ac.Title}");

                    return Ok(new { message = "Event deleted successfully." });

                }

                if (result == -1)
                    return NotFound(new { message = "Event not found." });

                return BadRequest(new { message = "Delete failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}