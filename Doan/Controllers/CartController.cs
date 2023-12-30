using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Doan.DAL;
//using Doan.Models;
using Newtonsoft.Json;

namespace Doan.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        huhuEntities db = new huhuEntities();
        private const string CartSession = "CartSession"; //tao session gio hang
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart; //kiem tra gio hang co null khong, neu khong thi luu vao
            }
            return View(list);
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });


        }

        private JsonResult Delete(long productId)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.product_id == productId); //xoa san pham theo id
            Session[CartSession] = sessionCart;
            return
                Json(new
                {
                    status = true
                });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.product.product_id == item.product.product_id);
                if (jsonItem != null)
                {
                    item.quantity = jsonItem.quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        
        public ActionResult AddItem(int productId, int quantity)
        {

            var product = db.products.FirstOrDefault(c => c.product_id == productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.product_id == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.product.product_id == productId)
                        {
                            item.quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.product = product;
                    item.quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.product = product;
                item.quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new bill();
            order.order_date = DateTime.Now;
            order.order_address = address;
            order.bill_email= email;
            order.bill_name = shipName;
            order.bill_phone = mobile;

            try
            {

                //Thêm Order
                db.bills.Add(order);
                db.SaveChanges();
                var Id = order.order_id; // cho id dau tien = 1
                //if (db.bills.Count() > 0) //kiem tra xem da co don hang nao chua, neu co thi tang gia tri Id
                //{
                //    Id = (int)(db.bills.Max(x => x.id) + 1);
                //}

                var sessioncart = (List<CartItem>)Session[CartSession];


                double total = 0;
                foreach (var item in sessioncart)
                {
                    var orderDetail = new order_detail_id();
                    orderDetail.product_id = item.product.product_id;
                    orderDetail.order_id = Id;
                    orderDetail.product_price = item.product_price;
                    orderDetail.quantity = item.quantity;
                    db.order_detail_id.Add(orderDetail);
                    db.SaveChanges();
                    total += item.product.product_price.GetValueOrDefault(0) * item.quantity;
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
               // var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

               // Để Gmail cho phép SmtpClient kết nối đến server SMTP của nó với xác thực
               //là tài khoản gmail của bạn, bạn cần thiết lập tài khoản email của bạn như sau:
               // Vào địa chỉ https://myaccount.google.com/security  Ở menu trái chọn mục Bảo mật, sau đó tại mục Quyền truy cập 
               // của ứng dụng kém an toàn phải ở chế độ bật
               //   Đồng thời tài khoản Gmail cũng cần bật IMAP
               // Truy cập địa chỉ https://mail.google.com/mail/#settings/fwdandpop

               // new MailHelper().SendMail(email, "Đơn hàng mới từ Tiệm Bánh", content);
               // new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Tiệm Bánh", content);
                //Xóa hết giỏ hàng
                Session[CartSession] = null;
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/Cart/UnSuccess");
            }
            return Redirect("/Cart/Success");
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult UnSuccess()
        {
            return View();
        }
    }
}
