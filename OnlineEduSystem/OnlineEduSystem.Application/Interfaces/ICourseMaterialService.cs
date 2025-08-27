using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Interfaces
{
    public interface ICourseMaterialService
    {
        Task<List<CourseMaterialDto>> GetAllAsync();
        Task<CourseMaterialDto?> GetByIdAsync(int id);
        Task<CourseMaterialDto> CreateAsync(CreateCourseMaterialDto dto);
        Task<CourseMaterialDto?> UpdateAsync(int id, UpdateCourseMaterialDto dto);
        Task<List<CourseMaterialDto>> GetByCourseIdAsync(int courseId);
        Task<bool> DeleteAsync(int id);
        
    }
}
