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
        sosEntities00 db = new sosEntities00();
        // GET: Order
        public ActionResult Index()
        {
            return View(db.bills.OrderByDescending(x => x.order_id).ToList());
        }
        public ActionResult Details(int order_id)
        {
            bill order = db.bills.Find(order_id);
            var order_details = db.order_detail_id.Where(x => x.order_id == order_id).ToList();
            var tuple = new Tuple<bill, IEnumerable<order_detail_id>>(order, order_details);

            double? SumAmount = order.total;
            ViewBag.TotalItems = order_details.Sum(x => x.quantity);
            ViewBag.Discount = 0;
            ViewBag.TAmount = SumAmount - 0;
            ViewBag.Amount = SumAmount;
            return View(tuple);
        }
    }
}