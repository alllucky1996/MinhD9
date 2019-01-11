using FashionGo.Models.Entities;
using FashionGo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Commons.Libs;
using System.Diagnostics;

namespace FashionGo.Controllers
{
    public class OrderController : BaseController
    {

        public ShoppingCart cart = ShoppingCart.Cart;
        string em = "";
        [HttpGet]
        public ActionResult Checkout()
        {
            if (cart.Count == 0)
            {
                Warning(string.Format("<b><h5>{0}</h4></b>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull");
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "-1").Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull");

            var model = new Order();

            if (ModelState.IsValid && Request.IsAuthenticated)
            {
                model.UserId = User.Identity.GetUserId();
                var user = db.Users.Find(User.Identity.GetUserId());
                model.ReceiveName = user.FullName;
                model.ReceivePhone = user.PhoneNumber;
                model.ReceiveAddress = user.Address;

                ViewBag.Email = user.UserName;

                if (user.District != null)//second
                {
                    ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " +  x.Name }), "ProvinceId", "NameFull", user.District.ProvinceId);
                    ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == user.District.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", user.DistrictId);
                }
            }

            return View(model);
        }
        

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Checkout(Order model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var sms = new SpeedSMSAPI();
                //Validate Cart
                if (cart.Count == 0)
                {
                    Warning(string.Format("<h5>{0}</h4>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                    return RedirectToAction("Index", "Home");
                }
               

                var user = db.Users.Find(model.UserId);
                 em = form["Email"];
                var us = db.Users.Where(o => o.Email == em).FirstOrDefault();
                //Kiểm tra nếu là người dùng mới thì tạo tài khoản
                if ( us== null)
                {
                    if (true)
                    {
                        //var password = Xstring.GeneratePassword();
                        //var newUser = new ApplicationUser
                        //{
                        //    UserName = form["Email"],
                        //    Email = form["Email"],
                        //    PhoneNumber = model.ReceivePhone,
                        //    PasswordHash = password,
                        //};

                        //var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        //var result = await UserManager.CreateAsync(newUser, password);

                        //if (result.Succeeded)
                        //{
                        //    var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                        //    await SignInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);
                        //    model.UserId = newUser.Id;

                        //    //Gửi sms
                        //    string smsAcc = "FashionGo: Tai khoan quan ly don hang cua ban tren FashionGo la: Email:" + form["Email"] + ", mat khau:" + password;
                        //    string sent = sms.sendSMS(model.ReceivePhone, smsAcc, 2, "");

                        //    //Gửi tin nhắn tài khoản cho người dùng.
                        //    var subject = "Tài khoản quản lý đơn hàng tại ZdealVN.!";
                        //    var msg = "Xin chào, " + model.ReceiveName;
                        //    msg += "<br>Tài khoản quản lý đơn hàng của bạn tại <a href='http://FashionGo.vn'>FashionGo.vn</a> là:";
                        //    msg += "<br>-Tên đăng nhập: " + form["Email"];
                        //    msg += "<br>-Mật khẩu của bạn: " + password;
                        //    msg += "<br>Bạn có thể sử dụng tài khoản này đăng nhập trên FashionGo.vn để quản lý đơn hàng và sử dụng các dịch vụ khác do FashionGo cung cấp.!";
                        //    msg += "<br>Cảm ơn bạn đã quan tâm sử dụng dịch vụ của FashionGo. mọi thắc mắc xin liên hệ hotline: 0901.002.822-0965.002.822.";
                        //    msg += "<br>FashionGo Hân hạnh được phục vụ bạn.";
                        //    msg += "<br>Chúc bạn một ngày tốt lành.";
                        //    msg += "<p></p><p></p>-BQT ZdealVN!.</p>";

                        //    XMail.Send(newUser.Email, subject, msg);
                        //}
                        //else
                        //{
                        //    foreach (var error in result.Errors)
                        //    {
                        //        ModelState.AddModelError("", "-" + error);
                        //    }
                        //}
                    }
                    var password = "ispasslakhoitao";
                    var newUser = new ApplicationUser
                    {
                        UserName = form["Email"],
                        Email = form["Email"],
                        PhoneNumber = model.ReceivePhone,
                        PasswordHash = password,
                    };

                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    var result = await UserManager.CreateAsync(newUser, password);

                    if (result.Succeeded)
                    {
                        var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                        await SignInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);
                        model.UserId = newUser.Id;

                        //Gửi sms
                        string smsAcc = "FashionGo: Tai khoan quan ly don hang cua ban tren FashionGo la: Email:" + form["Email"] + ", mat khau:" + password;
                        string sent = sms.sendSMS(model.ReceivePhone, smsAcc, 2, "");

                        //Gửi tin nhắn tài khoản cho người dùng.
                        var subject = "Tài khoản quản lý đơn hàng tại ZdealVN.!";
                        var msg = "Xin chào, " + model.ReceiveName;
                        msg += "<br>Tài khoản quản lý đơn hàng của bạn tại <a href='http://FashionGo.vn'>FashionGo.vn</a> là:";
                        msg += "<br>-Tên đăng nhập: " + form["Email"];
                        msg += "<br>-Mật khẩu của bạn: " + password;
                        msg += "<br>Bạn có thể sử dụng tài khoản này đăng nhập trên FashionGo.vn để quản lý đơn hàng và sử dụng các dịch vụ khác do FashionGo cung cấp.!";
                        msg += "<br>Cảm ơn bạn đã quan tâm sử dụng dịch vụ của FashionGo. mọi thắc mắc xin liên hệ hotline: 0901.002.822-0965.002.822.";
                        msg += "<br>FashionGo Hân hạnh được phục vụ bạn.";
                        msg += "<br>Chúc bạn một ngày tốt lành.";
                        msg += "<p></p><p></p>-BQT ZdealVN!.</p>";

                        //bool xxx= XMail.Sended(newUser.Email, subject, msg);
                       int xxx= vnMail.GuiMail(subject, msg, em, "dungitfa@gmail.com", "");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", "-" + error);
                        }
                    }
                    ViewBag.Email = form["Email"];
                }
                // ngược lại thì  gán order cho user tìm được
                else if (us != null)
                {
                    model.UserId = us.Id;
                }

                //Update order info 
                model.TotalAmount = cart.Total;
                model.TotalOrder = cart.OrderTotal;
                if (cart.Transport != null) { model.TransportId = cart.Transport.Id; }
                model.Coupon = cart.CouponCode;
                model.Discount = cart.Discount;
                model.OrderDate = DateTime.Now;
                model.StatusId = 1;
                // add
                db.Orders.Add(model);
                try
                {
                    foreach (var p in cart.Items)
                    {
                        var d = new OrderDetail
                        {
                            OrderId = model.Id,
                            ProductId = p.Id,
                            PriceAfter = p.PriceAfter.Value,
                            Discount = p.Discount == null ? 0 : p.Discount.Value,
                            // Amount = p.Amount
                            S = p.S,
                            M = p.M,
                            L = p.L,
                            XL = p.XL
                        };
                        //ViewBag.ProductDetail = cart.Items;
                        db.OrderDetails.Add(d);
                        // update thêm đang chờ bán
                        var pending = db.Products.Find(p.Id);
                        pending.Pending = (pending.Pending == null ? pending.Pending = 1 : pending.Pending += 1);
                    }
                    if (db.SaveChanges() > 0)
                    {
                        

                        Success(string.Format("<b><h5>{0}</h4></b>", "Đặt hàng thành công, chúng tôi sẽ liên hệ lại với bạn để xác nhận đơn hàng trước khi tiến hành giao hàng."), true);

                        //Gửi SMS/Mail xác nhận và báo tin cho Sale

                        var customerMsg = "FashionGo: Dat hang thanh cong don hang:#" + model.Id + ", Voi so tien: " + string.Format("{0:0,0}vnđ", model.TotalAmount);
                        var saleSMS = "FashionGo: Don hang moi #" + model.Id + " tu KH: " + model.ReceiveName + " - " + model.ReceivePhone;
                        //string response = sms.sendSMS(model.ReceivePhone, customerMsg, 2, "");
                        //response = sms.sendSMS("0327835923", saleSMS, 2, "");
                        string subject = "Đơn hàng ";
                        var msg = "Fashion kính chào quý khách: " + model.ReceiveName;
                        msg += "<br>quý khách đã đặt hàng thành công đơn hàng: " + model.Id + ", với số tiền: " + string.Format("{0:0,0}vnđ", cart.Total);
                        msg += "<br>Chi tiết:";
                        foreach (var item in cart.Items)
                        {
                            msg += "<br>Sản phẩm: " + item.Name;
                            msg += "<br>Số lượng: " + item.GetSoLuong;
                           
                        }
                        msg += "<br>Chú thích: " + model.Note;
                        msg += "<br>Cảm ơn bạn đã quan tâm sử dụng dịch vụ của FashionGo. mọi thắc mắc xin liên hệ hotline: 0978 132 474-032 783 5923";
                        msg += "<br>Hân hạnh được phục vụ quý khách.";
                        msg += "<br>Chúc quý khách một ngày tốt lành.";
                        msg += "<p></p><p></p>-BQT NguyenAnhDung!.</p>";
                        msg += "<p></p><p></p>-SDT: 032 783 5923.</p>";
                        int ok = vnMail.GuiMail(subject, msg, em, "dungitfa@gmail.com", "");
                        
                        cart.Clear();
                        return RedirectToAction("Detail", new { id = model.Id });
                    }
                }
                catch (Exception ex)
                {
                    //  Danger(string.Format("-{0}<br>", ex.Message), true);
                    ModelState.AddModelError("", ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                // Validate Email
                if (!Request.IsAuthenticated && String.IsNullOrEmpty(form["Email"]))
                    ModelState.AddModelError("", "-Bạn chưa nhập email nhận đơn hàng!");

                //Check quận huyện
                if (String.IsNullOrEmpty(form["DistrictId"]))
                    ModelState.AddModelError("", "-Bạn chưa chọn quận huyện nơi chuyển hàng tới!");

                //Check phuong thuc van chuyen
                if (String.IsNullOrEmpty(form["TransportId"]))
                    ModelState.AddModelError("", "-Bạn chưa chọn nhà vận chuyển trước khi đặt hàng!");
             
            }
            var provinceId = form["ProvinceId"];
            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", provinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == provinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", form["DistrictId"].ToString());

            return View(model);
        }



        public ActionResult Detail(int id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Total = order.StatusId;
            ViewBag.Email = em;
            bool free = false;
            if (cart.Total > 300000 || order.OrderDetails.Count > 2)
                free = true;
            ViewBag.FreeShip = free;
            return View(order);
        }

        public ActionResult List()
        {
            string currentUserId = User.Identity.GetUserId();
            var orders = db.Orders.Where(o => o.UserId == currentUserId).ToList();
            return View(orders);
        }

        
        public bool UpdateTransport(int transportId)
        {
            var transport = db.Transports.Find(transportId);
            if (transport == null)
            {
                return false;
            }
            
            //Update cart Transport
            cart.UpdateTransport(transport);

            return true;
        }

        
        public bool UpdateCoupon(string code)
        {
            var coupon = db.Coupons.Find(code);
            if (coupon == null)
            {
                return false;
            }
            //Update cart Transport
            cart.UpdateCoupon(coupon);

            return true;
        }

        
        public ActionResult AjaxGetTransport(string districtId)
        {
            var transports = db.Transports.Where(t => t.DistrictId == districtId).ToList();
            if (transports.Count() > 0)
            {
                UpdateTransport(transports.First().Id);
            }
            return PartialView(transports);
        }

        [HttpPost]
        
        public ActionResult AjaxUpdateCoupon(string couponCode)
        {
            var info = new
            {
                Status = 0,
                Msg = "Coupon không tồn tại hoặc đã hết hạn dùng.!"
            };

            if (UpdateCoupon(couponCode))
            {
                info = new
                {
                    Status = 1,
                    Msg = "Update thành công!"
                };
            }

            return Json(info, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult getOrderInfo()
        {
            var info = new
            {
                TransportCost = cart.TransportCost,
                Discount = cart.Discount,
                DiscountDescription = cart.discountDescription,
                OrderTotal = cart.OrderTotal
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}