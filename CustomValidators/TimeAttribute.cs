using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{

    public class TimeAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime time = Convert.ToDateTime(value);
            TimeSpan s = new TimeSpan(08, 0, 0);
            TimeSpan e = new TimeSpan(20, 0, 0);
            var start = Convert.ToDateTime(s.ToString());
            var end = Convert.ToDateTime(e.ToString());
            if (time >= start && time <= end)
            {


                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Try Between 8AM AND 8PM"); 
            }
        }
    }

}