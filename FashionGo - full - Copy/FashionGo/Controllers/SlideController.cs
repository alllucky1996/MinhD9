using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionGo.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Slide
        public ActionResult Index()
        {
            ViewBag.Nhung = "";// "<iframe src="https://onedrive.live.com/embed?resid=363FC342758AB5C9%211232&amp;authkey=%21AAuNkJbCdAxjKVQ&amp;em=2&amp;wdAr=1.3333333333333333" width="1026px" height="793px" frameborder="0">Đây là một tài liệu <a target="_blank" href="https://office.com">Microsoft Office</a> đã nhúng, được cung cấp bởi <a target="_blank" href="https://office.com/webapps">Office Online</a>.</iframe>";
            return View();
        }
    }
}