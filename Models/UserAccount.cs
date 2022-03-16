using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BouncyUKv1.Models
{
    public class UserAccount
    {
        DataContext db = new DataContext();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientID { get; set; }

        [Display(Name = "Customer Name :")]
        public string CName { get; set; }

        [Display(Name = "Customer Surname :")]
        public string CSurname { get; set; }

        [Display(Name = "Email Address :")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Username :")]
        public string UName { get; set; }

        [Display(Name = "Password :")]
        [Password]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password :")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Password]
        public string CPassword { get; set; }



    }
}