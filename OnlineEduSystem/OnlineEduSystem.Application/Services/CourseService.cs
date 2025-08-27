// OnlineEduSystem.Application/Services/CourseService.cs
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
namespace OnlineEduSystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        public CourseService(ICourseRepository repo) => _repo = repo;

        public async Task<List<CourseDto>> GetAllAsync()
        {
            // artık detaylı dolduracağız
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(e => new CourseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Capacity = e.Capacity,
                InstructorName = e.Instructor?.FullName,            // navigation’dan
                CurrentEnrollmentCount = e.Enrollments?.Count ?? 0      // kayıt sayısı
            })
            .ToList();
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdWithDetailsAsync(id);
            if (e == null) return null;
            return new CourseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Capacity = e.Capacity,
                InstructorName = e.Instructor?.FullName,
                CurrentEnrollmentCount = e.Enrollments?.Count ?? 0
            };
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            var entity = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Capacity = dto.Capacity,
                InstructorId = dto.InstructorId
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            // Detaylı dönmeye gerek yok; client ID’ye göre GET yapacak:
            return new CourseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Capacity = entity.Capacity,
                InstructorName = null,
                CurrentEnrollmentCount = 0
            };
        }

        public async Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) e.Title = dto.Title!;
            if (dto.Description is not null) e.Description = dto.Description;
            if (dto.Capacity.HasValue) e.Capacity = dto.Capacity.Value;
            if (!string.IsNullOrEmpty(dto.InstructorId)) e.InstructorId = dto.InstructorId!;

            _repo.Update(e);
            await _repo.SaveChangesAsync();

            // Basit dönüş
            return new CourseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Capacity = e.Capacity,
                InstructorName = null,
                CurrentEnrollmentCount = 0
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;
            _repo.Delete(e);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}