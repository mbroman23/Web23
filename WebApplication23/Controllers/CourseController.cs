using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication23.Models;
using WebApplication23.Models.Classes;

namespace WebApplication23.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Course
        public ActionResult Index()
        {
            return View(db.CourseClasses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseClass courseClass = db.CourseClasses.Find(id);
            if (courseClass == null)
            {
                return HttpNotFound();
            }
            return View(courseClass);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] CourseClass courseClass)
        {
            if (ModelState.IsValid)
            {
                db.CourseClasses.Add(courseClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseClass);
        }

        public ActionResult CourseToggle(int id)
        {
            CourseClass CurrentClass = db.CourseClasses.Where(g => g.Id == id).FirstOrDefault();
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (CurrentClass.AttendingStudents.Contains(CurrentUser))
            {
                CurrentClass.AttendingStudents.Remove(CurrentUser);
                db.SaveChanges();
            }
            else
            {
                CurrentClass.AttendingStudents.Add(CurrentUser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Course/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseClass courseClass = db.CourseClasses.Find(id);
            if (courseClass == null)
            {
                return HttpNotFound();
            }
            return View(courseClass);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] CourseClass courseClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseClass);
        }

        // GET: Course/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseClass courseClass = db.CourseClasses.Find(id);
            if (courseClass == null)
            {
                return HttpNotFound();
            }
            return View(courseClass);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseClass courseClass = db.CourseClasses.Find(id);
            db.CourseClasses.Remove(courseClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
