using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Branches")]
    public class BranchController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<BranchDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branches = await BranchBusiness.GetAllBranches();
                return Ok(branches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{BranchId}/Teams")]
        [ProducesResponseType(typeof(List<TeamDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllBranchTeams(int BranchId)
        {
            try
            {
                if (BranchId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var teams = await TeamBusiness.GetAllBranchTeams(BranchId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{BranchId}")]
        [ProducesResponseType(typeof(BranchDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetBranchById(int BranchId)
        {
            try
            {
                if (BranchId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var branch = await BranchBusiness.GetBranchById(BranchId);

                if (branch == null)
                    return NotFound(new { message = "Branch not found." });

                return Ok(branch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> CreateBranch([FromForm] Branch_Created_Request request)
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

                var result = await BranchBusiness.CreateBranch(request);

                if (result.Item2 == -2)
                {
                    return BadRequest(new
                    {
                        message = "Branch already exists"
                    });
                }
                if (result.Item1)
                {
                    await BAL.Services.AuditService.LogAsync("Create",userId,result.Item2, "Branch");
                    return CreatedAtAction(
                        nameof(GetBranchById),
                        new { BranchId = result.Item2 },
                        new { BranchId = result.Item2 }
                    );
                }

                return BadRequest(new { message = "Failed to create branch." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateBranch([FromForm] Branch_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.BranchId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var authResult = await authorizationService.AuthorizeAsync(User,userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await BranchBusiness.UpdateBranch(request);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update",userId,request.BranchId, "Branch");

                    return Ok(new { message = "Branch updated successfully." });
                }
                  

                if (result == -1)
                    return NotFound(new { message = "Branch not found." });
                if (result == -2)
                    return BadRequest(new{message = "Branch already exists"});


                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{BranchId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteBranch(int BranchId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(); 
                }

                if (BranchId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                BranchDTO? Ac = await BAL.BranchBusiness.GetBranchById(BranchId);

                if (Ac == null)
                    return NotFound(new { message = "Branch not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid();

                var result = await BranchBusiness.DeleteBranch(BranchId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Delete", userId, BranchId, $"Branch | Branch Title : {Ac.Title}");
                    return Ok(new { message = "Branch deleted successfully." });
                }

                return BadRequest(new { message = "Delete failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}