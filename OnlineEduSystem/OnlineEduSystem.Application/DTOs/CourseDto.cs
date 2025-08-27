using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public string? InstructorName { get; set; }
        public int CurrentEnrollmentCount { get; set; }
    }
}
