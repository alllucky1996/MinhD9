using FashionGo.Models;
using FashionGo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FashionGo.Controllers
{
    public class SoSanhController : BaseController
    {
       
        // GET: SoSanh
        public ActionResult Index()
        {
            var model = SoSanhSp.ListSp;
            //for (int i=0; i<2;i++)
            //{
            //    if (model[i] == null)
            //        model[i] = new Product();
            //}

            return View(SoSanhSp);
        }
        public async Task<ActionResult> Add(string id, string id2)
        {
            try
            {
                var sp1 = await db.Products.FindAsync(id);
                var sp2 = await db.Products.FindAsync(id2);
                SoSanhSp.Add(sp1, sp2);
                return Json(new ExecuteResult() { IsOk = true });
            }
            catch (Exception ex)
            {
                return Json(new ExecuteResult() { IsOk = true, Data = ex.Message }); 
            }
            
        }
        public async Task<ActionResult> AddOne(int id)
        {
            try
            {
                var sp1 = await db.Products.FindAsync(id);
                if(SoSanhSp.sp1== null)
                    SoSanhSp.Add(sp1, 0);
                else
                    SoSanhSp.Add(sp1, 1);
                return Json(new ExecuteResult() { IsOk = true, Data= sp1.Name}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ExecuteResult() { IsOk = true, Mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}