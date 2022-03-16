using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BouncyUKv1.Models;
using System.Timers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;
using System.Threading;
using System.Net.Mail;
using BouncyUKv1.Logic;
using MailMessage = System.Net.Mail.MailMessage;

namespace BouncyUK.Controllers
{
    [RequireHttps]
    public class UserAccountsController : Controller
    {
        private DataContext db = new DataContext();
        UserLogic ul = new UserLogic();
        BookLogic bl = new BookLogic();
        string mess = "Username OR Password Invalid";

        public ActionResult AllBooking(string refer)
        {
            return View(bl.coreSearchAdmin(refer));
        }
        public ActionResult AllUsers(string CustomerName, string CustomerSname, string email, string user)
        {
            return View(ul.coreSearchAdmin(CustomerName, CustomerSname, email , user));
        }
        public ActionResult AdminPage()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Home");
        }
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount acc = db.userAccount.Find(id);
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LoggedIn"); 



            }

            return View();

        }

        public ActionResult BookingHistory()
        {
            var user = Session["Username"].ToString();
            var query = (from u in db.Booking where u.username.Equals(user) select u).ToList();

            return View(query);
        }
        public ActionResult Home()

        {
            return View();
        }
        
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(UserAccount account)
        {

            if (ModelState.IsValid)
            {
                db.userAccount.Add(account);
                db.SaveChanges();
                RedirectToAction("Login");

                ModelState.Clear();
                ViewBag.Message = "Successfully Registered";
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Login(UserAccount user)
        {
            if (user != null)
            {
                Session["Username"] = user.UName.ToString();
                if (user.UName.Equals("Administrator") && user.Password.Equals("admin123"))
                {
                    return RedirectToAction("AdminPage");
                }

            }

            using (DataContext db = new DataContext())
            {
                var usr = db.userAccount.SingleOrDefault(u => u.UName == user.UName && u.Password == user.Password);

                if (usr != null)
                {
                    Session["ClientID"] = usr.ClientID.ToString();
                    Session["Username"] = usr.UName.ToString();
                    Session["CName"] = usr.CName.ToString();
                    Session["CSurname"] = usr.CSurname.ToString();
                    Session["Email"] = usr.Email.ToString();
                    return RedirectToAction("LoggedIn");
                }

                else
                {
                    ViewBag.Message = mess;
                }

            }
            return View();
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult LoggedIn(UserAccount acc)
        {
            if (Session["ClientID"] != null)
            {
                using (DataContext db = new DataContext())
                {


                    string usr = Session["UserName"].ToString();
                    var query = (from s in db.userAccount where s.UName.Equals(usr) select s).ToList();

                    return View(query);

                }
            }

            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult ContactUs(Contact c)
        {
            if (ModelState.IsValid)
            {
                SendEmail(c);
                ModelState.Clear();

            }
            //ViewBag.Message = ("Email Sent");
            return View(c);
        }
        public void SendEmail(Contact contact)
        {
            try
            {
                if (!String.IsNullOrEmpty(contact.Email))
                    using (MailMessage mail = new MailMessage(contact.Email, "bouncyuk21@gmail.com"))
                    {
                        const string email = "bouncyuk21@gmail.com";
                        const string password = "bouncyuk19";

                        var loginInfo = new NetworkCredential(email, password);
                        mail.Subject = "Bouncy UK Enquiry" + "-" + DateTime.Now.Year;
                        string body = contact.Name + "\n" + "has logged an enquiry";
                        body += "<br /><br />";
                        body += "Name:" + "\n" + contact.Name;
                        body += "<br/>";
                        body += "Email:" +"\n"+contact.Email;
                        body += "<br/>";
                        body += "Cell Number:" + contact.CellPhone;
                        body += "<br/>";
                        body += "Enquiry:" + "\n" + contact.Message;
                        body += "<br/>";
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        try
                        {
                            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))

                            {
                                smtpClient.EnableSsl = true;
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = loginInfo;
                                smtpClient.Send(mail);
                            }

                        }

                        finally
                        {
                            //dispose the client
                            mail.Dispose();
                        }

                    }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write(t.FailedRecipient);
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }

        public ActionResult About()
        {
            return View();
        }

        // GET: UserAccounts
        public ActionResult Index()
        {
            return View(db.userAccount.ToList());
        }

        // GET: UserAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.userAccount.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }


        // GET: UserAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.userAccount.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientID,CName,CSurname,Email,UName,Password,CPassword")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.userAccount.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount userAccount = db.userAccount.Find(id);
            db.userAccount.Remove(userAccount);
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
