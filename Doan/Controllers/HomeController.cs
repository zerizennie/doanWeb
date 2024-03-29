﻿using Doan.DAL;
using Doan.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
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


        //Danh sách Category
        public ActionResult MenuProduct()
        {
            var items = db.catetories.ToList();
            var categoryDetails = items.Select(c => new categoryDetail { catetory_id = c.catetory_id, catetory_name = c.catetory_name }).ToList();
            return PartialView("_MenuProduct", categoryDetails);
        }

        //Tải sp
        public ActionResult LoadProduct(int? id)
        {
            var items = db.products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.catetory_id == id).ToList();
            }
            var productDetails = items.Select(p => new productDetail { product_id = p.product_id, product_name = p.product_name, product_price = p.product_price, product_image = p.product_image,product_ingredients=p.product_ingredients, catetory_id = p.catetory_id }).ToList();
            return PartialView("_LoadProduct",productDetails);
        }

        // Lọc sản phẩm theo categoryId chỉ định
        public ActionResult CateProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Product"); // Gọi và trả về Action "Product"
            }

            var items = db.products.ToList();
            if (id >= 0)
            {
                items = items.Where(x => x.catetory_id == id).ToList();
            }
            var productDetails = items.Select(p => new productDetail { product_id = p.product_id, product_name = p.product_name, product_price = p.product_price, product_image = p.product_image, product_ingredients = p.product_ingredients, catetory_id = p.catetory_id }).ToList();
            return View(productDetails);
        }


        //Hiển thị DS cho trang index
        public ActionResult HienThiSP(int? id)
        {
            var items = db.products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.catetory_id == id).ToList();
            }
            var productDetails = items.Select(p => new productDetail { product_id = p.product_id, product_name = p.product_name, product_price = p.product_price, product_image = p.product_image, product_ingredients = p.product_ingredients, catetory_id = p.catetory_id }).ToList();
            return PartialView("_HienThiSP", productDetails);
        }
            //Hiển thị chi tiết sp
            public ActionResult ChitietSP(int id)
        {
            var items = db.products.AsQueryable();
            if (id >= 0)
            {
                items = items.Where(x => x.product_id == id);
            }
            var infoProduct = items.Select(p => new productDetail
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
            }).FirstOrDefault();


            if (infoProduct != null)
            {
                return Json(infoProduct, JsonRequestBehavior.AllowGet);
            }

            // Xử lý khi không tìm thấy sản phẩm
            return RedirectToAction("NotFound");

        }

        //tìm sp
        public ActionResult ResultFind(string keyword)
        {
            var products = db.products.ToList();
            products = db.products.Where(p => p.product_name.Contains(keyword)).ToList();

            var FindResult = products.Select(p => new productDetail { product_id = p.product_id, product_name = p.product_name, product_price = p.product_price, product_image = p.product_image, product_ingredients = p.product_ingredients, catetory_id = p.catetory_id }).ToList();
            return View(FindResult);
        }

        public ActionResult Product()
        {
            ViewBag.Message = "Your product page.";
            return View();
        }

        public ActionResult Review()
        {
            ViewBag.Message = "Your review page.";

            return View();
        }

        huhuEntities db = new huhuEntities();
        

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

        public string CaptchaCode { get; set; }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Incorrect!")]
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
                    MvcCaptcha.ResetCaptcha("ExampleCaptcha");
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