using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    public class SeedData : DropCreateDatabaseIfModelChanges<RootProjDBContext>
    {
        protected override void Seed(RootProjDBContext context)
        {
            context.AdminDbSet.Add(new Admin { 
                AdminName = "Mahmudul Hasan Khan",
                Email = "cse.mahmudul@gmail.com",
                Password = "12345",
                IsActive = true
            });
            context.SaveChanges();

            context.DepartmentDbSet.Add(new Department {
                DeptCode = "CSE",
                DeptName = "Computer Science and Engineering"
            });
            context.DepartmentDbSet.Add(new Department
            {
                DeptCode = "BA",
                DeptName = "Business Administration"
            });
            context.SaveChanges();

            context.DesignationDbSet.Add(new Designation { DsgName = "Professor" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Associate Professor" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Assistant Profeassor" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Lecturer" });
            context.SaveChanges();

            context.TeacherDbSet.Add(
                new Teacher {
                    TeacherName = "Not Yet Assigned",
                    Address = "",
                    Email = "a@b.cd",
                    ContactNo = "",
                    CreditsToBeTaken = 0.0000,
                    CreditsHaveTaken = 0.0000,
                    DesignationID = 1,
                    DepartmentID = 1,
                    IsActive = false
                });

            context.TeacherDbSet.Add(
                new Teacher
                {
                    TeacherName = "Zahirul Alam Taimoon",
                    Address = "Shyamoli",
                    Email = "taimoon@mail.com",
                    ContactNo = "0123456789",
                    CreditsToBeTaken = 16.0000,
                    CreditsHaveTaken = 0.0000,
                    DesignationID = 1,
                    DepartmentID = 1,
                    IsActive = true
                });

            context.TeacherDbSet.Add(
                new Teacher
                {
                    TeacherName = "Foysal Ahmed",
                    Address = "Kawran Bazar",
                    Email = "foysal@mail.com",
                    ContactNo = "0123456789",
                    CreditsToBeTaken = 10.0000,
                    CreditsHaveTaken = 0.0000,
                    DesignationID = 4,
                    DepartmentID = 1,
                    IsActive = true
                });

            context.TeacherDbSet.Add(
                new Teacher
                {
                    TeacherName = "Mamun Ur Rashid",
                    Address = "Mirpur",
                    Email = "mamun@mail.com",
                    ContactNo = "0123456789",
                    CreditsToBeTaken = 17.0000,
                    CreditsHaveTaken = 0.0000,
                    DesignationID = 1,
                    DepartmentID = 2,
                    IsActive = true
                });

            context.SaveChanges();

            context.SemesterDbSet.Add(new Semester { SemesterName = "1st Year 1st Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "1st Year 2nd Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "2nd Year 1st Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "2nd Year 2nd Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "3rd Year 1st Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "3rd Year 2nd Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "4th Year 1st Semester" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "4th Year 2nd Semester" });

            context.SaveChanges();

            context.CourseDbSet.Add(new Course { 
                CourseCode = "CSE101",
                CourseName = "JAVA SE Programming",
                Credit = 8.0000,
                Description = "Programming",
                DepartmentID = 1,
                SemesterID = 1,
                IsValid = true
            });

            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE201",
                CourseName = "ASP.Net Development",
                Credit = 8.0000,
                Description = "Software Development",
                DepartmentID = 1,
                SemesterID = 3,
                IsValid = true
            });

            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE301",
                CourseName = "PHP Web Development",
                Credit = 1.0000,
                Description = "Web Development",
                DepartmentID = 1,
                SemesterID = 5,
                IsValid = true
            });

            context.CourseDbSet.Add(new Course
            {
                CourseCode = "BA401",
                CourseName = "Management Information System",
                Credit = 8.0000,
                Description = "Information System about Management",
                DepartmentID = 2,
                SemesterID = 7,
                IsValid = true
            });

            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE403",
                CourseName = "Image Processing",
                Credit = 8.0000,
                Description = "Research with Image Processing",
                DepartmentID = 1,
                SemesterID = 7,
                IsValid = true
            });

            context.SaveChanges();

            context.AssignedCourseDbSet.Add(
                new AssignedCourse {
                    CourseID = 1,
                    TeacherID = 1,
                    IsAssigned = false,
                    IsValid = true,
                    IsOutDated = false
                });

            context.AssignedCourseDbSet.Add(
                new AssignedCourse
                {
                    CourseID = 2,
                    TeacherID = 1,
                    IsAssigned = false,
                    IsValid = true,
                    IsOutDated = false
                });

            context.AssignedCourseDbSet.Add(
                new AssignedCourse
                {
                    CourseID = 3,
                    TeacherID = 1,
                    IsAssigned = false,
                    IsValid = true,
                    IsOutDated = false
                });

            context.AssignedCourseDbSet.Add(
                new AssignedCourse
                {
                    CourseID = 4,
                    TeacherID = 1,
                    IsAssigned = false,
                    IsValid = true,
                    IsOutDated = false
                });

            context.AssignedCourseDbSet.Add(
                new AssignedCourse
                {
                    CourseID = 5,
                    TeacherID = 1,
                    IsAssigned = false,
                    IsValid = true,
                    IsOutDated = false
                });

            context.SaveChanges();

            context.WeekDayDbSet.Add(new WeekDay { DayName = "Sun" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Mon" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Tue" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Wed" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Thu" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Fri" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Sat" });

            context.SaveChanges();

            context.RoomDbSet.Add(new Room { RoomNo = "A-101" });
            context.RoomDbSet.Add(new Room { RoomNo = "A-201" });
            context.RoomDbSet.Add(new Room { RoomNo = "A-301" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-10" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-20" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-30" });
            context.RoomDbSet.Add(new Room { RoomNo = "M-3513" });

            context.SaveChanges();

            context.StudentDbSet.Add(
                new Student 
                {
                    StudentName = "MAHMUDUL HASAN KHAN",
                    RegNo = "CSE2013001",
                    Email = "hasan417@diu.edu.bd",
                    ContactNo = "+8801701757766",
                    Address = "New - Market, Dhaka",
                    AdmissionDate = Convert.ToDateTime("10/06/2013"),
                    DepartmentID = 1,
                    IsActive = true
                });

            context.StudentDbSet.Add(
                new Student
                {
                    StudentName = "ALIA BHATT",
                    RegNo = "CSE2013002",
                    Email = "alia@mail.com",
                    ContactNo = "0123456789",
                    Address = "Mumbai, India",
                    AdmissionDate = Convert.ToDateTime("10/06/2013"),
                    DepartmentID = 1,
                    IsActive = true
                });

            context.StudentDbSet.Add(
                new Student
                {
                    StudentName = "SHRADDHA KAPOOR",
                    RegNo = "CSE2013003",
                    Email = "shraddha@mail.com",
                    ContactNo = "013513",
                    Address = "USA",
                    AdmissionDate = Convert.ToDateTime("10/06/2013"),
                    DepartmentID = 1,
                    IsActive = true
                });

            context.StudentDbSet.Add(
                new Student
                {
                    StudentName = "YAMI GAUTAM",
                    RegNo = "BA2017001",
                    Email = "yami@mail.com",
                    ContactNo = "0123456789",
                    Address = "Chittagong",
                    AdmissionDate = Convert.ToDateTime("03/01/2017"),
                    DepartmentID = 2,
                    IsActive = true
                });

            context.StudentDbSet.Add(
                new Student
                {
                    StudentName = "YAMI GAUTAM",
                    RegNo = "BA2017002",
                    AdmissionDate = Convert.ToDateTime("03/01/2017"),
                    DepartmentID = 2,
                    IsActive = true
                });

            context.SaveChanges();

            context.GradeDbSet.Add(new Grade { GradeLetter = "Result Not Published Yet" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "A+" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "A" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "A-" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B+" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B-" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "C+" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "C" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "D" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "F" });

            context.SaveChanges();

            context.ExamDbSet.Add(
                new Exam 
                {
                    StudentID = 1,
                    CourseID = 1,
                    GradeID = 1,
                    EnrollmentDate = Convert.ToDateTime("06/01/2017"),
                    IsGradeSubmitted = false,
                    IsValid = true
                });

            context.ExamDbSet.Add(
                new Exam
                {
                    StudentID = 1,
                    CourseID = 2,
                    GradeID = 1,
                    EnrollmentDate = Convert.ToDateTime("06/01/2017"),
                    IsGradeSubmitted = false,
                    IsValid = true
                });

            context.ExamDbSet.Add(
                new Exam
                {
                    StudentID = 1,
                    CourseID = 3,
                    GradeID = 1,
                    EnrollmentDate = Convert.ToDateTime("06/01/2017"),
                    IsGradeSubmitted = false,
                    IsValid = true
                });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}