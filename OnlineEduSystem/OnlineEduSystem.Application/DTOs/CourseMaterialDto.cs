using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.DTOs
{
    public class CourseMaterialDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public MaterialType Type { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
