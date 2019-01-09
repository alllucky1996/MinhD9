namespace FashionGo.Models
{
    using FashionGo.Models.Entities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;

    public class ShoppingCart
    {


        // Chứa các mặt hàng đã chọn
        public List<Product> Items = new List<Product>();

        public int Count { get; set; }

        public double Total { get; set; }

        public Coupon Coupon { get; set; }

        public Transport Transport { get; set; }



        // Lấy giỏ hàng từ Session
        public static ShoppingCart Cart
        {
            get
            {
                var cart = HttpContext.Current.Session["Cart"] as ShoppingCart;
                // Nếu chưa có giỏ hàng trong session -> tạo mới và lưu vào session
                if (cart == null)
                {
                    cart = new ShoppingCart();
                    cart.Count = 0;
                    cart.Total = 0;
                    HttpContext.Current.Session["Cart"] = cart;
                }
                return cart;
            }
        }

        public string CouponCode
        {
            get
            {
                if (Coupon != null)
                {
                    return Coupon.Code;
                }
                return "";
            }
        }

        public double Discount
        {
            get
            {
                var db = new ApplicationDbContext();
                if (Coupon == null)
                {
                    return 0;
                }

                switch (Coupon.DiscountFor)
                {
                    case DiscountObject.Product:
                        if (Coupon.DiscountForId.HasValue)
                        {
                            var product = db.Products.Find(Coupon.DiscountForId);
                            if (Items.Contains(product))
                            {
                                return (double)product.PriceAfter * Coupon.Discount / 100;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                        break;
                    case DiscountObject.Order:
                        return (double)Total * Coupon.Discount / 100;
                    case DiscountObject.Transport:
                        return (double)Transport.Cost * Coupon.Discount / 100;

                }
                db.Dispose();
                return 0;
            }
        }

        public string discountDescription
        {
            get
            {
                if (Coupon == null)
                {
                    return "";
                }

                return "<br><small>(" + Coupon.Name + ")</small";
            }
        }

        public double OrderTotal
        {
            get
            {
                return Total + TransportCost - Discount;
            }
        }


        public double TransportCost
        {
            get
            {
                var cost = Transport == null ? 0 : Transport.Cost.Value;
                //Nếu số tiền lớn hơn 300k hoặc số lượng mua từ 3 sản phẩm trở lên thì miễn phí vận chuyển
                if (Total > 300000 || Count > 2)
                {
                    // cost = cost - 15000;
                    cost = 0;
                }

                return cost < 0 ? 0 : cost;
            }
        }


        public void Add(int id, int soluong, string type = null)
        {
            var db = new ApplicationDbContext();
            Product item = null;
            try
            {
                item = Items.Single(i => i.Id == id);
            }
            catch (System.Exception ex)
            {
                item = db.Products.Find(id);
                Debug.WriteLine(ex.Message);
            }
            item.M = 1;
            item.S = item.L = item.XL = 0;
            Items.Add(item);
            double t = 0;int c = 0;
            foreach (var i in Items)
            {
                t += i.PriceAfter.Value * (item.Tong == null ? 0 : item.Tong.Value);
                c += (item.Tong == null ? 0 : item.Tong.Value);
            }
            Total = t;Count = c;

            db.Dispose();
            if (true)
            {
                //if (type== null)
                //{

                //try // tìm thấy trong giỏ -> tăng số lượng lên 1
                //{
                //    item = Items.Single(i => i.Id == id);
                //    // item.Amount = item.Amount + soluong;
                //    item.M += soluong;
                //}
                //catch // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
                //{

                //    item = db.Products.Find(id);
                //    // item.Amount = soluong;
                //    item.M += soluong;

                //    Items.Add(item);
                //}

                //  }
                // thực chất k chạy vào đây
                //else
                //{

                //    try // tìm thấy trong giỏ -> tăng số lượng lên 1
                //    {
                //        item = Items.Single(i => i.Id == id);
                //        //  item.Amount = item.Amount + soluong;
                //        if (type.ToLower() == "S") item.S = item.S + soluong;
                //        else if (type.ToLower() == "M") item.M = item.M + soluong;
                //        else if (type.ToLower() == "L") item.L = item.L + soluong;
                //        else if (type.ToLower() == "XL") item.XL = item.XL + soluong;
                //    }
                //    catch // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
                //    {

                //        item = db.Products.Find(id);
                //        //item.Amount = soluong;
                //        if (type.ToLower() == "S") item.S = item.S + soluong;
                //        else if (type.ToLower() == "M") item.M = item.M + soluong;
                //        else if (type.ToLower() == "L") item.L = item.L + soluong;
                //        else if (type.ToLower() == "XL") item.XL = item.XL + soluong;

                //        Items.Add(item);
                //    }
                //    Total += item.PriceAfter.Value * soluong;
                //    Count += soluong;
                //    db.Dispose();
                //}
            }
        }
        //public void Add(int id, int soluong, string type)
        //{
        //    var db = new ApplicationDbContext();
        //    Product item = null;
        //    try // tìm thấy trong giỏ -> tăng số lượng lên 1
        //    {
        //        item = Items.Single(i => i.Id == id);
        //        //  item.Amount = item.Amount + soluong;
        //        if (type.ToLower() == "S") item.S = item.S + soluong;
        //        else if (type.ToLower() == "M") item.M = item.M + soluong;
        //        else if (type.ToLower() == "L") item.L = item.L + soluong;
        //        else if (type.ToLower() == "XL") item.XL = item.XL + soluong;
        //    }
        //    catch // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
        //    {

        //        item = db.Products.Find(id);
        //        //item.Amount = soluong;
        //        if (type.ToLower() == "S") item.S = item.S + soluong;
        //        else if (type.ToLower() == "M") item.M = item.M + soluong;
        //        else if (type.ToLower() == "L") item.L = item.L + soluong;
        //        else if (type.ToLower() == "XL") item.XL = item.XL + soluong;

        //        Items.Add(item);
        //    }
        //    Total += item.PriceAfter.Value * soluong;
        //    Count += soluong;
        //    db.Dispose();
        //}
        /// <summary>
        /// Lỗi ở dây
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            try
            {
                var item = Items.Single(i => i.Id == id);
                Total -= item.PriceAfter.Value * (item.Tong == null ? 0 : item.Tong.Value);
                Count -= (item.Tong == null ? 0 : item.Tong.Value);
                Items.Remove(item);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Có đâu mà xóa");
                Debug.WriteLine(ex.Message);
            }
        }

        public void Update(int id, int newQuantity)
        {
            var item = Items.Single(i => i.Id == id);
            Total += item.PriceAfter.Value * (newQuantity - item.Amount.Value);
            Count += newQuantity - item.Amount.Value;
            item.Amount = newQuantity;
        }
        public void Update(int id,int slS, int slM, int slL, int slXL)
        {
           
            var item = Items.Single(i => i.Id == id);
            item.S = slS;
            item.M = slM;
            item.L = slL;
            item.XL = slXL;
            // if (slS>0)
            // {
            //     var xxx = (slS - item.S == null ? 0 : item.S.Value);
            ////     Total += item.PriceAfter.Value * xxx;
            ////     Count += (slS - item.S==null?0: item.S.Value);
            //     item.S = slS;
            // }
            // if (slM == 1)
            // {
            // //    Total = item.PriceAfter.Value ;
            //     Count += 1;
            //     item.M = slM;
            // }
            // if (slM>1)
            // {
            //     var xxx = (slM - item.M == null ? 0 : item.M.Value);
            //  //   Total += item.PriceAfter.Value * xxx;
            // //    Count +=  (slM - item.M == null ? 0 : item.M.Value);
            //     item.M = slM;
            // }
            // if (slL>0)
            // {
            //     var xxx = (slL - item.L == null ? 0 : item.L.Value);
            //  //   Total += item.PriceAfter.Value * xxx;
            ////     Count += slL - item.L== null?0: item.L.Value;
            //     item.L = slL;
            // }
            // if (slXL>0)
            // {
            //     var xxx = (slXL - (item.XL == null ? 0 : item.XL.Value));
            //   //  Total += item.PriceAfter.Value * xxx;
            //   //  Count += slXL - item.XL== null?0: item.XL.Value;
            //     item.XL = slXL;
            // }
            // Total += item.PriceAfter.Value * (item.Tong == null ? 0 : item.Tong.Value);
            double t = 0; int c = 0;
            foreach (var i in Items)
            {
                t += i.PriceAfter.Value * (item.Tong == null ? 0 : item.Tong.Value);
                c += (item.Tong == null ? 0 : item.Tong.Value);
            }
            Total = t; Count = c;


        }

        public void UpdateCoupon(Coupon coupon)
        {
            Coupon = coupon;
        }

        public void UpdateTransport(Transport transport)
        {
            Transport = transport;
        }

        public int getQuantity(int id)
        {
            var item = Items.Single(i => i.Id == id);
            return item.Amount.Value;
        }
        public int getCurentQuantity(int id)
        {
            var item = Items.Single(i => i.Id == id);
            return item.S.Value + item.M.Value + item.L.Value + item.XL.Value;
        }

        public void Clear()
        {
            Count = 0;
            Total = 0;
            Items.Clear();
        }

    }
}
