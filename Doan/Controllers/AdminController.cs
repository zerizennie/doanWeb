using Doan.DAL;
using Doan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Categories()
        {
            var categories = GetCategories();
            return View(categories);
        }

        private List<catetory> GetCategories()
        {
            using (var unitOfWork = new GenericUnitOfWork())
            {
                return unitOfWork.GetRepositoryInstance<catetory>().GetAllRecordsIQueryable().Where(i => i.isDelete == false).ToList();
            }
        }
    }
}