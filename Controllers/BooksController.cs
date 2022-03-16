using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BouncyUKv1.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using DHTMLX.Scheduler;

namespace BouncyUK.Controllers
{
    [RequireHttps]
    public class BooksController : Controller
    {
        private DataContext db = new DataContext();
        public static Random id = new Random();
        public static int num = id.Next(1000);
        // GET: Books

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Index()
        {
            var book = db.Booking.ToList();
            return View(book);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Booking.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create(int? id)
        {

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,BookingDate,PayMtd,Refer,TotalCost,DeliverAddress,PostalCode,Cell,Time , id , username , Booking")] Book book)
        {

            var url = Request.QueryString["id"];

            Session["BookID"] = book.BookID.ToString();
            Session["PayMtd"] = book.PayMtd.ToString();
            Session["TotalCost"] = book.GetTotal().ToString();


            //var date = book.BookingDate;
            //if (date != null)
            //{
            //    ViewBag.BMessage = "Booking Date Invaild , Try a future date";
            //}

            //var time = book.Time;
            //if (time != null)
            //{
            //    ViewBag.TMessage = "Booking Time Invaild , Try a Between 8AM- 8PM";
            //}
            var Time = (from i in db.Booking
                        where i.Time == book.Time && i.BookingDate == book.BookingDate
                        select new { book.Time, book.BookingDate }).FirstOrDefault();
            if (Time != null)
            {
                ViewBag.Message = "Booking Time Reserved , Try another hour";

            }
            else
            {
                if (ModelState.IsValid)
                {
                    book.TotalCost = book.GetTotal();
                    book.Refer = book.ProductRef();
                    book.id = Convert.ToInt32(url);
                    book.Booking = BookRef();
                    book.username = Session["Username"].ToString();
                    db.Booking.Add(book);
                    BookingEmail(book);
                    BookingEmailCustomer(book);
                    db.SaveChanges();
                    if (book.PayMtd == "Cash On Delivery")
                    {
                        return RedirectToAction("Create", "Invoices");
                    }
                    else
                    if (book.PayMtd == "Online Payment")
                    {
                        return RedirectToAction("Payment", "StripePayments");
                    }

                }

            }

            return View(book);
        }

        public JsonResult GetBooking()
        {
            using (DataContext dc = new DataContext())
            {
                var v = dc.Booking.OrderBy(a => a.BookingDate).ToList();
                return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }



        private string BookRef()
        {
            var book = (Session["Username"].ToString() + DateTime.Now.Year + "UK" + num).ToString();

            return (book);
        }


        private void BookingEmail(Book obj)
        {

            try
            {
                using (MailMessage mm = new MailMessage("bouncyuk21@gmail.com", "bouncyuk21@gmail.com"))
                {
                    mm.Subject = "Bouncy UK : Customer Booking - Customer :" + Session["CName"];
                    string body = Session["CName"] + "\n" + "has booked a castle";
                    body += "<br /><br />";
                    body += "Booking ID :" +obj.Booking ;
                    body += "<br />";
                    body += "Castle Reference : " + obj.Refer;
                    body += "<br />";
                    body += " Payment Method : " + Session["PayMtd"];
                    body += "<br />";
                    body += " Amount Payable : " + "£" + obj.TotalCost;
                    body += "<br />";
                    body += " Delivery Address : " + obj.DeliverAddress;
                    body += "<br />";
                    body += " Date Reserved: " + obj.BookingDate.ToShortDateString();
                    body += "<br />";
                    body += " Time : " + obj.Time.ToShortTimeString();
                    body += "<br />";
                    body += " Postal Code : " + obj.PostalCode;
                    body += "<br />";
                    body += " Contact Number : " + obj.Cell;
                    body += "<br />";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("bouncyuk21@gmail.com", "bouncyuk19");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;

                    smtp.Send(mm);

                }


            }
            catch (Exception e)
            {
                Response.Write(e.Message);

            }
        }
        private void BookingEmailCustomer(Book obj)
        {


            try
            {
                using (MailMessage mm = new MailMessage("bouncyuk21@gmail.com", Session["Email"].ToString()))
                {

                    Product pro = new Product();
                    mm.Subject = "Bouncy UK : Your Booking Details - Customer :" + Session["CName"];
                    string body = "You booked a castle" + "\n" + Session["CName"];
                    body += "<br /><br />";
                    body += "Booking ID : " + obj.Booking;
                    body += "<br />";
                    body += "Castle Reference : " + obj.Refer;
                    body += "<br />";
                    body += " Your Delivery Address : " + obj.DeliverAddress;
                    body += "<br />";
                    body += " Date Reserved: " + obj.BookingDate.ToShortDateString();
                    body += "<br />";
                    body += " Time Booked : " + obj.Time.ToShortTimeString();
                    body += "<br />";
                    body += " If you have any issues kindly email us @ bouncyuk21@gmail.com with your Booking ID ";
                    body += "<br />";

                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("bouncyuk21@gmail.com", "bouncyuk19");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;

                    smtp.Send(mm);

                }


            }
            catch (Exception e)
            {
                Response.Write(e.Message);

            }
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Booking.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,BookingDate,TotalCost,DeliverAddress,PostalCode,Cell,Time")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Booking.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Booking.Find(id);
            db.Booking.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
