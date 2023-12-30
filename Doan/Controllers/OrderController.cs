using Doan.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan.Controllers
{
    public class OrderController : Controller
    {
        huhuEntities db = new huhuEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View(db.bills.OrderByDescending(x => x.order_id).ToList());
        }

        public ActionResult Details1(int order_id, order_detail_id x, int? order_id1)
        {
            bill order = db.bills.Find(order_id);
            var order_details = db.order_detail_id.Where(y => order_id1 == order_id).ToList();
            var tuple = new Tuple<bill, IEnumerable<order_detail_id>>(order, order_details);

            double? SumAmount = order.total;
            ViewBag.TotalItems = order_details.Sum(z => z.quantity);
            ViewBag.Discount = 0;
            ViewBag.TAmount = SumAmount - 0;
            ViewBag.Amount = SumAmount;
            return View(tuple);
        }

        public ActionResult Details(int order_id, order_detail_id x)
        {
            return Details1(order_id, x, x.order_id1);
        }

        

        //public ActionResult Details(int order_id, int? order_id1)
        //{
        //    bill order = db.bills.Find(order_id);
        //    var order_details = db.order_detail_id.Where(y => y.order_id1 == order_id).ToList();

        //    ViewBag.TotalItems = order_details.Sum(x => x.quantity);

        //    double? SumAmount = order_details.Sum(x => x.product_price);
        //    ViewBag.Amount = SumAmount;

        //    ViewBag.Discount = 0;
        //    ViewBag.TAmount = SumAmount - 0;

        //    // Tạo Tuple để truyền dữ liệu đến view
        //    var tuple = new Tuple<bill, IEnumerable<order_detail_id>>(order, order_details);

        //    return View(tuple);
        //}

    }
}