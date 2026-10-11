using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Partnerships")]
    public class PartnershipsController : ControllerBase
    {

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
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

                return Ok(await PartnershipBusiness.GetAllPartnerships());
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Active")]
        public async Task<IActionResult> GetAllActive()
        {
            try
            {
                return Ok(await PartnershipBusiness.GetAllActivePartnerships());
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{PartnershipId}")]
        public async Task<IActionResult> GetById(int PartnershipId)
        {
            try
            {
                if (PartnershipId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var item = await PartnershipBusiness.GetPartnershipById(PartnershipId);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Partnership_Created_Request request)
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

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Partnerships");

                var model = new Partnership_Created
                {
                    PartnerId = request.PartnerId,
                    Title = request.Title,
                    Description = request.Description,
                    Image = fileName,
                    CreatedBy = userId
                };

                var result = await PartnershipBusiness.CreatePartnership(model);

                if (result.Item1)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId,result.Item2,"Partnership");

                    return CreatedAtAction(nameof(GetById), new { PartnershipId = result.Item2 }, result.Item2);
                }

                if (result.Item2 == -2)
                {
                    return BadRequest("PartnerShip Title Is Exists");
                }

                return BadRequest();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] Partnership_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.PartnerId <= 0 || request.PartnershipId <=0)
                    return BadRequest(new { message = "Invalid Id" });

                PartnershipDTO? PS = await BAL.PartnershipBusiness.GetPartnershipById(request.PartnershipId);

                if (PS== null)
                    return NotFound(new { message = "PartnerShip  not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, PS.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Partnerships");

                var model = new Partnership_Updated
                {
                    PartnershipId = request.PartnershipId,
                    PartnerId = request.PartnerId,
                    Title = request.Title,
                    Description = request.Description,
                    Image = fileName,
                    StatusId = request.StatusId
                };

                var result = await PartnershipBusiness.UpdatePartnership(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update",userId,PS.PartnershipId, "Partnership");
                    return Ok();
                }

                if (result == -2)
                    return BadRequest("PartnerShip Title Is Exists");

                return BadRequest();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{PartnershipId}")]
        public async Task<IActionResult> Delete(int PartnershipId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (PartnershipId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                PartnershipDTO? Ac = await BAL.PartnershipBusiness.GetPartnershipById(PartnershipId);

                if (Ac == null)
                    return NotFound(new { message = "PartnerShip  not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await PartnershipBusiness.DeletePartnership(PartnershipId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Delete",userId,Ac.PartnershipId,$"Partnership | PartnerShip Title : {Ac.Title}");
                    return Ok();
                }

                return BadRequest();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}