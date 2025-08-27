using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.DTOs
{
    public class UpdateCourseMaterialDto
    {
        public string? Title { get; set; }
        public string? FilePath { get; set; }
        public MaterialType? Type { get; set; }
    }
}
