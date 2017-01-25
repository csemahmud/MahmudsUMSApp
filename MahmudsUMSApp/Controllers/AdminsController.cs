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
    //
    // GET: /Admins/UnAuthorizedAccess

    public class AdminsController : Controller
    {
        private RootProjDBContext db = new RootProjDBContext();

        public ActionResult UnAuthorizedAccess()
        {
            ViewBag.Message = "You Are NOT Authorized to View This";
            return View("~/Views/Shared/UnAuthorizedAccess.cshtml");
        }

        //
        // GET: /Admins/

        public ActionResult Index()
        {
            if(Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            ViewBag.Message = "Hello, " + Session["AdminName"].ToString();
            return View(db.AdminDbSet.ToList());
        }

        //
        // GET: /Admins/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Admin admin = db.AdminDbSet.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        //
        // GET: /Admins/Create

        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            return View();
        }

        //
        // POST: /Admins/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                Admin checkAdmin = db.AdminDbSet.FirstOrDefault(a => a.Email == admin.Email);
                if (checkAdmin == null)
                {
                    db.AdminDbSet.Add(admin);
                    if(db.SaveChanges() > 0)
                    {
                        ViewBag.Message = "Admin with email : "
                            + admin.Email + " has been saved successfully";
                    }
                }
                else
                {
                    ViewBag.Message = "Error : email : "
                            + admin.Email + " already exists.";
                }
            }

            return View(admin);
        }

        //
        // GET: /Admins/LogIn

        public ActionResult LogIn()
        {
            if (Session["Email"] != null)
            {
                return RedirectToAction("Index");
            }
            if(db.AdminDbSet.Where(a => a.IsActive).Count() <= 0)
            {
                db.AdminDbSet.Add(
                    new Admin {
                        AdminName = "Mahmudul Hasan Khan",
                        Email = "cse.mahmudul@gmail.com",
                        Password = "12345",
                        IsActive = true
                    });
                db.SaveChanges();
            }
            return View();
        }

        //
        // POST: /Admins/LogIn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(Admin admin)
        {
            if (Session["Email"] != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Admin checkAdmin = db.AdminDbSet.FirstOrDefault(a => (a.Email == admin.Email && a.Password == admin.Password && a.IsActive));
                if (checkAdmin != null)
                {
                    Session["AdminName"] = checkAdmin.AdminName;
                    if (Session["AdminName"] == null)
                    {
                        Session["AdminName"] = String.Empty;
                    }
                    Session["Email"] = checkAdmin.Email;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Error : Invalid Email or Password !!!";
                }
            }

            return View();
        }

        //
        // GET: /Admins/LogOut

        public ActionResult LogOut()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            string name = Session["AdminName"].ToString(); 
            Session["AdminName"] = null;
            Session["Email"] = null;
            ViewBag.Message = "Dear " + name
                + ", You are Logged out .\nThanks for using this Software .";
            return View();
        }

        //
        // GET: /Admins/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Admin admin = db.AdminDbSet.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        //
        // POST: /Admins/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            if (ModelState.IsValid)
            {
                Admin checkAdmin = db.AdminDbSet.FirstOrDefault(a => (a.Email == admin.Email && a.AdminID != admin.AdminID));
                if (checkAdmin == null)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.Message = "Admin with email : "
                            + admin.Email + " has been updated successfully";
                    }
                }
                else
                {
                    ViewBag.Message = "Error : email : "
                            + admin.Email + " already exists.";
                }
            }
            return View(admin);
        }

        //
        // GET: /Admins/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Admin admin = db.AdminDbSet.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        //
        // POST: /Admins/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("UnAuthorizedAccess");
            }
            Admin admin = db.AdminDbSet.Find(id);
            db.AdminDbSet.Remove(admin);
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