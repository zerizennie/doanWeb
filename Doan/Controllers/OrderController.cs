using Doan.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
            //tạo tuple chứa hóa đơn (bill) và danh sách các chi tiết đơn hàng (order_detail_id).
            //tính toán một số thông tin thống kê như tổng số lượng mục và tổng tiền -> view
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

        //Get Edit
        [HttpGet]
        public ActionResult Edit(int order_id)
        {
            bill order = db.bills.Single(x => x.order_id == order_id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View("Edit", order);
        }

        //Post Edit
        [HttpPost]
        public ActionResult Edit(bill order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Order");
            }

            return View(order);
        }

    }
}