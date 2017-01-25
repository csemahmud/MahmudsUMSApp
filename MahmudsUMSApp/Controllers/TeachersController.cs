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
    public class TeachersController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /Teachers/

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            var teacherdbset = db.TeacherDbSet.Include(t => t.Department).Include(t => t.Designation).OrderBy(t => t.TeacherName).Where(t => t.IsActive);
            return View(teacherdbset.ToList());
        }

        //
        // GET: /Teachers/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Teacher teacher = db.TeacherDbSet.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // GET: /Teachers/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName");
            return View();
        }

        //
        // POST: /Teachers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                teacher.CreditsHaveTaken = 0.0000;
                teacher.IsActive = true;
                db.TeacherDbSet.Add(teacher);
                if (db.SaveChanges() > 0) 
                {
                    ViewBag.Message = "Teacher :- " + teacher.TeacherName
                        + " has been saved successfully .";
                }
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            return View(teacher);
        }

        public JsonResult Check_Email (string email)
        {
            var result = db.TeacherDbSet.Count(t => (t.Email == email && t.IsActive)) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Teachers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Teacher teacher = db.TeacherDbSet.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            return View(teacher);
        }

        //
        // POST: /Teachers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                Teacher checkTeacher = db.TeacherDbSet.FirstOrDefault(t => (t.Email == teacher.Email && t.IsActive && t.TeacherID != teacher.TeacherID));
                if(checkTeacher != null)
                {
                    ViewBag.Message = "Error : Email : " + teacher.Email
                        + " Already Exists !!!";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
                    ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
                    return View(teacher);
                }
                db.Entry(teacher).State = EntityState.Modified;
                if(db.SaveChanges() > 0)
                {
                    ViewBag.Message = "Teacher :- " + teacher.TeacherName
                        + " has been updated successfully .";
                }
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            return View(teacher);
        }

        //
        // GET: /Teachers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Teacher teacher = db.TeacherDbSet.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // POST: /Teachers/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Teacher teacher = db.TeacherDbSet.Find(id);
            List<AssignedCourse> assignedCourseList = db.AssignedCourseDbSet.Where(a => (a.TeacherID == teacher.TeacherID && a.IsValid)).ToList();
            foreach (var assignedCourse in assignedCourseList)
            {
                assignedCourse.IsAssigned = false;
                assignedCourse.IsValid = false;
                db.Entry(assignedCourse).State = EntityState.Modified;
                db.SaveChanges();
            }
            teacher.IsActive = false;
            db.Entry(teacher).State = EntityState.Modified;
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