using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    /// <summary>
    /// Developed By : MAHMUDUL HASAN KHAN CSE
    /// on 07-December-2016 in Dhaka, Bangladesh
    /// </summary>
    public class RootProjDBContext : DbContext
    {
        public DbSet<Admin> AdminDbSet { set; get; }
        public DbSet<Department> DepartmentDbSet { set; get; }
        public DbSet<Designation> DesignationDbSet { set; get; }
        public DbSet<Teacher> TeacherDbSet { set; get; }
        public DbSet<Semester> SemesterDbSet { set; get; }
        public DbSet<Course> CourseDbSet { set; get; }
        public DbSet<AssignedCourse> AssignedCourseDbSet { set; get; }
        public DbSet<WeekDay> WeekDayDbSet { set; get; }
        public DbSet<Room> RoomDbSet { set; get; }
        public DbSet<AllocatedRoom> AllocatedRoomDbSet { set; get; }
        public DbSet<Student> StudentDbSet { set; get; }
        public DbSet<Grade> GradeDbSet { set; get; }
        public DbSet<Exam> ExamDbSet { set; get; }

        public RootProjDBContext() : base("name=RootProjDBContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().HasRequired(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Teacher>().HasRequired(t => t.Designation).WithMany().HasForeignKey(t => t.DesignationID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Student>().HasRequired(s => s.Department).WithMany().HasForeignKey(s => s.DepartmentID).WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}