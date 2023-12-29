using Doan.DAL;
using Doan.Models;
using Doan.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<catetory>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.catetory_id.ToString(), Text = item.catetory_name });
            }    
            return list;
        }
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
            if (catetory_id != 0)
            { 
                cd = JsonConvert.DeserializeObject<categoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<catetory>().GetFirstorDefault(catetory_id)));
            }
            else
            {
                cd = new categoryDetail();
            }
            return View("UpdateCategory", cd);
        }
        public ActionResult CategoryEdit(int category_id)
        {
            return View(_unitOfWork.GetRepositoryInstance<catetory>().GetFirstorDefault(category_id));
        }
        [HttpPost]
        public ActionResult CategoryEdit(catetory tbCategory)
        {
            _unitOfWork.GetRepositoryInstance<catetory>().Update(tbCategory);
            return RedirectToAction("Categories");
        }
        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<product>().GetProduct());
        }
        public ActionResult ProductEdit(int product_id)
        {
            return View(_unitOfWork.GetRepositoryInstance<product>().GetFirstorDefault(product_id));
        }
        [HttpPost]
        public ActionResult ProductEdit(product tbProduct, HttpPostedFileBase file)
        {
            string img = null;
            if (file != null)
            {
                img = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), img);
                //upload ảnh
                file.SaveAs(path);
            }
            tbProduct.product_image = file != null ? img : tbProduct.product_image;
            _unitOfWork.GetRepositoryInstance<product>().Update(tbProduct);
            return RedirectToAction("Product");
        }
        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        [HttpPost]
        public ActionResult ProductAdd(product tbProduct, HttpPostedFileBase file)
        {
            string img = null;
            if (file != null)
            {
                img = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), img);
                //upload ảnh
                file.SaveAs(path);
            }
            tbProduct.product_image = img;
            _unitOfWork.GetRepositoryInstance<product>().Add(tbProduct);
            return RedirectToAction("Product");
        }
        
    }
}