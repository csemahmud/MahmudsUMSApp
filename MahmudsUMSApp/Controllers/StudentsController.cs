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
    public class StudentsController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /Students/

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            var studentdbset = db.StudentDbSet.Include(s => s.Department).OrderBy(s => s.RegNo).Where(s => s.IsActive);
            return View(studentdbset.ToList());
        }

        //
        // GET: /Students/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Student student = db.StudentDbSet.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // GET: /Students/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            return View();
        }

        //
        // POST: /Students/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                int count = db.StudentDbSet.Count(s => (s.DepartmentID == student.DepartmentID && s.AdmissionDate.Year == student.AdmissionDate.Year)) + 1;
                Department aDepartment = db.DepartmentDbSet.FirstOrDefault(d => d.DepartmentID == student.DepartmentID);
                student.RegNo = aDepartment.DeptCode + student.AdmissionDate.Year + count.ToString("D3");
                student.IsActive = true;
                db.StudentDbSet.Add(student);
                if (db.SaveChanges() > 0) 
                {
                    ViewBag.Message = "Student: " + student.StudentName
                        + " has been registered successfully with Registration No: "
                        + student.RegNo;
                }
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }

        //
        // GET: /Students/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Student student = db.StudentDbSet.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }

        //
        // POST: /Students/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                if (db.SaveChanges() > 0) 
                {
                    ViewBag.Message = "Student: " + student.StudentName
                        + " with Registration No: " + student.RegNo
                        + " has been updated successfully .";
                }
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }

        //
        // GET: /Students/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Student student = db.StudentDbSet.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Students/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Student student = db.StudentDbSet.Find(id);
            student.IsActive = false;
            db.Entry(student).State = EntityState.Modified;
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