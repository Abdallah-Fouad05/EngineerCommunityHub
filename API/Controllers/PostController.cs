using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Posts")]
    public class PostsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await PostBusiness.GetAllPosts();
                return Ok(posts);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{PostId}")]
        [ProducesResponseType(typeof(PostDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetPostById(int PostId)
        {
            try
            {
                if (PostId <= 0)
                    return BadRequest(new { message = "Invalid PostId" });

                var post = await PostBusiness.GetPostById(PostId);

                if (post == null)
                    return NotFound(new { message = "Post not found." });

                return Ok(post);
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
        public async Task<IActionResult> CreatePost([FromForm] Post_Created_Request request)
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

                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Posts");
                List<string> videoNames = await BAL.Services.Services.SaveVideosAsync(request.Videos, "Posts");

                var model = new Post_Created
                {
                    Title = request.Title,
                    Description = request.Description,
                    Link = request.Link,
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null,
                    Videos = videoNames.Count > 0 ? string.Join(",", videoNames) : null,
                    CreatedBy = userId
                };

                var result = await PostBusiness.CreatePost(model);

                if (result.success)
                {
                    await BAL.Services.AuditService.LogAsync($"Create", userId,result.postId, "Post");
                    return CreatedAtAction(
                        nameof(GetPostById),
                        new { PostId = result.postId },
                        new { PostId = result.postId }
                    );
                }

                return BadRequest(new { message = "Failed to create post." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdatePost([FromForm] Post_Updated_Request request, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (request.PostId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                PostDTO? Ac = await BAL.PostBusiness.GetPostById(request.PostId);

                if (Ac == null)
                    return NotFound(new { message = "Post not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                List<string> imageNames = await BAL.Services.Services.SaveImagesAsync(request.Images, "Posts");
                List<string> videoNames = await BAL.Services.Services.SaveVideosAsync(request.Videos, "Posts");


                var model = new Post_Updated
                {
                    PostId = request.PostId,
                    Title = request.Title,
                    Description = request.Description,
                    Link = request.Link,
                    Images = imageNames.Count > 0 ? string.Join(",", imageNames) : null,
                    Videos = videoNames.Count > 0 ? string.Join(",", videoNames) : null
                };

                var result = await PostBusiness.UpdatePost(model);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Update", userId, Ac.PostId , "Post");

                    return Ok(new { message = "Post updated successfully." });
                }
                return BadRequest(new { message = "Update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("{PostId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeletePost(int PostId, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (PostId <= 0)
                    return BadRequest(new { message = "Invalid Id" });

                PostDTO? P = await BAL.PostBusiness.GetPostById(PostId);

                if (P == null)
                    return NotFound(new { message = "Post not found." });

                var authResult = await authorizationService.AuthorizeAsync(User, P.CreatedBy.UserId, "SuperAdminOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // 403

                var result = await PostBusiness.DeletePost(PostId);

                if (result == 1)
                {
                    await BAL.Services.AuditService.LogAsync($"Delete", userId,P.PostId, $"Post | Post Title : {P.Title}");

                    return Ok(new { message = "Post deleted successfully." });
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