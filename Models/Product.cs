using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{
    public partial class Product
    {
        DataContext db = new DataContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [DisplayName("Castle Name :")]
        public string ProductName { get; set; }

        [DisplayName("Castle Description :")]
        public string Description { get; set; }

        [DisplayName("Castle Category :")]
        public string Category { get; set; }

        [DisplayName("Cost :")]
        public double Price { get; set; }

        [DisplayName("Product :")]
        public byte[] Image { get; set; }
        [DisplayName("Type :")]
        public string ImageType { get; set; }

        [DisplayName("Product Reference :")]
        public string ProductRef { get; set; }


        public string GetRef()
        {
            var refer = ProductName.Substring(0, 6) + "/" + DateTime.Now.Year;

            return refer;
        }


    }
}