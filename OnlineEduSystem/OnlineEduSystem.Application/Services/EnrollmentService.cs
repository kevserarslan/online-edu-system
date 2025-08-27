using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        public EnrollmentService(IEnrollmentRepository repo) => _repo = repo;

        public async Task<List<EnrollmentDto>> GetAllAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseTitle = e.Course?.Title ?? string.Empty,
                UserName = e.User?.UserName ?? string.Empty,
                EnrolledAt = e.EnrolledAt
            }).ToList();
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdWithDetailsAsync(id);
            if (e == null) return null;
            return new EnrollmentDto
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseTitle = e.Course?.Title ?? string.Empty,
                UserName = e.User?.UserName ?? string.Empty,
                EnrolledAt = e.EnrolledAt
            };
        }

        public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
        {
            var entity = new Enrollment
            {
                CourseId = dto.CourseId,
                UserId = dto.UserId,
                EnrolledAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            var created = await _repo.GetByIdWithDetailsAsync(entity.Id);
            return new EnrollmentDto
            {
                Id = created!.Id,
                CourseId = created.CourseId,
                CourseTitle = created.Course?.Title ?? string.Empty,
                UserName = created.User?.UserName ?? string.Empty,
                EnrolledAt = created.EnrolledAt
            };
        }

        public async Task<EnrollmentDto?> UpdateAsync(int id, UpdateEnrollmentDto dto)
        {
            var e = await _repo.GetByIdWithDetailsAsync(id);
            if (e == null) return null;

            if (dto.CourseId.HasValue) e.CourseId = dto.CourseId.Value;
            if (!string.IsNullOrEmpty(dto.UserId)) e.UserId = dto.UserId!;
            if (dto.EnrolledAt.HasValue) e.EnrolledAt = dto.EnrolledAt.Value;

            _repo.Update(e);
            await _repo.SaveChangesAsync();

            var updated = await _repo.GetByIdWithDetailsAsync(id);
            return new EnrollmentDto
            {
                Id = updated!.Id,
                CourseId = updated.CourseId,
                CourseTitle = updated.Course?.Title ?? string.Empty,
                UserName = updated.User?.UserName ?? string.Empty,
                EnrolledAt = updated.EnrolledAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            _repo.Delete(existing);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
