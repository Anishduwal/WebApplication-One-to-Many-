using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Details = new List<OrderDetail>();
        }
        public long id { get; set; }
        public string Customer_name { get; set; }
        public string Address { get; set; }
        public string Phone_number { get; set; }
        public string[] OrderDetails { get; set; }
        public IList<OrderDetail> Details { get; set; }

    }
    public class OrderDetail
    {
        public string Order_name { get; set; }
        public Nullable<System.DateTime> Order_date { get; set; }
    }
}