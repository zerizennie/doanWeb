using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}