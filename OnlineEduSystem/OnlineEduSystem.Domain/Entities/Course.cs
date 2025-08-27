using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }                     // Kurs kimliği
        public string Title { get; set; }               // Kurs adı
        public string? Description { get; set; }        // Açıklama (opsiyonel)
        public int Capacity { get; set; }               // Kontenjan

        // İlişkiler
        public ICollection<Enrollment>? Enrollments { get; set; }  // Kayıt olan öğrenciler
        public string? InstructorId { get; set; }                  // Eğitmen Id (User)
        public ApplicationUser? Instructor { get; set; }           // Eğitmen objesi
        public ICollection<CourseMaterial>? Materials { get; set; }
    }

}
