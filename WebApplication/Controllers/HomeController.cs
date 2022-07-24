using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        DemoEntities db = new DemoEntities();
        public ActionResult Index()
        {
            var orderlist = db.Customer_Order.DistinctBy(a=> a.Order_name).ToList();
            ViewBag.ordername = new MultiSelectList(orderlist, "id", "Order_name" );
            var list = db.Customers.ToList();
            return View(list);
        }


        [HttpPost]
        public JsonResult Create(CustomerModel model)
        {
            Customer customer = new Customer();
            customer.Customer_name = model.Customer_name;
            customer.Address = model.Address;
            customer.Phone_number = model.Phone_number;

            foreach (var item in model.OrderDetails)
            {
                Customer_Order order = new Customer_Order();
                var oid = Convert.ToInt64(item);
                var ordername = db.Customer_Order.FirstOrDefault(x => x.id == oid).Order_name;
                order.Order_name = ordername;
                order.Order_date = DateTime.Now;
                customer.Customer_Order.Add(order);
            }

            db.Customers.Add(customer);
            db.SaveChanges();
            String message = "SUCCESS";
            return Json(new { Status = true, Message = message, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var del = db.Customers.Find(id);
            db.Customer_Order.RemoveRange(del.Customer_Order);
            db.Customers.Remove(del);
            db.SaveChanges();
            return Json(new { Status = true, Message = "Deleted Successfully", JsonRequestBehavior.AllowGet });
        }
        [HttpGet]
        public ActionResult Update(CustomerModel model)
        {
            var data = db.Customers.Find(model.id);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(CustomerModel model)
        {

            if (model.id == 0)
            {
                throw new Exception("Customer id not found");
            }
            var data = db.Customers.Find(model.id);
            var del = db.Customer_Order.Where(X => X.Cid == model.id);
            db.Customer_Order.RemoveRange(del);

            data.Customer_name = model.Customer_name;
            data.Address = model.Address;
            data.Phone_number = model.Phone_number;
            foreach (var item in model.Details)
            {
                if (item.Order_name != null)
                {
                    Customer_Order order = new Customer_Order();
                    order.Order_name = item.Order_name;
                    order.Order_date = DateTime.Now;

                    data.Customer_Order.Add(order);
                }
            }
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Updated the record successfully";
            ViewBag.Message = message;
            return RedirectToAction("Index");

        }

    }
}