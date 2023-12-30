using Doan.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan.Models
{
    public class categoryDetail
    {
        public int catetory_id { get; set; }
        [Required(ErrorMessage ="Name Required")]
        public string catetory_name { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }

    }

    public class productDetail
    {
        public int product_id { get; set; }
        [Required(ErrorMessage ="Name Required")]
        public string product_name { get; set; }
        public string product_ingredients { get; set; }
        public string product_description { get; set; }
        public Nullable<double> product_price { get; set; }
        public string product_image { get; set; }
        public Nullable<int> catetory_id { get; set; }
        [Required]
        public Nullable<int> quantity { get; set; }
        public Nullable<double> current_price { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }

        public SelectList catetory { get; set; }

    }
}


