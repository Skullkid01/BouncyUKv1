using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{
    public class Contact
    {
        [Display(Name = " Name :")]
        public string Name { get; set; }

        [Display(Name = "Email Address :")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Phone Number :")]
        [DataType(DataType.PhoneNumber)]
        public string CellPhone { get; set; }
        [Display(Name = "Message :")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}