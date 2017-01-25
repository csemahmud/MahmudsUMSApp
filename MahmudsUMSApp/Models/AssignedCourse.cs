using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    [Table("AssignedCourse")]
    public class AssignedCourse
    {
        public int AssignedCourseID { set; get; }
        public virtual Teacher Teacher { set; get; }
        public int TeacherID { set; get; }
        public virtual Course Course { set; get; }
        public int CourseID { set; get; }
        public bool IsAssigned { set; get; }
        public bool IsValid { set; get; }
        public bool IsOutDated { set; get; }
    }
}