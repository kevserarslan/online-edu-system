// OnlineEduSystem.Api/Controllers/EnrollmentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineEduSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _svc;
        public EnrollmentsController(IEnrollmentService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<EnrollmentDto>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDto>> Get(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentDto>> Create(CreateEnrollmentDto dto)
        {
            try
            {
                var created = await _svc.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            // Unique ihlali: ya index adından ya da SQL hata kodundan yakala
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                return Conflict(new { message = "Bu kursa zaten kayıtlısınız." });
            }
        }

        // Yardımcı: unique ihlali mi?
        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sql &&
                (sql.Number == 2601 || sql.Number == 2627))
                return true;

            // EF'in oluşturduğu index adı farklıysa burayı kendi index adına göre güncelle
            return ex.InnerException?.Message?.Contains("IX_Enrollments_CourseId_UserId") == true;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<EnrollmentDto>> Update(int id, UpdateEnrollmentDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => (await _svc.DeleteAsync(id)) ? NoContent() : NotFound();


    }
}
