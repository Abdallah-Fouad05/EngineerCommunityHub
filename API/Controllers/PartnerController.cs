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
    [Route("api/v1/Partners")]
    public class PartnersController : ControllerBase
    {
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<PartnerDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (Helper.IsBlocked(userId)) //Blocked
                    return Forbid();

                var partners = await PartnerBusiness.GetAllPartners();
                return Ok(partners);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Status")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartnerStatus()
        {
            try
            {
                var users = await UserBusiness.GetUserStatus();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Active")]
        [ProducesResponseType(typeof(List<PartnerDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllActive()
        {  
            try
            {
                var partners = await PartnerBusiness.GetAllActivePartners();
                return Ok(partners);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{PartnerId}")]
        [ProducesResponseType(typeof(PartnerDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetPartnerById(int PartnerId)
        {
            try
            {
                if (PartnerId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var partner = await PartnerBusiness.GetPartnerById(PartnerId);

                if (partner == null)
                    return NotFound(new { message = "Partner not found." });

                return Ok(partner);
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
        public async Task<IActionResult> CreatePartner([FromForm] Partner_Created_Request request)
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

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Partners");

                var model = new Partner_Created
                {
                    PartnerName = request.PartnerName,
                    Description = request.Description,
                    Link = request.Link,
                    Image = fileName
                };

                var result = await PartnerBusiness.CreatePartner(model);
                if (result.partnerId == -2)
                    return BadRequest(new { Message = "Partner already exists" });
                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId, result.partnerId, "Partner");

                    return CreatedAtAction(
                        nameof(GetPartnerById),
                        new { PartnerId = result.partnerId },
                        new { PartnerId = result.partnerId }
                    );
                }

                return BadRequest(new { message = "Failed to create partner." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
     
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdatePartner([FromForm] Partner_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            { 
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.PartnerId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Partners");

                var model = new Partner_Updated
                {
                    PartnerId = request.PartnerId,
                    PartnerName = request.PartnerName,
                    Description = request.Description,
                    Link = request.Link,
                    Image = fileName,
                    StatusId = request.StatusId
                };

                var result = await PartnerBusiness.UpdatePartner(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update", userId, request.PartnerId, "Partner");

                    return Ok(new { message = "Partner updated successfully." });
                }
                if (result == -1)
                    return NotFound(new { message = "Partner not found." });

                if (result == -2)
                    return BadRequest(new { message = "Partner already exists." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{PartnerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeletePartner(int PartnerId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (PartnerId <= 0)
                    return BadRequest(new { message = "Invalid Id"});

                PartnerDTO? Ac = await BAL.PartnerBusiness.GetPartnerById(PartnerId);

                if (Ac == null)
                    return NotFound(new { message = "Partner not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await PartnerBusiness.DeletePartner(PartnerId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Delete", userId,PartnerId, $"Partner | Partner Name : {Ac.PartnerName}");
                    return Ok(new { message = "Partner deleted successfully." });
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