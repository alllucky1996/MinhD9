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
using Microsoft.AspNet.Identity.EntityFramework;

namespace FashionGo.Areas.Admin.Controllers
{
    public class RoleController : AdminController
    {
        // GET: Admin/Categories
        public ActionResult Index()
        {
            var role = db.Roles;
            var model = new List<RoleViewModel>();
            foreach (var item in role)
            {
                var u = new RoleViewModel() {Id = item.Id, Name = item.Name };
                model.Add(u);
            }
            return View(model);
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Roles.Find(id);
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
        public ActionResult Create( RoleViewModel model)
        {
          
            if (ModelState.IsValid)
            {
                var result = db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole() { Id = model.Id, Name = model.Name});
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
            var item = db.Roles.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            
            return View(new RoleViewModel() { Id = item.Id, Name = item.Name});
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
               var item= db.Roles.Where(o => o.Id == model.Id).FirstOrDefault();
                item.Name = model.Name;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(model);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Roles.Find(id);
            
            var itemview = new RoleViewModel() { Id = user.Id,Name = user.Name};
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
            var category = db.Roles.Find(id);
            db.Roles.Remove(category);
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
