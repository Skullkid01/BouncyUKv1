using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BouncyUKv1.Models
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool validPassword = false;
            string reason = String.Empty;
            string Password = value == null ? String.Empty : value.ToString();
            if (String.IsNullOrEmpty(Password) || Password.Length < 8)
            {
                reason = "Your  password must be at least 8 characters long. ";
            }
            else
            {
                Regex reSymbol = new Regex("[^a-zA-Z0-9]");
                if (!reSymbol.IsMatch(Password))
                {
                    reason += "Your  password must contain at least 1 special character , e.g: @";
                }
                else
                {
                    validPassword = true;
                }
            }
            if (validPassword)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(reason);
            }
        }
    }
}