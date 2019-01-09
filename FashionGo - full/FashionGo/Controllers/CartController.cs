using System.Linq;
using System.Web.Mvc;
using FashionGo.Models.Entities;
using FashionGo.Models;
using System.Collections.Generic;

namespace FashionGo.Controllers
{
    public class CartController : BaseController
    {
        List<Size> listSize() {
            List<Size> list= new List<Size>();
            list.Add(new Size("S", "S"));
            list.Add(new Size("M", "M"));
            list.Add(new Size("L", "L"));
            list.Add(new Size("XL", "XL"));
            return list;
        }
        
        
        public ActionResult Index()
        {
            if (ShoppingCart.Cart.Items.Count == 0)
            {
                Warning(string.Format("<b><h5>{0}</h4></b>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                return RedirectToAction("Index", "Home");
            }
           
            var cart = ShoppingCart.Cart;
            double tatol = 0;
            foreach (var item in ShoppingCart.Cart.Items)
            {
                tatol += (item.PriceAfter==null?item.Price.Value: item.PriceAfter.Value) * item.Tong.Value;
            }
            ViewBag.tatol = tatol;
            return View(cart.Items);
            
        }
        public ActionResult OrderDetail()
        { 
            var cart = ShoppingCart.Cart;
            return PartialView("Partials/_OrderDetail", cart.Items);
        }
        

        public ActionResult _PartialCart()
        {
            var cart = ShoppingCart.Cart;
            return PartialView(cart.Items);
        }

        public ActionResult Add(int id, int quatity)
        {
            var cart = ShoppingCart.Cart;
            //if(type== null)
            //    cart.Add(id, quatity);
            //else
                cart.Add(id, quatity);

            var info = new { Count = cart.Count, Total = cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            var cart = ShoppingCart.Cart;
            cart.Remove(id);

            var info = new { Count = cart.Count, Total = cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int id, int quantity, string slS, string slM, string slL, string slXL)
        {
            var cart = ShoppingCart.Cart;
            int S = slS== null?0: int.Parse(slS);
            int M = slM == null ? 0 : int.Parse(slM);
            int L = slL == null ? 0 : int.Parse(slL);
            int XL = slXL == null ? 0 : int.Parse(slXL);
          //  if (slS != null || slM != null|| slL != null|| slXL != null)
                cart.Update(id, S,M,L, XL);
           //else
           //     cart.Update(id, quantity);

            var p = cart.Items.Single(i => i.Id == id);
            var info = new
            {
                Count = cart.Count,
                Total = cart.Total,
                quantity=quantity,
                // Amount = p.PriceAfter.Value * p.Amount
                S = p.PriceAfter.Value * p.S,
                M = p.PriceAfter.Value * p.M,
                L = p.PriceAfter.Value * p.L ,
                XL = p.PriceAfter.Value * p.XL,
                tong =( S+M+L+XL),
                PriceAfter = p.PriceAfter
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult UpdateList(int id, int quantity, string type)
        //{
        //    var cart = ShoppingCart.Cart;
        //    if (type != null) cart.Update(id, quantity, type);
        //    else
        //        cart.Update(id, quantity);

        //    var p = cart.Items.Single(i => i.Id == id);
        //    var info = new
        //    {
        //        Count = cart.Count,
        //        Total = cart.Total,
        //        quantity = quantity,
        //        // Amount = p.PriceAfter.Value * p.Amount
        //        S = p.PriceAfter.Value * p.S,
        //        M = p.PriceAfter.Value * p.M,
        //        L = p.PriceAfter.Value * p.L,
        //        XL = p.PriceAfter.Value * p.XL,
        //    };
        //    return Json(info, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Clear()
        {
            
            var cart = ShoppingCart.Cart;
            cart.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult _MiniCart()
        {
            return PartialView();
        }
    }
    public class Size
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Size(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }
    }
   
}