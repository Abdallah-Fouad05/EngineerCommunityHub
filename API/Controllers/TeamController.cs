using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Teams")]
    public class TeamController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<TeamDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {
                var teams = await TeamBusiness.GetAllTeams();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{TeamId}/SubTeams")]
        [ProducesResponseType(typeof(List<SubTeamDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllTeamSubTeams(int TeamId)
        {
            try
            {
                var result = await SubTeamBusiness.GetAllTeamSubTeams(TeamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{TeamId}")]
        [ProducesResponseType(typeof(TeamDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetTeamById(int TeamId)
        {
            try
            {
                if (TeamId <= 0)
                    return BadRequest(new { message = "Invalid TeamId" });

                var team = await TeamBusiness.GetTeamById(TeamId);

                if (team == null)
                    return NotFound(new { message = "Team not found." });

                return Ok(team);
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
        public async Task<IActionResult> CreateTeam([FromForm] Team_Created_Request request)
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

                var result = await TeamBusiness.CreateTeam(request);
                if(result.Item2 == -2)
                    return BadRequest(new
                    {
                        message = "Team already exists"
                    });

                if (result.Item1)
                {
                    await BAL.Services.AuditService.LogAsync("Create", userId, result.Item2, "Team");

                    return CreatedAtAction(
                        nameof(GetTeamById),
                        new { TeamId = result.Item2 },
                        new { TeamId = result.Item2 }
                    );
                }

                return BadRequest(new { message = "Failed to create team." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateTeam([FromForm] Team_Updated_Request request,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.TeamId <= 0)
                    return BadRequest(new { message = "Invalid TeamId" });

                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await TeamBusiness.UpdateTeam(request);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Update", userId, request.TeamId, "Team");
                    return Ok(new { message = "Team updated successfully." });
                }

                if (result == -1)
                    return NotFound(new { message = "Team not found." });

                if(result == 2)
                    return BadRequest(new{message = "Team already exists"});

                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

  
        [HttpDelete("{TeamId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteTeam(int TeamId,[FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (TeamId <= 0)
                    return BadRequest(new { message = "Invalid TeamId" });

                TeamDTO? Ac = await BAL.TeamBusiness.GetTeamById(TeamId);

                if (Ac == null)
                    return NotFound(new { message = "Team not found." });


                var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await TeamBusiness.DeleteTeam(TeamId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Delete", userId, TeamId, $"Team | Team Title : {Ac.Title}");

                    return Ok(new { message = "Team deleted successfully." });
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