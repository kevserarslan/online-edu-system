
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Enums;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OnlineEduSystem.Infrastructure.Data
{
    public static class AppDbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Veritabanı yoksa oluştur
            context.Database.EnsureCreated();

            // 1) Kullanıcıları (eğitmen ve öğrenci) ekle
            if (!context.ApplicationUsers.Any())
            {
                var instructor = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "Örnek Eğitmen",
                    UserName = "eğitmen1",
                    Role = UserRole.Instructor
                };
                var student = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "Örnek Öğrenci",
                    UserName = "ogrenci1",
                    Role = UserRole.Student
                };

                context.ApplicationUsers.AddRange(instructor, student);
                context.SaveChanges();
            }

            // 2) Kursları ekle
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course { Title = "Yapay Zeka", Capacity = 50, InstructorId = context.ApplicationUsers.First(u => u.Role == UserRole.Instructor).Id },
                    new Course { Title = "Veri Bilimi", Capacity = 40, InstructorId = context.ApplicationUsers.First(u => u.Role == UserRole.Instructor).Id },
                    new Course { Title = "Programlamaya Giriş", Capacity = 60, InstructorId = context.ApplicationUsers.First(u => u.Role == UserRole.Instructor).Id }
                );
                context.SaveChanges();
            }

            // 3) Materyalleri ekle
            if (!context.CourseMaterials.Any())
            {
                var aiCourse = context.Courses.First(c => c.Title == "Yapay Zeka");
                context.CourseMaterials.AddRange(
                    new CourseMaterial
                    {
                        CourseId = aiCourse.Id,
                        Title = "Giriş PDF'si",
                        FilePath = "/uploads/ai-intro.pdf",
                        Type = MaterialType.Pdf
                    },
                    new CourseMaterial
                    {
                        CourseId = aiCourse.Id,
                        Title = "Ders 1 Videosu",
                        FilePath = "/uploads/ai-lesson1.mp4",
                        Type = MaterialType.Video
                    }
                );
                context.SaveChanges();
            }

            // 4) Kayıtları (Enrollments) ekle
            if (!context.Enrollments.Any())
            {
                var studentId = context.ApplicationUsers.First(u => u.Role == UserRole.Student).Id;
                // Örnek olarak tüm kurslara kaydettiriyoruz
                foreach (var course in context.Courses)
                {
                    context.Enrollments.Add(new Enrollment
                    {
                        CourseId = course.Id,
                        UserId = studentId,
                        EnrolledAt = DateTime.UtcNow
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
