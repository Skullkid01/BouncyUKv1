using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;
using BouncyUKv1.Models;
using System.Net;

namespace StripePayments.Controllers
{
    [RequireHttps]
    public class StripePaymentsController : Controller
    {

        public ActionResult PaymentStatus()
        {
            return View();
        }
        // GET: Cart/Create
        public ActionResult Payment()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        public ActionResult Payment(string stripeToken)
        {
            var model = new ChargeVM();
            int val = Convert.ToInt32(Session["TotalCost"]);


            StripeConfiguration.ApiKey = "sk_test_51IT1kDDNWIB8svWnO35GJIW1qieiWTipl6DZnM6cWo3rJL59wwIQ8UWfkTkIT9UEBfOmxhJtcHDag05KDCwWEduj00pt9g1QuE";

            var options = new ChargeCreateOptions
            {


                Amount = val * 100,
                Currency = "gbp",
                Source = stripeToken,
                Description = "BouncyUK-Payment /" + Session["CName"],
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);




            model.ChargeID = charge.Id;


            return View("PaymentStatus", model);
        }
    }
}
