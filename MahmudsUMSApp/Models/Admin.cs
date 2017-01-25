using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    [Table("Admin")]
    public class Admin
    {
        public int AdminID { set; get; }

        public string AdminName { set; get; }

        [Required(ErrorMessage = "Error : Email can not be empty !!!")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Error : Password can not be empty !!!")]
        public string Password { set; get; }

        public bool IsActive { set; get; }
    }
}