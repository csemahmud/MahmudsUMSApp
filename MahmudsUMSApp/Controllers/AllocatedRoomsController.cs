using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MahmudsUMSApp.Models;

namespace MahmudsUMSApp.Controllers
{
    public class AllocatedRoomsController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /AllocatedRooms/

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet.OrderBy(d => d.DeptCode), "DepartmentID", "DeptCode");
            List<Course> courseList = db.CourseDbSet.OrderBy(c => c.CourseCode).Where(c => c.IsValid).ToList();
            foreach (Course course in courseList)
            {
                course.AllocatedRoomList = null;
                course.AllocatedRoomList = new List<AllocatedRoom>();
                course.AllocatedRoomList.Clear();
                course.AllocatedRoomList = db.AllocatedRoomDbSet.Include(a => a.Room).Include(a => a.WeekDay)
                    .Where(a => (a.CourseID == course.CourseID && a.IsAllocated)).ToList();
            }
            return View(courseList);
        }

        public PartialViewResult AllocatedRoomFilteredList(int? departmentID)
        {
            List<Course> courseModel;
            if (departmentID != null)
            {
                courseModel = db.CourseDbSet.OrderBy(c => c.CourseCode).Where(c => (c.DepartmentID == departmentID && c.IsValid)).ToList();
            }
            else
            {
                courseModel = db.CourseDbSet.OrderBy(c => c.CourseCode).Where(c => c.IsValid).ToList();
            }
            foreach (Course course in courseModel)
            {
                course.AllocatedRoomList = null;
                course.AllocatedRoomList = new List<AllocatedRoom>();
                course.AllocatedRoomList.Clear();
                course.AllocatedRoomList = db.AllocatedRoomDbSet.Include(a => a.Room).Include(a => a.WeekDay)
                    .Where(a => (a.CourseID == course.CourseID && a.IsAllocated)).ToList();
            }
            return PartialView("~/Views/AllocatedRooms/_AllocatedRoomFilteredList.cshtml", courseModel);
        }

        private class Result
        {
            public List<object> courses;
        }

        //
        // GET: /AllocatedRooms/JsonData

        public JsonResult JsonData()
        {
            if (Session["Email"] == null)
            {
                string asd = null;
                return Json(asd, JsonRequestBehavior.DenyGet);
            }
            var courses =
                from course in db.CourseDbSet.Include(c => c.Department).Where(c => c.IsValid).ToList()
                select new
                {
                    course.CourseID,
                    course.CourseCode,
                    course.CourseName,
                    course.Department.DepartmentID,
                    course.Department.DeptCode,
                    AllocatedRooms = from allocatedRoom in db.AllocatedRoomDbSet.Include(a => a.Room).Include(a => a.WeekDay).Where(a => (a.CourseID == course.CourseID && a.IsAllocated)).ToList()
                                     select new 
                                     {
                                         allocatedRoom.AllocatedRoomID,
                                         allocatedRoom.Room.RoomNo,
                                         allocatedRoom.WeekDay.WeekDayID,
                                         allocatedRoom.WeekDay.DayName,
                                         allocatedRoom.StartTime,
                                         allocatedRoom.EndTime
                                     }
                };

            Result result = new Result();
            result.courses = new List<object>();
            result.courses.AddRange(courses);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /AllocatedRooms/AngularJs

        public ActionResult AngularJs()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            return View();
        }
        

        //
        // GET: /AllocatedRooms/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            if (allocatedroom == null)
            {
                return HttpNotFound();
            }
            return View(allocatedroom);
        }

        //
        // GET: /AllocatedRooms/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo");
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName");
            return View();
        }

        //
        // POST: /AllocatedRooms/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AllocatedRoom allocatedroom)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                try 
                {
                    int startMinutes = ConvertTimeStrIntoMinutes(allocatedroom.StartTime);
                    int endMinutes = ConvertTimeStrIntoMinutes(allocatedroom.EndTime);
                    if(startMinutes < endMinutes)
                    {
                        if((startMinutes >= 0) && (endMinutes < 1440))
                        {
                            Room room = db.RoomDbSet.Find(allocatedroom.RoomID);
                            WeekDay weekDay = db.WeekDayDbSet.Find(allocatedroom.WeekDayID);
                            Course course = db.CourseDbSet.Find(allocatedroom.CourseID);

                            List<AllocatedRoom> overLappedRoomList = new List<AllocatedRoom>();
                            List<AllocatedRoom> overLappedCourseList = new List<AllocatedRoom>();

                            foreach(AllocatedRoom anAllocatedRoom in
                                db.AllocatedRoomDbSet.Include(a => a.Course).Include(a => a.Room).Include(a => a.WeekDay)
                                .Where(a => (
                                    (a.RoomID == allocatedroom.RoomID || a.CourseID == allocatedroom.CourseID)
                                    && a.WeekDayID == allocatedroom.WeekDayID && a.IsAllocated))
                                )
                            {
                                int allocatedStartMinutes = ConvertTimeStrIntoMinutes(anAllocatedRoom.StartTime);
                                int allocatedEndMinutes = ConvertTimeStrIntoMinutes(anAllocatedRoom.EndTime);
                                if (
                                    ((allocatedStartMinutes <= startMinutes) && (startMinutes < allocatedEndMinutes))
                                    // Suppose, Already Alloctated Room 09:30 to 10:30 
                                    // OverLapped Room 9:30 to 11:00 or 10:00 to 11:00 or 9:45 to 10:15
                                    || ((allocatedStartMinutes < endMinutes) && (endMinutes <= allocatedEndMinutes))
                                    // Suppose, Already Alloctated Room 09:30 to 10:30 
                                    // OverLapped Room 09:00 to 10:00 or 09:00 to 10:30 or 9:45 to 10:15
                                    || ((startMinutes < allocatedStartMinutes) && (allocatedEndMinutes < endMinutes))
                                    // Suppose, Already Alloctated Room 09:30 to 10:30 
                                    // OverLapped Room 09:00 to 11:00
                                    )
                                {
                                    if(anAllocatedRoom.RoomID == allocatedroom.RoomID)
                                    {
                                        overLappedRoomList.Add(anAllocatedRoom);
                                    }
                                    else if(anAllocatedRoom.CourseID == allocatedroom.CourseID)
                                    {
                                        overLappedCourseList.Add(anAllocatedRoom);
                                    }
                                }
                            }
                            if ((overLappedRoomList.Count() <= 0) && (overLappedCourseList.Count() <= 0))
                            {
                                allocatedroom.IsAllocated = true;
                                db.AllocatedRoomDbSet.Add(allocatedroom);
                                if (db.SaveChanges() > 0)
                                {
                                    ViewBag.Message = "Room :- "
                                        + room.RoomNo
                                        + " has been successfully allocated "
                                        + " for course :- " + course.CourseCode
                                        + " from " + allocatedroom.StartTime
                                        + " to " + allocatedroom.EndTime
                                        + " on " + weekDay.DayName;
                                }
                            }
                            else
                            {
                                string errorMessage = string.Empty;
                                if (overLappedRoomList.Count() > 0)
                                {
                                    errorMessage = "Room :- " + room.RoomNo
                                        + " is overlapped with : ";
                                    foreach (AllocatedRoom overLappedRoom in overLappedRoomList)
                                    {
                                        errorMessage += " Course : "
                                            + overLappedRoom.Course.CourseCode
                                            + " Start Time : " + overLappedRoom.StartTime
                                            + " End Time : " + overLappedRoom.EndTime
                                            + " Overlapped From : ";

                                        int allocatedStartMinutes = ConvertTimeStrIntoMinutes(overLappedRoom.StartTime);
                                        int allocatedEndMinutes = ConvertTimeStrIntoMinutes(overLappedRoom.EndTime);

                                        string overLappingStart, overLappingEnd;
                                        if ((allocatedStartMinutes <= startMinutes) && (startMinutes < allocatedEndMinutes))
                                        // Suppose, Already Alloctated Room 09:30 to 10:30 
                                        // OverLapped Room 9:30 to 11:00 or 10:00 to 11:00 or 9:45 to 10:15
                                        {
                                            overLappingStart = allocatedroom.StartTime;
                                            // OverLapping Starts from 9:30 or 10:00 or 9:45 respectively
                                        }
                                        else
                                        {
                                            overLappingStart = overLappedRoom.StartTime;
                                            // Otherwise, OverLapping Starts from 9:30
                                        }
                                        if ((allocatedStartMinutes < endMinutes) && (endMinutes <= allocatedEndMinutes))
                                        // Suppose, Already Alloctated Room 09:30 to 10:30 
                                        // OverLapped Room 09:00 to 10:00 or 09:00 to 10:30 or 9:45 to 10:15
                                        {
                                            overLappingEnd = allocatedroom.EndTime;
                                            // OverLapping Ends in 10:00 or 10:30 or 10:15 respectively
                                        }
                                        else
                                        {
                                            overLappingEnd = overLappedRoom.EndTime;
                                            // Otherwise, OverLapping Ends in 10:30
                                        }

                                        errorMessage += overLappingStart + " to " + overLappingEnd;
                                    }
                                }
                                if (overLappedCourseList.Count() > 0)
                                {
                                    errorMessage += " Course :- " + course.CourseCode
                                        + " is overlapped with : ";
                                    foreach (AllocatedRoom overLappedCourse in overLappedCourseList)
                                    {
                                        errorMessage += " Room : "
                                            + overLappedCourse.Room.RoomNo
                                            + " Start Time : " + overLappedCourse.StartTime
                                            + " End Time : " + overLappedCourse.EndTime
                                            + " Overlapped From : ";

                                        int allocatedStartMinutes = ConvertTimeStrIntoMinutes(overLappedCourse.StartTime);
                                        int allocatedEndMinutes = ConvertTimeStrIntoMinutes(overLappedCourse.EndTime);

                                        string overLappingStart, overLappingEnd;

                                        // Same Logic as Room Overlapping
                                        if ((allocatedStartMinutes <= startMinutes) && (startMinutes < allocatedEndMinutes))
                                        {
                                            overLappingStart = allocatedroom.StartTime;
                                        }
                                        else
                                        {
                                            overLappingStart = overLappedCourse.StartTime;
                                        }
                                        if ((allocatedStartMinutes < endMinutes) && (endMinutes <= allocatedEndMinutes))
                                        {
                                            overLappingEnd = allocatedroom.EndTime;
                                        }
                                        else
                                        {
                                            overLappingEnd = overLappedCourse.EndTime;
                                        }

                                        errorMessage += overLappingStart + " to " + overLappingEnd;
                                    }
                                }

                                errorMessage += " on " + weekDay.DayName;
                                ViewBag.Message = errorMessage;
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Please input time within 24 hours";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Start Time must be before End Time (within 24 hours)";
                    }
                }
                catch(Exception anException)
                {
                    ViewBag.Message = anException.Message;
                }
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
            return View(allocatedroom);
        }

        public PartialViewResult LoadCourseDropDown(int? departmentID)
        {
            if (departmentID != null)
            {
                ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => (c.DepartmentID == departmentID && c.IsValid)), "CourseID", "CourseCode");
            }
            else
            {
                ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            }
            return PartialView("~/Views/AssignedCourses/_LoadCourseDropDown.cshtml");
        }

        private int ConvertTimeStrIntoMinutes(string timeStr) 
        { 
            try
            {
                if(timeStr.Length > 5)
                {
                    return -1;
                }

                if(timeStr.Length == 4)
                {
                    timeStr = "0" + timeStr;
                }

                string hourStr = timeStr[0].ToString() + timeStr[1];
                string minuteStr = timeStr[3].ToString() + timeStr[4];
                int hours = Convert.ToInt32(hourStr);
                int minutes = Convert.ToInt32(minuteStr);

                if ((minutes < 0) || (minutes >= 60)) {
                    return -1;
                }

                minutes += (hours * 60);
                return minutes;
            }
            catch (FormatException aFormatException)
            {
                throw new FormatException(
                    "Please input time ONLY in this format HH:mm (within 24 hours)",
                    aFormatException);
            }
            catch (IndexOutOfRangeException anIndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException(
                    "Please input time ONLY in this format HH:mm (within 24 hours)",
                    anIndexOutOfRangeException);
            }
        }

        public JsonResult Check_StartTime(string startTime)
        {
            bool result = true;
            try 
            {
                int minutes = ConvertTimeStrIntoMinutes(startTime);
                if((minutes < 0)||(minutes >= 1440))
                {
                    result = false;
                }
            }
            catch(Exception)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_EndTime(string endTime)
        {
            bool result = true;
            try
            {
                int minutes = ConvertTimeStrIntoMinutes(endTime);
                if ((minutes < 0) || (minutes >= 1440))
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /AllocatedRooms/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            if (allocatedroom == null)
            {
                return HttpNotFound();
            }
            return View(allocatedroom);
        }

        //
        // POST: /AllocatedRooms/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            allocatedroom.IsAllocated = false;
            db.Entry(allocatedroom).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /AllocatedRooms/UnAllocateAllClassRooms

        public ActionResult UnAllocateAllClassRooms()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.Message = "Are You Sure You Want to Unallocate All Class Rooms ?";
            return View();
        }

        //
        // GET: /AllocatedRooms/UnAllocateConfirmed

        public ActionResult UnAllocateConfirmed()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            List<AllocatedRoom> allocatedRoomList = db.AllocatedRoomDbSet.Where(a => a.IsAllocated).ToList();
            bool IsSuccess = true;
            foreach (var allocatedRoom in allocatedRoomList)
            {
                allocatedRoom.IsAllocated = false;
                db.Entry(allocatedRoom).State = EntityState.Modified;
                if (db.SaveChanges() <= 0) 
                {
                    IsSuccess = false;
                }
            }
            if (IsSuccess)
            {
                ViewBag.Message = "All the class rooms have been unallocated successfully.";
            }
            else
            {
                ViewBag.Message = "Could not Unallocate all the class rooms ";
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}