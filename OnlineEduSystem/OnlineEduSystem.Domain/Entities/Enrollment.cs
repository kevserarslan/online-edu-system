﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }

}
