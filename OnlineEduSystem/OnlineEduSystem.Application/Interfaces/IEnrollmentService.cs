using OnlineEduSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto?> GetByIdAsync(int id);
        Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
        Task<EnrollmentDto?> UpdateAsync(int id, UpdateEnrollmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
