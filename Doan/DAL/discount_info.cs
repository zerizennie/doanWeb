//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Doan.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class discount_info
    {
        public int product_id { get; set; }
        public Nullable<decimal> discount_percentage { get; set; }
        public Nullable<System.DateTime> discount_start_date { get; set; }
        public Nullable<System.DateTime> discount_end_date { get; set; }
    
        public virtual product product { get; set; }
    }
}
