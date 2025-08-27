using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
using System.Security.Claims;

// EK: aşağıdaki ikisi List aksiyonundaki kayıt kontrolü için gerekli
using Microsoft.EntityFrameworkCore;
using OnlineEduSystem.Infrastructure.Data;

namespace OnlineEduSystem.Api.Controllers
{
    [ApiController]
    [Route("api/courses/{courseId:int}/materials")]
    [Produces("application/json")]
    public class CourseMaterialsController : ControllerBase
    {
        private readonly ICourseMaterialService _service;

        public CourseMaterialsController(ICourseMaterialService service)
        {
            _service = service;
        }

        // LIST (belirli kurs)  -> GÜNCELLENDİ
        [Authorize] // materyal görmek için giriş şart
        [HttpGet]
        public async Task<ActionResult<List<CourseMaterialDto>>> List(
            int courseId,
            [FromServices] AppDbContext ctx) // sadece bu ek servis
        {
            // Eğitmen/Admin ise serbest
            if (User.IsInRole("Instructor") || User.IsInRole("Admin"))
                return Ok(await _service.GetByCourseIdAsync(courseId));

            // Öğrenci kayıtlı mı?
            var userId   = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrolled = await ctx.Enrollments
                                    .AsNoTracking()
                                    .AnyAsync(e => e.CourseId == courseId && e.UserId == userId);
            if (!enrolled) return Forbid(); // 403

            return Ok(await _service.GetByCourseIdAsync(courseId));
        }

        // GET tek kayıt
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseMaterialDto>> Get(int courseId, int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        // CREATE (JSON)
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<CourseMaterialDto>> Create(int courseId, CreateCourseMaterialDto dto)
        {
            dto.CourseId = courseId; // route’dan al
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { courseId, id = created.Id }, created);
        }

        // UPDATE (JSON)
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        public async Task<ActionResult<CourseMaterialDto>> Update(int courseId, int id, UpdateCourseMaterialDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE
        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int courseId, int id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        // -------- UPLOAD (multipart/form-data) --------
        public class UploadMaterialForm
        {
            public string Title { get; set; } = null!;
            public MaterialType Type { get; set; }  // Pdf | Video | Document
            public IFormFile File { get; set; } = null!;
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CourseMaterialDto>> Upload(
            int courseId,
            [FromForm] UploadMaterialForm form,
            [FromServices] OnlineEduSystem.Infrastructure.Services.FileService fileSvc,
            [FromServices] ICourseRepository courseRepo)
        {
            var course = await courseRepo.GetByIdWithDetailsAsync(courseId);
            if (course == null) return NotFound("Kurs bulunamadı.");

            var meId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin" && course.InstructorId != meId) return Forbid();

            if (form.File == null || form.File.Length == 0) return BadRequest("Dosya gönderilmedi.");

            var path = await fileSvc.SaveFileAsync(form.File); // "/uploads/xxx.ext"

            var created = await _service.CreateAsync(new CreateCourseMaterialDto
            {
                CourseId = courseId,
                Title = form.Title,
                FilePath = path,
                Type = form.Type
            });

            return CreatedAtAction(nameof(Get), new { courseId, id = created.Id }, created);
        }
    }
}
