// OnlineEduSystem.Application/Interfaces/ICourseService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineEduSystem.Application.DTOs;

namespace OnlineEduSystem.Application.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
