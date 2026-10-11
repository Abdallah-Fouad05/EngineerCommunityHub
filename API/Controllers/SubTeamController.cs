using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/SubTeams")]
    public class SubTeamController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<SubTeamDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllSubTeams()
        {
            try
            {
                var result = await SubTeamBusiness.GetAllSubTeams();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{SubTeamId}")]
        [ProducesResponseType(typeof(SubTeamDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetSubTeamById(int SubTeamId)
        {
            try
            {
                if (SubTeamId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var result = await SubTeamBusiness.GetSubTeamById(SubTeamId);

                if (result == null)
                    return NotFound(new { message = "SubTeam not found." });

                return Ok(result);
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
        public async Task<IActionResult> CreateSubTeam([FromForm] SubTeam_Created_Request request)
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

                var result = await SubTeamBusiness.CreateSubTeam(request);

                if(result.Item2 == -2)
                {
                    return BadRequest(new { message = "SubTeam already exists" });
                }
                if (result.Item1)
                {
                    await BAL.Services.AuditService.LogAsync($"Create", userId, result.Item2, "SubTeam");

                    return CreatedAtAction(
                        nameof(GetSubTeamById),
                        new { SubTeamId = result.Item2 },
                        new { SubTeamId = result.Item2 }
                    );
                }

                return BadRequest(new { message = "Failed to create SubTeam." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

       
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateSubTeam([FromForm] SubTeam_Updated_Request request,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.SubTeamId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await SubTeamBusiness.UpdateSubTeam(request);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Update", userId,request.SubTeamId, "SubTeam");
                    return Ok(new { message = "SubTeam updated successfully." });

                }              

                if (result == -1)
                    return NotFound(new { message = "SubTeam not found." });

                if (result == -2)
                    return BadRequest(new { message = "SubTeam already exists" });

                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        
        [HttpDelete("{SubTeamId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteSubTeam(int SubTeamId,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (SubTeamId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                SubTeamDTO? Ac = await BAL.SubTeamBusiness.GetSubTeamById(SubTeamId);

                if (Ac == null)
                    return NotFound(new { message = "SubTeam not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await SubTeamBusiness.DeleteSubTeam(SubTeamId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Delete", userId, SubTeamId, $"SubTeam | SubTeam Title : {Ac.Title}");

                    return Ok(new { message = "SubTeam deleted successfully." });
                }

                return BadRequest(new { message = "Delete failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{SubTeamId}/Projects")]
        public async Task<IActionResult> GetProjectBySubTeamId(int SubTeamId)
        {
            var data = await ProjectBusiness.GetProjectsBySubTeamId(SubTeamId);
            return Ok(data);
        }

        // SUBTEAM MEMBER
        [AllowAnonymous]
        [HttpGet("{subTeamId}/SubTeamMembers")]
        public async Task<IActionResult> GetBySubTeamId(int subTeamId)
        {
            var data = await SubTeamBusiness.GetSubTeamMembersBySubTeamId(subTeamId);
            return Ok(data);
        }

        [HttpGet("{UserId}/SubTeams")]
        public async Task<IActionResult> GetByUserId(int UserId, [FromServices] IAuthorizationService authorizationService)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int Id))
            {
                return Unauthorized();
            }

            if (UserId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var authResult = await authorizationService.AuthorizeAsync(User, UserId, "SuperAdminOrAdminOrUser");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var data = await SubTeamBusiness.GetSubTeamsByUserId(UserId);
            return Ok(data);
        }


        [HttpPost("SubTeamMembers")]
        public async Task<IActionResult> Add([FromBody] SubTeamMembers_Created request, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var (success, id) = await SubTeamBusiness.Add(request);

            if (success) 
            {
                await BAL.Services.AuditService.LogAsync("Add", userId, request.MemberId, "SubTeam Member");

                return Ok(new { SubTeamMemberId = id });
            }

            if (id == -2)
                return BadRequest(new { message = "Member already exists in SubTeam" });

            return BadRequest(new { message = "Failed to add member" });
            
        }

        [HttpDelete("SubTeamMembers")]
        public async Task<IActionResult> Remove([FromBody] SubTeamMembers_Deleted request, [FromServices] IAuthorizationService authorizationService)
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await SubTeamBusiness.Remove(request);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Remove", userId, request.MemberId, "Project Member");

                    return Ok(new { message = "SubTeam deleted successfully." });
                }
               

                if (result == -1)
                    return NotFound(new { message = "SubTeam not found." });

                return BadRequest(new { message = "Delete failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}