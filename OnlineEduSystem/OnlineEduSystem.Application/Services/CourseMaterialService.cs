using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Services
{
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly ICourseMaterialRepository _repo;

        public CourseMaterialService(ICourseMaterialRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CourseMaterialDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(e => new CourseMaterialDto
            {
                Id = e.Id,
                CourseId = e.CourseId,
                Title = e.Title,
                FilePath = e.FilePath,
                Type = e.Type,
                UploadedAt = e.UploadedAt
            })
            .ToList();
        }

        public async Task<CourseMaterialDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;

            return new CourseMaterialDto
            {
                Id = e.Id,
                CourseId = e.CourseId,
                Title = e.Title,
                FilePath = e.FilePath,
                Type = e.Type,
                UploadedAt = e.UploadedAt
            };
        }

        public async Task<CourseMaterialDto> CreateAsync(CreateCourseMaterialDto dto)
        {
            var entity = new CourseMaterial
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                FilePath = dto.FilePath,
                Type = dto.Type
                // UploadedAt ctor’da zaten UtcNow atılıyor
            };

            // 1) Ekleme
            await _repo.AddAsync(entity);
            // 2) Veritabanına kaydetme
            await _repo.SaveChangesAsync();

            return new CourseMaterialDto
            {
                Id = entity.Id,
                CourseId = entity.CourseId,
                Title = entity.Title,
                FilePath = entity.FilePath,
                Type = entity.Type,
                UploadedAt = entity.UploadedAt
            };
        }

        public async Task<CourseMaterialDto?> UpdateAsync(int id, UpdateCourseMaterialDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Sadece gelen alanları güncelle
            if (!string.IsNullOrEmpty(dto.Title)) existing.Title = dto.Title!;
            if (!string.IsNullOrEmpty(dto.FilePath)) existing.FilePath = dto.FilePath!;
            if (dto.Type.HasValue) existing.Type = dto.Type.Value;

            // 1) Değişikliği işaretle
            _repo.Update(existing);
            // 2) Veritabanına kaydet
            await _repo.SaveChangesAsync();

            return new CourseMaterialDto
            {
                Id = existing.Id,
                CourseId = existing.CourseId,
                Title = existing.Title,
                FilePath = existing.FilePath,
                Type = existing.Type,
                UploadedAt = existing.UploadedAt
            };
        }
        public async Task<List<CourseMaterialDto>> GetByCourseIdAsync(int courseId)
        {
            var items = await _repo.GetByCourseIdAsync(courseId);
            return items.Select(e => new CourseMaterialDto
            {
                Id = e.Id,
                CourseId = e.CourseId,
                Title = e.Title,
                FilePath = e.FilePath,
                Type = e.Type,
                UploadedAt = e.UploadedAt
            }).ToList();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            // 1) Silme işaretle
            _repo.Delete(existing);
            // 2) Veritabanına kaydet
            await _repo.SaveChangesAsync();

            return true;
        }

    }
}
