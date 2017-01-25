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
    public class CoursesController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /Courses/

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            var coursedbset = db.CourseDbSet.Include(c => c.Department).Include(c => c.Semester).OrderBy(c => c.CourseCode).Where(c => c.IsValid);
            return View(coursedbset.ToList());
        }

        //
        // GET: /Courses/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Course course = db.CourseDbSet.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // GET: /Courses/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName");
            return View();
        }

        //
        // POST: /Courses/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                course.IsValid = true;
                course.AssignedCourseList = new List<AssignedCourse>();
                if(db.TeacherDbSet.Count(t => !t.IsActive) <= 0)
                {
                    db.TeacherDbSet.Add(
                        new Teacher
                        {
                            TeacherName = "Not Yet Assigned",
                            Address = "",
                            Email = "a@b.cd",
                            ContactNo = "",
                            CreditsToBeTaken = 0.0000,
                            CreditsHaveTaken = 0.0000,
                            DesignationID = 1,
                            DepartmentID = course.DepartmentID,
                            IsActive = false
                        });
                    db.SaveChanges();
                }
                course.AssignedCourseList.Add(
                    new AssignedCourse { 
                        TeacherID = db.TeacherDbSet.FirstOrDefault(t => !t.IsActive).TeacherID,
                        IsAssigned = false,
                        IsValid = true,
                        IsOutDated = false
                    });
                db.CourseDbSet.Add(course);
                if (db.SaveChanges() > 0) {
                    ViewBag.Message = "Course :- " + course.CourseName
                        + " has been saved successullly .";
                }
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }

        public JsonResult Check_CourseCode(String courseCode)
        {
            var result = db.CourseDbSet.Count(c => (c.CourseCode == courseCode && c.IsValid)) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_CourseName(String courseName)
        {
            var result = db.CourseDbSet.Count(c => (c.CourseName == courseName && c.IsValid)) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Courses/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Course course = db.CourseDbSet.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }

        //
        // POST: /Courses/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course, double PreCredit)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                Course checkCourse1 = db.CourseDbSet.FirstOrDefault(c => (c.CourseCode == course.CourseCode && c.IsValid && c.CourseID != course.CourseID));
                if(checkCourse1 != null)
                {
                    ViewBag.Message = "Error : Course Code : " + checkCourse1.CourseCode + " Already Exists !!!";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
                    return View(course);
                }
                Course checkCourse2 = db.CourseDbSet.FirstOrDefault(c => (c.CourseName == course.CourseName && c.IsValid && c.CourseID != course.CourseID));
                if (checkCourse2 != null)
                {
                    ViewBag.Message = "Error : Course Name : " + checkCourse2.CourseName + " Already Exists !!!";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
                    return View(course);
                }
                if(course.Credit != PreCredit)
                {
                    AssignedCourse anAssignedCourse = db.AssignedCourseDbSet.Include(a => a.Teacher).FirstOrDefault(a => (a.CourseID == course.CourseID && a.IsAssigned && a.IsValid && !a.IsOutDated));
                    if(anAssignedCourse != null)
                    {
                        anAssignedCourse.Teacher.CreditsHaveTaken += course.Credit - PreCredit;
                        db.Entry(anAssignedCourse.Teacher).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                db.Entry(course).State = EntityState.Modified;
                if(db.SaveChanges() > 0)
                {
                    ViewBag.Message = "Course :- " + course.CourseName
                        + " has been updated successfully .";
                }
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }

        //
        // GET: /Courses/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Course course = db.CourseDbSet.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // POST: /Courses/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Course course = db.CourseDbSet.Find(id);
            
            List<AssignedCourse> assignedCourseList = db.AssignedCourseDbSet.Include(a => a.Teacher).Where(a => (a.CourseID == course.CourseID && !a.IsOutDated)).ToList();
            foreach(var assignedCourse in assignedCourseList)
            {
                if (assignedCourse.IsAssigned)
                {
                    assignedCourse.Teacher.CreditsHaveTaken -= course.Credit;
                    db.Entry(assignedCourse.Teacher).State = EntityState.Modified;
                    db.SaveChanges();
                }
                assignedCourse.IsAssigned = false;
                assignedCourse.IsValid = false;
                assignedCourse.IsOutDated = true;
                db.Entry(assignedCourse).State = EntityState.Modified;
                db.SaveChanges();
            }

            List<AllocatedRoom> allocatedRoomList = db.AllocatedRoomDbSet.Where(a => (a.CourseID == course.CourseID && a.IsAllocated)).ToList();
            foreach (var allocatedRoom in allocatedRoomList)
            {
                allocatedRoom.IsAllocated = false;
                db.Entry(allocatedRoom).State = EntityState.Modified;
                db.SaveChanges();
            }

            course.IsValid = false;
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}