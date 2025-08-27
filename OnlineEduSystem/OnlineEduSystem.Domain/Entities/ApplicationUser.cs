using OnlineEduSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Entities
{
    public class ApplicationUser
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;//bu da listelerde belirtilen iisim soyism
        public UserRole Role { get; set; }
        public string UserName { get; set; } = null!;//kullanıcı ismi girişyaparken kullanılan
        public string PasswordHash { get; set; } = null!; // Hashlenmiş şifre
        // Eğitmen olarak açtığı kurslar
        public ICollection<Course>? Courses { get; set; }

        // Öğrenci olarak kayıtlı olduğu kurslar
        public ICollection<Enrollment>? Enrollments { get; set; }
    }

}
