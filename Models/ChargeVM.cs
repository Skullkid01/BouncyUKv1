using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{
    public class ChargeVM
    {

        DataContext db = new DataContext();
        public string ChargeID { get; set; }

        [DisplayName("Amount Due :")]
        public double amt { get; set; }



    }
}