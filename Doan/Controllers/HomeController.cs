using Doan.DAL;
using Doan.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Doan.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string name, string email, string phone, string subject, string content)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
            {
                ViewBag.ErrorMessage = "Vui lòng điền đầy đủ thông tin.";
                return View("Contact");
            }

            if (!IsValidEmail(email))
            {
                ViewBag.ErrorMessage = "Định dạng email không hợp lệ.";
                return View("Contact");
            }

            if (!IsValidPhoneNumber(phone))
            {
                ViewBag.ErrorMessage = "Định dạng số điện thoại không hợp lệ.";
                return View("Contact");
            }

            try
            {
                // Ghi thông tin liên hệ vào tệp tin contact.txt
                string contactData = $"Name: {name}\nEmail: {email}\nPhone: {phone}\nSubject: {subject}\nContent: {content}\n";
                string filePath = Server.MapPath("~/App_Data/contact.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(contactData);
                }

                ViewBag.SuccessMessage = "Chúng tôi đã tiếp nhận yêu cầu!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Có lỗi xảy ra: {ex.Message}";
            }

            return View("Contact");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }


        public ActionResult Product(int? id, int? page, int? pagesize)
        {
            ViewBag.Message = "Your product page.";

            return View();
        }
        
        //Trả về Danh sách Category
        public ActionResult MenuProduct()
        {
            var items = db.catetories.ToList();
            var categoryDetails = items.Select(c => new categoryDetail { catetory_id = c.catetory_id, catetory_name = c.catetory_name }).ToList();
            return PartialView("_MenuProduct", categoryDetails);
        }

        //Hiển thị SP kèm phân trang
        public ActionResult HienThiSP(int? id, int? page, int? pageSize)
        {
            var items = db.products.ToList();
            // Lọc sản phẩm theo categoryId chỉ định
            if (id != null)
            {
                items = items.Where(x => x.catetory_id == id).ToList();
            }
            var _pageSize = pageSize ?? 6;//không truyền số sp trên trang thì mặc định 6
            var pageIndex = page ?? 1;//không truyền số phân trang thì mặc định trang đầu tiên
            var totalItems = items.Count;
            var numberPages = Math.Ceiling((double)totalItems /_pageSize);

            var pagedProducts = items.Skip((pageIndex - 1) * _pageSize).Take(_pageSize)
                .Select(p => new productDetail
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_price = p.product_price,
                    product_image = p.product_image,
                    product_ingredients = p.product_ingredients,
                    product_description = p.product_description,
                    catetory_id = p.catetory_id,
                    quantity = p.quantity,
                    end_date = p.end_date,
                    start_date = p.start_date
                }).ToList();

            var result = new GetProducViewModel
            {
                Data = pagedProducts,
                TotalItems = totalItems,
                CurrentPage = pageIndex,
                NumberPages = numberPages,
                PageSize = _pageSize
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Review()
        {
            ViewBag.Message = "Your review page.";

            return View();
        }

        sosEntities00 db = new sosEntities00();
        

        //GET: Register

        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(customer _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.customers.FirstOrDefault(s => s.email == _user.email);
                if (check == null)
                {
                    _user.password = GetMD5(_user.password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.customers.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }

        private static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = db.customers.Where(s => s.email.Equals(email) && s.password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().first_name + " " + data.FirstOrDefault().last_name;
                    Session["Email"] = data.FirstOrDefault().email;
                    Session["idUser"] = data.FirstOrDefault().user_id;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
    }
}