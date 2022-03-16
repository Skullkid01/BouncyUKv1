using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{
    public class Invoice
    {
        DataContext db = new DataContext();

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Invoice ID :")]
        public int InvID { get; set; }

        [DisplayName("Date Of Payment / Booking Process :")]
        public System.DateTime DateCreated { get; set; }

        [DisplayName("Email Address :")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Payment Method :")]
        public string PayMethod { get; set; }

        [DisplayName("Amount :")]
        public double TotalAmount { get; set; }


        public DateTime GetDT()
        {
            DateCreated = DateTime.Now;

            return DateCreated;
        }
    }
}