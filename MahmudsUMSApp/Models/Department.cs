using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MahmudsUMSApp.Models
{
    [Table("Department")]
    public class Department
    {
        public int DepartmentID { set; get; }

        [Required(ErrorMessage = "Error : Department Code can not be empty !!!")]
        [Remote("Check_DeptCode", "Departments",
            ErrorMessage = "Error : This Department Code already exists !!!")]
        [Display(Name = "Department Code :-")]
        public string DeptCode { set; get; }

        [Required(ErrorMessage = "Error : Department Name can not be empty !!!")]
        [Remote("Check_DeptName", "Departments",
            ErrorMessage = "Error : This Department Name already exists !!!")]
        [Display(Name = "Department Name :-")]
        public string DeptName { set; get; }

        public virtual List<Teacher> TeacherList { set; get; }
        public virtual List<Course> CourseList { set; get; }
    }
}