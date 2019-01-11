﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FashionGo.Models.Entities;
using FashionGo.Models;
using System.Diagnostics;

namespace FashionGo.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ShoppingCart cart = ShoppingCart.Cart;
        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.OrderStatus).Include(o => o.PaymentMethod).Include(o => o.Transport).Include(o => o.User);
            return View(orders.OrderByDescending(o=>o.OrderDate).ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // GET: Admin/Orders/Validate/5
        public ActionResult Validate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            try
            {
                foreach (var d in order.OrderDetails)
                {
                    var p = d.Product;
                    // update bỏ đang chờ bán
                    // hoàn thành thì cho đang chừo về 0 và giảm số lượng đi

                    //p.Amount = p.Amount - p.Pending;
                    //p.Pending = 0;
                    p.S = p.S - p.PendingS; p.PendingS = 0;
                    p.S = p.M - p.PendingM; p.PendingM = 0;
                    p.S = p.L - p.PendingL; p.PendingL = 0;
                    p.S = p.XL - p.PendingXL; p.PendingXL = 0;
                }
                order.StatusId = 6;
                int result = db.SaveChanges();
                if(result >0) return Json(new { success = true, message = "Đã hoàn thành đơn hàng của " + order.ReceiveName }, JsonRequestBehavior.AllowGet);
                else return Json(new { success = true, message = "Không hoàn tất được đơn hàng của " + order.ReceiveName }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                Json(new { success = true, message = "Không hoàn tất được đơn hàng của " + order.ReceiveName }, JsonRequestBehavior.AllowGet);
            }
            return null ;
        }
        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name");
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,StatusId,Coupon,Discount,ExtraDiscount,TotalAmount,TransportId,PaymentMethodId,ReceiveName,ReceiveAddress,ReceivePhone,Note,OrderDate,RequireDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", order.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId", order.TransportId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", order.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId", order.TransportId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,StatusId,Coupon,Discount,ExtraDiscount,TotalAmount,TransportId,PaymentMethodId,ReceiveName,ReceiveAddress,ReceivePhone,Note,OrderDate,RequireDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", order.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId", order.TransportId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            //Remove order detail
            db.OrderDetails.RemoveRange(order.OrderDetails);
            db.Orders.Remove(order);
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
