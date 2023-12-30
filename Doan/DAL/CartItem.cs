using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doan.DAL
{
    [Serializable]
    public class CartItem
    {
        public product product { get; set; }
        public int product_id { get;set; }
        public double product_price { get; set; }

        public int quantity { get; set; }

    }
}