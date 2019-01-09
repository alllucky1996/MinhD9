using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FashionGo.Models;
using FashionGo.Models.Entities;

namespace FashionGo.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        // GET: Admin/Categories
        public ActionResult Index()
        {
            var Users = db.Users;
            var model = new List<UserViewModel>();
            foreach (var user in Users)
            {
                var u = new UserViewModel(user);
                model.Add(u);
            }
            return View(model);
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( UserViewModel User)
        {
          
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = User.UserName, Email = User.Email, Address = User.Address, FullName = User.FullName,PhoneNumber = User.PhoneNumber};
                var result = db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(User);
;
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.Users.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var model = new UserViewModel(item);
            return View(model);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var item = db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                item.FullName = user.FullName;
                item.Email = user.Email;
                item.Address = user.Address;
                item.PhoneNumber = user.PhoneNumber;
               db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(user);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            var itemview = new UserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(itemview);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var category = db.Users.Find(id);
            db.Users.Remove(category);
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
