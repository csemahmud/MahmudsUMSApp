using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MahmudsUMSApp.Models
{
    [Table("Room")]
    public class Room
    {
        public int RoomID { set; get; }
        public string RoomNo { set; get; }
        public virtual List<AllocatedRoom> AllocatedRoomList { set; get; }
    }
}