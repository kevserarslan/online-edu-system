﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.DTOs
{
    public class CreateEnrollmentDto
    {
        public int CourseId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
