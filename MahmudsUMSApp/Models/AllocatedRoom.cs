using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MahmudsUMSApp.Models
{
    [Table("AllocatedRoom")]
    public class AllocatedRoom
    {
        public int AllocatedRoomID { set; get; }
        public virtual Course Course { set; get; }
        public int CourseID { set; get; }
        public virtual Room Room { set; get; }
        public int RoomID { set; get; }
        public virtual WeekDay WeekDay { set; get; }
        public int WeekDayID { set; get; }

        [Required(ErrorMessage = "Start Time is required !!!")]
        [Remote("Check_StartTime", "AllocatedRooms",
            ErrorMessage = "Please input Start Time ONLY in this format HH:mm (within 24 hours)")]
        [Display(Name = "Start Time :- HH:mm (within 24 hours)")]
        public string StartTime { set; get; }

        [Required(ErrorMessage = "End Time is required !!!")]
        [Remote("Check_EndTime", "AllocatedRooms",
            ErrorMessage = "Please input End Time ONLY in this format HH:mm (within 24 hours)")]
        [Display(Name = "End Time :- HH:mm (within 24 hours)")]
        public string EndTime { set; get; }
        public bool IsAllocated { set; get; }
    }
}