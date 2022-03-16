using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BouncyUKv1.Models;
using System.IO;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BouncyUK.Controllers
{
    [RequireHttps]
    public class InvoicesController : Controller
    {
        private DataContext db = new DataContext();
        public static Random id = new Random();
        public static int num = id.Next(1000);

        // GET: Invoices
        public ActionResult Index()
        {
            return View(db.Invoices.ToList());
        }
        public ActionResult Records()
        {
            return View(db.Invoices.ToList());
        }
        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }
        public ActionResult Preview()
        {
            return View(db.Invoices.ToList());
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvID ,DateCreated , Email , PaymentMethod , TotalAmount ")] Invoice invoice)
        {

            Session["InvID"] = invoice.InvID.ToString();



            var doc = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            doc.Open();

            Image image = Image.GetInstance(Server.MapPath("~/Content/Castle.jfif"));
            image.ScaleAbsolute(120f, 155.25f);
            image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth), (PageSize.A4.Height - image.ScaledHeight));
            writer.DirectContent.AddImage(image, false);

            Paragraph heading = new Paragraph("GOOD DAY " + Session["CName"].ToString().ToUpper(), new iTextSharp.text.Font());
            heading.Alignment = 1;
            doc.Add(heading);
            PdfPCell cell = new PdfPCell(new Phrase("Invoice Details"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPTable table = new PdfPTable(1);

            table.AddCell(cell);
            table.AddCell("Invoice ID :" + Session["Username"]+ num+ " - BouncyUK - "+ DateTime.Now.Year);
            table.AddCell("Customer Name :" + Session["CName"].ToString());
            table.AddCell("Customer Surname :" + Session["CSurname"].ToString());
            table.AddCell("Date & Time Of Payment  :" + invoice.GetDT().ToString());
            table.AddCell("Payment Method  :" + Session["PayMtd"].ToString());
            table.AddCell("Amount  :" + "£" + Session["TotalCost"].ToString());
            table.AddCell("Customer Email  :" + Session["Email"].ToString());
            doc.Add(table);





            PdfContentByte content = writer.DirectContent;
            Rectangle rectangle = new Rectangle(doc.PageSize);
            rectangle.Left += doc.LeftMargin;
            rectangle.Right -= doc.RightMargin;
            rectangle.Top -= doc.TopMargin;
            rectangle.Bottom += doc.BottomMargin;
            content.SetColorStroke(GrayColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();


            writer.CloseStream = false;
            doc.Close();

            memoryStream.Position = 0;

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("bouncyuk21@gmail.com");
            mm.To.Add(Session["Email"].ToString());
            {
                mm.Subject = "Payment Details  - Bouncy UK ";
                mm.IsBodyHtml = true;
                mm.Body = " Good Day : " + Session["CName"] + " Please find attached invoice summary :";
            };
            mm.Attachments.Add(new Attachment(memoryStream, "Invoice Summary.pdf"));
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("bouncyuk21@gmail.com", "bouncyuk19")

            };

            smtp.Send(mm);


            if (invoice == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                invoice.Email = Session["Email"].ToString();
                invoice.PayMethod = Session["PayMtd"].ToString();
                invoice.DateCreated = invoice.GetDT();
                invoice.TotalAmount = Convert.ToDouble(Session["TotalCost"]);
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("ProductPage", "Products");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvID,Email")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
