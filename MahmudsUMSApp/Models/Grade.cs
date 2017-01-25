using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    [Table("Grade")]
    public class Grade
    {
        public int GradeID { set; get; }
        public string GradeLetter { set; get; }
        public virtual List<Exam> ExamList { set; get; }
    }
}