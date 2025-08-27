using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineEduSystem.Domain.Entities;

namespace OnlineEduSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. CourseMaterial ↔ Course (1–N)
            modelBuilder.Entity<CourseMaterial>(entity =>
            {
                entity.HasKey(cm => cm.Id);

                entity
                    .HasOne(cm => cm.Course)
                    .WithMany(c => c.Materials)
                    .HasForeignKey(cm => cm.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 2. Enrollment ↔ Course (1–N)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Enrollment ↔ ApplicationUser (1–N)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity
                    .HasOne(e => e.User)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.User)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ⬇️ Öğrenci bir kursa yalnızca 1 kez kayıt olabilir
                entity.HasIndex(e => new { e.CourseId, e.UserId })
                      .IsUnique();   // <— bunu ekledik
            });

            // 4. Course ↔ ApplicationUser (Instructor) (1–N)
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity
                    .HasOne(c => c.Instructor)
                    .WithMany(u => u.Courses)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // (Opsiyonel) ApplicationUser ↔ CourseMaterial doğrudan bir ilişki yok,
            // materyaller üzerinden eğitmen bilgisine Course → Instructor nav prop ile erişilir.
        }
    }
}
