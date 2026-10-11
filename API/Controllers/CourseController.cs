using System.Reflection;
using System.Security.Claims;
using BAL;
using BAL.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/Courses")]
    public class CoursesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await CourseBusiness.GetAllCourses();
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("{CourseId}")]
        public async Task<IActionResult> GetById(int CourseId)
        {
            if(CourseId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var item = await CourseBusiness.GetCourseById(CourseId);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Course_Created_Request request)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); 
            }

            if (!Helper.IsActive(userId)) //Not Active
                return Forbid();

            string? fileName = await BAL.Services.Services.SaveImageAsync(request.Image, "Courses");

            var model = new Course_Created
            {
                CourseName = request.CourseName,
                Title = request.Title,
                Description = request.Description,
                Image = fileName,
                Price = request.Price,
                IsPaid = request.IsPaid,
                StartDate = request.StartDate,
                LastDate = request.LastDate,
                Requirements = request.Requirements,
                Skills = request.Skills,
                Link = request.Link,
                CategoryId = request.CategoryId,
                CreatedBy = userId
            };

            var result = await CourseBusiness.CreateCourse(model);

            if (result.Item1)
            {
                await BAL.Services.AuditService.LogAsync("Create", userId,result.Item2, "Course");

                return Ok(new { CourseId = result.Item2 });
            }
                

            if (result.Item2 == -2)
                return BadRequest("Course Name Is Exists");

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] Course_Updated_Request req, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); 
            }

            if (req.CourseId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            CourseDTO? Ac = await BAL.CourseBusiness.GetCourseById(req.CourseId);
            
            if(Ac == null)
                return NotFound(new { message = "Course not found." });

            var authResult = await authorizationService.AuthorizeAsync(User,Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            string? fileName = await BAL.Services.Services.SaveImageAsync(req.Image, "Courses");

            var model = new Course_Updated
            {
                CourseId = req.CourseId,
                CourseName = req.CourseName,
                Title = req.Title,
                Description = req.Description,
                Image = fileName,
                Price = req.Price,
                IsPaid = req.IsPaid,
                StartDate = req.StartDate,
                LastDate = req.LastDate,
                Requirements = req.Requirements,
                Skills = req.Skills,
                Link = req.Link,
                CategoryId = req.CategoryId
            };

            var result = await CourseBusiness.UpdateCourse(model);

            if (result == 1)
            {
                await BAL.Services.AuditService.LogAsync("Update",userId,Ac.CourseId, "Course");
                return Ok();
            }
            if (result == -2) return BadRequest("Course Name Is Exists"); ;

            return BadRequest();
        }

        [HttpDelete("{CourseId}")]
        public async Task<IActionResult> Delete(int CourseId, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); 
            }

            if (CourseId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            CourseDTO? Ac = await BAL.CourseBusiness.GetCourseById(CourseId);

            if (Ac == null)
                return NotFound(new { message = "Course not found." });

            var authResult = await authorizationService.AuthorizeAsync(User,Ac.CreatedBy.UserId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var result = await CourseBusiness.DeleteCourse(CourseId);

            if (result == 1)
            {
                await BAL.Services.AuditService.LogAsync("Delete",userId, Ac.CourseId, $"Course | Course Name : {Ac.CourseName}");
                return Ok();
            }
            if (result == -1) return NotFound();

            return BadRequest();
        }

        //Instructor

        [AllowAnonymous]
        [HttpGet("Instructors")]
        public async Task<IActionResult> GetInstructors()
        {
            var data = await InstructorCourseBusiness.GetAllInstructors();

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("{CourseId}/Instructors")]
        public async Task<IActionResult> GetAllInstructorsCourse(int CourseId)
        {
            if (CourseId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var data = await InstructorCourseBusiness.GetInstructorsByCourseId(CourseId);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        //instructor courses

        [AllowAnonymous]
        [HttpGet("{InstructorId}/InstructorCourses")]
        public async Task<IActionResult> GetCourses(int InstructorId)
        {
            if (InstructorId <= 0)
                return BadRequest(new { message = "Invalid Id" });

            var data = await InstructorCourseBusiness.GetCoursesByInstructorId(InstructorId);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost("InstructorCourses")]
        public async Task<IActionResult> AddInstructorToCourse(InstructorCourse_Created request, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, userId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var result = await InstructorCourseBusiness.Create(request);

            if (result.Item1)
            {
                await BAL.Services.AuditService.LogAsync("Add",userId,request.InstructorId,"Instructor Course");

                return Ok(new { id = result.Item2 });
            }

            if (result.Item2 == -2)
                return BadRequest(new { message = "Instructor Already Exists" });

            return BadRequest(new { message = "Failed" });
        }

        [HttpDelete("InstructorCourses")]
        public async Task<IActionResult> RemoveInstructor(InstructorCourse_Created request, [FromServices] IAuthorizationService authorizationService)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); 
            }

            var authResult = await authorizationService.AuthorizeAsync(User,userId, "SuperAdminOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            var result = await InstructorCourseBusiness.Delete(request);

            if (result == 1)
            {
                await BAL.Services.AuditService.LogAsync("Remove",userId, request.InstructorId,"Instructor Course");
                return Ok("Removed");
            }
            return NotFound();
        }
    }
}