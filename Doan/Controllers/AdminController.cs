using Doan.DAL;
using Doan.Models;
using Doan.Repository;
using Newtonsoft.Json;
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

        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }

        public ActionResult UpdateCategory(int catetory_id)
        {
            categoryDetail cd;
                if (catetory_id != null)
            {
                cd = JsonConvert.DeserializeObject<categoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<catetory>().GetFirstorDefault(catetory_id)));
            }
            else
            {
                {
                    cd= new categoryDetail();

                }
                return View("UpdateCategory", cd);     
            }
        }
    }
}