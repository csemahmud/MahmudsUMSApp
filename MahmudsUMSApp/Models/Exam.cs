using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    [Table("Exam")]
    public class Exam
    {
        public int ExamID { set; get; }
        public virtual Student Student { set; get; }
        public int StudentID { set; get; }
        public virtual Course Course { set; get; }
        public int CourseID { set; get; }
        public DateTime EnrollmentDate { set; get; }
        public virtual Grade Grade { set; get; }
        public int GradeID { set; get; }

        public bool IsGradeSubmitted { set; get; }
        public bool IsValid { set; get; }
    }
}