using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Routing;

namespace BouncyUKv1.Models
{
    public class Book
    {
        DataContext db = new DataContext();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Booking ID :")]
        public int BookID { get; set; }

        [DisplayName("Booking Date :")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Date]
        public DateTime BookingDate { get; set; }


        [DisplayName("Delivery Address :")]
        [DataType(DataType.MultilineText)]
        public string DeliverAddress { get; set; }
        [DisplayName("Postal Code :")]
        public string PostalCode { get; set; }
        [DisplayName("Contact Number :")]
        [DataType(DataType.PhoneNumber)]
        public string Cell { get; set; }

        [DisplayName("Time of Delivery :")]
        [DataType(DataType.Time)]
        [Time]
        public DateTime Time { get; set; }

        [DisplayName("Payment Method :")]
        public string PayMtd { get; set; }

        [DisplayName("Product Reference :")]
        public string Refer { get; set; }

        [DisplayName("Total Cost :")]
        public double TotalCost { get; set; }
        
        public int id { get; set; }

        [DisplayName("User :")]
        public string username { get; set; }
        [DisplayName("Booking Reference :")]
        public string Booking { get; set; }




        public string ProductRef()
        {

            var query =
            (from c in db.Products
             where c.ProductID == id
             select new { c.ProductRef }).Single();

            string result = query.ProductRef;

            return result;
        }



        public double GetPrice()
        {

            var query =
            (from c in db.Products
             where c.ProductID == id
             select new { c.Price }).Single();

            double result = query.Price;

            return result;
        }
        public double GetTotal()
        {
            var total = GetPrice();

            return Convert.ToDouble(total);

        }



    }
}