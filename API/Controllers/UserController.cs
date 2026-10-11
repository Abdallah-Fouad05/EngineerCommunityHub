using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Services;
using System.Reflection;
namespace API.Controllers
{

    [ApiController]
    [Route("api/v1/Users")]
    public class UserController : ControllerBase
    {
      

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (Helper.IsBlocked(userId)) //bloked
                    return Forbid();

                var users = await UserBusiness.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("CommunityMembers")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCommunityMembers()
        {
            try
            {
                var users = await UserBusiness.GetAllCommunityMembers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("Status")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserStatus()
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

        [HttpGet("{UserId}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int UserId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                UserDTO? Ac = await BAL.UserBusiness.GetUserById(UserId);

                if (Ac == null)
                    return NotFound(new { message = "Workshop not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.UserId, "SuperAdminOrAdminOrUser");

                if (!authResult.Succeeded)
                    return Forbid(); // 403


                var user = await UserBusiness.GetUserById(UserId);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        ///
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] User_Update_Request user)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                    return Unauthorized();

                if (id != user.UserId)
                    return Forbid();

                string? fileName = await BAL.Services.Services.SaveImageAsync(user.Image, "Users");

                var updatedUser = new User_Update
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    BirthDate = user.BirthDate,
                    Image = fileName,
                    Bio = user.Bio,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };

                var result = await UserBusiness.UpdateUser(updatedUser);

                if (result == 1)
                    return Ok(new { message = "User updated successfully." });

                if (result == -1)
                    return NotFound(new { message = "User not found." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("ChangePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        ///
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] User_ChangePassword user)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                    return Unauthorized();

                if (id != user.UserId)
                    return Forbid();

                if (!Helper.IsPasswordValid(user.NewPassword))
                    return BadRequest("Invalid password");

                var result = await UserBusiness.ChangePassword(user);

                if (result == 1)
                    return Ok(new { message = "User password updated successfully." });

                if (result == -1)
                    return NotFound(new { message = "User not found." });

                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }




        [HttpDelete("{UserId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int Id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                var result = await UserBusiness.DeleteUser(UserId);

                if (result == 1)
                    return Ok(new { message = "User deleted successfully." });

                if (result == -1)
                {
                    await BAL.Services.AuditService.LogAsync($"Delete", Id, UserId, "User");
                    return NotFound(new { message = "User not found."});
                }

                return BadRequest(new { message = "Delete failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(int userId, int RoleId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int Id))
            {
                return Unauthorized();
            }

            if (!Helper.IsActive(Id)) //Not Active
                return Forbid();

            var result =  await UserBusiness.UpdateUserRole(userId, RoleId,Id);

            switch (result)
            {
                case -1:
                    return NotFound(new { message = "Invalid data" });

                case 1:
                    await BAL.Services.AuditService.LogAsync(
                        "Update User Role",
                        Id,
                        userId,
                        "User Role");

                    return Ok(new { message = "Role updated successfully" });

                case -3:
                case -4:
                    return Forbid();

                default:
                    return BadRequest(new { message = "Failed Update" });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("UpdateUserStatus")]
        public async Task<IActionResult> UpdateUserStatus(int userId, int StatusId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int Id))
            {
                return Unauthorized();
            }

            if (!Helper.IsActive(Id)) //Not Active
                return Forbid();

            var result = await UserBusiness.UpdateUserStatus(userId, StatusId, Id);

            switch (result)
            {
                case -1:
                    return NotFound(new { message = "Invalid data" });

                case 1:
                    await BAL.Services.AuditService.LogAsync(
                        "Update User Status",
                        Id,
                        userId,
                        "User Status");
                    return Ok(new { message = "Status updated successfully" });

                case -3:
                case -4:
                    return Forbid();

                default:
                    return BadRequest(new { message = "Failed Update" });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("UpdateUserCommunityRole")]
        public async Task<IActionResult> UpdateUserCommunityRole(int userId, int RoleId)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int Id))
            {
                return Unauthorized();
            }


            if (!Helper.IsActive(Id)) //Not Active
                return Forbid();

            var result = await UserBusiness.UpdateUserCommunityRole(userId, RoleId, Id);

            switch (result)
            {
                case -1:
                    return NotFound(new { message = "Invalid data" });

                case 1:
                    await BAL.Services.AuditService.LogAsync(
                        "Update User community Role",
                        Id,
                        userId,
                        "User Community Role");
                    return Ok(new { message = "Role updated successfully" });

                case -3:
                case -4:
                    return Forbid();

                default:
                    return BadRequest(new { message = "Failed Update" });
            }
        }


        //--------DISPLAY Records Created by USER(ADMIN)---------

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Posts")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllPosts(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(id) || id !=  UserId) //Blocked
                    return Forbid();

                var posts = await PostBusiness.GetAllPostsByUserId(UserId);
                return Ok(posts);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Workshops")]
        [ProducesResponseType(typeof(List<WorkshopDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllWorkshops(int UserId)
        {
            try 
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();

                var workshops = await WorkshopBusiness.GetAllWorkshopsByUserId(UserId);
                return Ok(workshops);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Projects")]
        [ProducesResponseType(typeof(List<ProjectDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllProjects(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();

                var Projects = await ProjectBusiness.GetAllProjectsByUserId(UserId);
                return Ok(Projects);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Events")]
        [ProducesResponseType(typeof(List<EventDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllEvents(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();

                var events = await EventBusiness.GetAllEventsByUserId(UserId);
                return Ok(events);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Partnerships")]
        [ProducesResponseType(typeof(List<PartnershipDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllPartnerships(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();


                var Partnerships = await PartnershipBusiness.GetAllPartnershipsByUserId(UserId);
                return Ok(Partnerships);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Achievements")]
        [ProducesResponseType(typeof(List<AchievementDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllAchievementsByUserId(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });


                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();


                var Partnerships = await AchievementBusiness.GetAllAchievementsByUserId(UserId);
                return Ok(Partnerships);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{UserId}/Courses")]
        [ProducesResponseType(typeof(List<CourseDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllCoursesByUserId(int UserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int id))
                {
                    return Unauthorized();
                }

                if (UserId <= 0)
                    return BadRequest(new { message = "Invalid Id" });


                if (Helper.IsBlocked(id) || id != UserId) //Blocked
                    return Forbid();

                var data = await CourseBusiness.GetAllCoursesByUserId(UserId);
                return Ok(data);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //-- Audit
        [HttpGet("AuditLogs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuditLogs(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(new { message = "Invalid pagination values" });

            var logs = await BAL.UserBusiness.GetAuditLogs(page, pageSize);

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                Data = logs
            });
        }
    }
}