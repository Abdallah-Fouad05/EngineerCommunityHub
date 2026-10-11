using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Projects")]
    public class ProjectsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await ProjectBusiness.GetAllProjects();
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("Status/{StatusId}/Projects")]
        public async Task<IActionResult> GetAll(int StatusId)
        {
            if (StatusId <= 0)
                return BadRequest("Invalid Id");

            var data = await ProjectBusiness.GetProjectsByStatusId(StatusId);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("Status")]
        [ProducesResponseType(typeof(List<PartnerDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProjectStatus()
        {
            try
            {
                var users = await ProjectBusiness.GetProjectStatus();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{ProjectId}")]
        public async Task<IActionResult> GetById(int ProjectId)
        {
            if (ProjectId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var item = await ProjectBusiness.GetProjectById(ProjectId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Project_Created_Request request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            if (!Helper.IsActive(userId)) //Not Active
                return Forbid();

            List<string> images = await BAL.Services.Services.SaveImagesAsync(request.Images, "Projects");


            var model = new Project_Created
            {
                SubTeamId = request.SubTeamId,
                Title = request.Title,
                Description = request.Description,
                StatusId = request.StatusId,
                CompletedAt = request.CompletedAt,
                Images = images.Count > 0 ? string.Join(",", images) : null
               ,CreatedBy = userId
            };

            var result = await ProjectBusiness.CreateProject(model);

            if (result.Item1)
            {
                await BAL.Services.AuditService.LogAsync("Create", userId,result.Item2, "Project");

                return Ok(new { ProjectId = result.Item2 });
            }
               

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] Project_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            if (request.ProjectId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            ProjectDTO? Ac = await BAL.ProjectBusiness.GetProjectById(request.ProjectId);

            if (Ac == null)
                return NotFound(new { message = "Project not found." });

            var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            List<string> images = await BAL.Services.Services.SaveImagesAsync(request.Images, "Projects");

            var model = new Project_Updated
            {
                ProjectId = request.ProjectId,
                SubTeamId = request.SubTeamId,
                Title = request.Title,
                Description = request.Description,
                StatusId = request.StatusId,
                CompletedAt = request.CompletedAt,
                Images = images.Count > 0 ? string.Join(",", images) : null,
            };

            var result = await ProjectBusiness.UpdateProject(model);

            if (result == 1)
            {
                await BAL.Services.AuditService.LogAsync("Update", userId, Ac.ProjectId, "Project");
                return Ok();
            }
               

            return BadRequest();
        }

        [HttpDelete("{ProjectId}")]
        public async Task<IActionResult> Delete(int ProjectId, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            if (ProjectId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            ProjectDTO? Ac = await BAL.ProjectBusiness.GetProjectById(ProjectId);

            if (Ac == null)
                return NotFound(new { message = "Project not found." });

            var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var result = await ProjectBusiness.DeleteProject(ProjectId);

            if (result == 1)
            {
                await BAL.Services.AuditService.LogAsync("Delete", userId, Ac.ProjectId, $"Project | Project Title : {Ac.Title}");
                return Ok();

            }
            

            return BadRequest();
        }

        [HttpGet("{MemberId}/Projects")]
        public async Task<IActionResult> GetProjects(int MemberId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                if (MemberId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var authResult = await authorizationService.AuthorizeAsync(User, MemberId, "SuperAdminOrAdminOrUser");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var data = await ProjectBusiness.GetProjectsByMemberId(MemberId);
                return Ok(data);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //PROJECT MEMBER
        [AllowAnonymous]
        [HttpGet("{ProjectId}/ProjectMembers")]
        public async Task<IActionResult> GetProjectMembers(int ProjectId)
        {
            try
            {
                if (ProjectId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var data = await ProjectBusiness.GetProjectMembers(ProjectId);
                return Ok(data);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpPost("ProjectMembers")]
        public async Task<IActionResult> AddProjectMember([FromBody] ProjectMember_Created request, [FromServices] IAuthorizationService authorizationService)
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

                var result = await ProjectBusiness.AddProjectMember(request);

                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync("Add", userId, request.UserId, "Project Member");

                    return Ok(new { message = "Member added successfully", id = result.result });
                }

                if (result.result == -2)
                    return BadRequest(new { message = "Member already exists" });

                return BadRequest(new { message = "Failed to add member" });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("ProjectMembers")]
        public async Task<IActionResult> RemoveProjectMember(int projectId, int memberId, [FromServices] IAuthorizationService authorizationService)
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

                var result = await ProjectBusiness.RemoveProjectMember(projectId, memberId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync("Remove", userId,memberId,"Project Member");

                    return Ok(new { message = "Member removed successfully" });
                }

                if (result == -1)
                    return NotFound(new { message = "Member not found" });

                return BadRequest(new { message = "Failed to remove member" });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}