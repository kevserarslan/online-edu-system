using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Entities
{
    public class CourseMaterial
    {
        public int Id { get; set; }

        // Hangi kursa ait
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string Title { get; set; } = null!;        // Materyal başlığı
        public string FilePath { get; set; } = null!;     // Kaydedilen dosya yolu (örn. "/uploads/abc.pdf")
        public MaterialType Type { get; set; }            // Pdf, Video, Document vb.
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
