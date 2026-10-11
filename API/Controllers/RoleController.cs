using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Roles")]
    public class RoleController : ControllerBase
    {
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]

        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (Helper.IsBlocked(userId)) //Not Active
                    return Forbid();

                var roles = await RoleBusiness.GetAllRoles();
                return Ok(roles);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{RoleId}")]
        [ProducesResponseType(typeof(RoleDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetRoleById(int RoleId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (RoleId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(userId)) //Not Active
                    return Forbid();

                var role = await RoleBusiness.GetRoleById(RoleId);

                if (role == null)
                    return NotFound(new { message = "Roles not found." });

                return Ok(role);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("{RoleId}/Users")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsersByRoleId(int RoleId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (RoleId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                if (Helper.IsBlocked(userId)) //Not Active
                    return Forbid();

                var users = await UserBusiness.GetAllUsersByRoleId(RoleId);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //[HttpPost]
        //[ProducesResponseType(201)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> CreateRole([FromBody] Role_Created role)
        //{
        //    try
        //    {
        //        var result = await RoleBusiness.CreateRole(role);

        //        if (result.RoleId == -2)
        //        {
        //            return BadRequest(new
        //            {
        //                message = "Role already exists"
        //            });
        //        }
        //        if (result.success)
        //        {
        //            return CreatedAtAction(
        //                nameof(GetRoleById),
        //                new { id = result.RoleId },
        //                new { roleId = result.RoleId }
        //            );
        //        }

        //        return BadRequest(new { message = "Failed to create role." });
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}


        //[HttpPut]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> UpdateRole([FromBody] RoleDTO role)
        //{
        //    try
        //    {
        //        var result = await RoleBusiness.UpdateRole(role);

        //        if (result == 1)
        //            return Ok(new { message = "Role updated successfully." });

        //        if (result == 0)
        //            return NotFound(new { message = "Role not found." });

        //        if(result == -2)
        //            return NotFound(new { message = "Role Already Exists." });


        //        return BadRequest(new { message = "Update failed." });
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}


        //[HttpDelete("{RoleId}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> DeleteRole(int RoleId)
        //{
        //    try
        //    {
        //        if (RoleId <= 0)
        //            return BadRequest(new { message = "Invalid Id" });
        //        var result = await RoleBusiness.DeleteRole(RoleId);

        //        if (result == 1)
        //            return Ok(new { message = "Role deleted successfully." });

        //        if (result == 0)
        //            return NotFound(new { message = "Role not found." });

        //        return BadRequest(new { message = "Delete failed." });
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}
    }
}