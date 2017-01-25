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
    public class DepartmentsController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /Departments/

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            return View(db.DepartmentDbSet.ToList());
        }

        //
        // GET: /Departments/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Department department = db.DepartmentDbSet.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // GET: /Departments/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            return View();
        }

        //
        // POST: /Departments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                db.DepartmentDbSet.Add(department);
                if (db.SaveChanges() > 0) {
                    ViewBag.Message = "Department :- " + department.DeptCode
                        + " has been SAVED successfully .";
                }
            }

            return View(department);
        }

        public JsonResult Check_DeptCode(String deptCode) 
        {
            var result = db.DepartmentDbSet.Count(d => d.DeptCode == deptCode) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_DeptName(String deptName)
        {
            var result = db.DepartmentDbSet.Count(d => d.DeptName == deptName) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Departments/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Department department = db.DepartmentDbSet.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Departments/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                Department chkDept1 = db.DepartmentDbSet.FirstOrDefault(d => (d.DeptCode == department.DeptCode && d.DepartmentID != department.DepartmentID));
                if(chkDept1 != null)
                {
                    ViewBag.Message = "Department Code : " + chkDept1.DeptCode
                        + " Already Exists !!!";
                    return View(department);
                }
                Department chkDept2 = db.DepartmentDbSet.FirstOrDefault(d => (d.DeptName == department.DeptName && d.DepartmentID != department.DepartmentID));
                if (chkDept2 != null)
                {
                    ViewBag.Message = "Department Name : " + chkDept2.DeptName
                        + " Already Exists !!!";
                    return View(department);
                }
                db.Entry(department).State = EntityState.Modified;
                if (db.SaveChanges() > 0) {
                    ViewBag.Message = "Department :- " + department.DeptCode
                        + " has been UPDATED successfully .";
                }
            }
            return View(department);
        }

        //
        // GET: /Departments/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Department department = db.DepartmentDbSet.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Departments/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Department department = db.DepartmentDbSet.Find(id);
            List<Teacher> teacherList = db.TeacherDbSet.Where(t => t.DepartmentID == department.DepartmentID).ToList();
            foreach (var teacher in teacherList)
            {
                db.TeacherDbSet.Remove(teacher);
            }
            db.SaveChanges();
            List<Student> studentList = db.StudentDbSet.Where(s => s.DepartmentID == department.DepartmentID).ToList();
            foreach (var student in studentList)
            {
                db.StudentDbSet.Remove(student);
            }
            db.SaveChanges();
            db.DepartmentDbSet.Remove(department);
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