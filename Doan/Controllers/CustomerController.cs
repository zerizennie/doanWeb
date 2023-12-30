using Doan.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan.Controllers
{
    public class CustomerController : Controller
    {
        huhuEntities db = new huhuEntities();

        // GET: Customer

        public ActionResult Index()
        {
            return View(db.customers.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(customer customer)
        {
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            return View();
        }

        //Get Edit
        [HttpGet]
        public ActionResult Edit(int user_id)
        {
            customer customer = db.customers.Single(x => x.user_id == user_id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View("Edit", customer);
        }

        //Post Edit
        [HttpPost]
        public ActionResult Edit(customer cust)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }

            return View(cust);
        }

        //Get Details
        //public ActionResult Details(int id)
        //{
        //    customer cust = customer.Find(id);
        //    if (cust == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cust);
        //}

        //Get Delete
        public ActionResult Delete(int user_id)
        {
            customer cust = db.customers.Find(user_id);
            if (cust == null)
            {
                return HttpNotFound();
            }
            return View(cust);

        }

        //Post Delete Confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int user_id)
        {
            customer cust = db.customers.Find(user_id);
            db.customers.Remove(cust);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}