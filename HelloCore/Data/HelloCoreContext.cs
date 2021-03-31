using HelloCore.Areas.Identity.Data;
using HelloCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloCore.Data
{
    public class HelloCoreContext : IdentityDbContext<CustomUser>
    {
        public HelloCoreContext(DbContextOptions<HelloCoreContext> options) : base(options)
        {
        }

        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling").Property(p => p.Prijs).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<StudentCourse>()
                .ToTable("StudentCourse");
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(sc => sc.StudentId);
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CourseId);
        }
    }
}
