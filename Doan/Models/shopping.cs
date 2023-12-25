using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doan.Models
{
    public class bill
    {
        public int order_id { get; set; }
        [Required]
        public Nullable<int> user_id { get; set; }
        [Required]

        public Nullable<System.DateTime> order_date { get; set; }
        public string order_address { get; set; }
        [Required]

        public string payment { get; set; }

        [Required]
        public string payment_status { get; set; }
        [Required]

        public Nullable<double> total { get; set; }


    }
}