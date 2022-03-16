using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BouncyUKv1.Models;

namespace BouncyUKv1.Logic
{
    public class UserLogic
    {
        DataContext db = new DataContext();

        public List<UserAccount> Getdata()
        {
            return db.userAccount.ToList();
        }

        public List<UserAccount> GetAll()
        {
            return Getdata().Select(x => new UserAccount
            {
                ClientID = x.ClientID,
                CName = x.CName,
                CSurname = x.CSurname,
                UName = x.UName,
                Email = x.Email,
                Password = x.Password,
                CPassword = x.CPassword,
            }).ToList();
        }
        public UserAccount FindById(int id)
        {
            return GetAll().FirstOrDefault(x => x.ClientID.Equals(id));
        }
        public IQueryable<UserAccount> coreSearchAdmin(string CustomerName , string CustomerSname, string email , string user)
        {

            var result = from i in db.userAccount select i;

            if (!string.IsNullOrEmpty(CustomerName))
            {
                try
                {
                    result = result.Where(x => x.CName.Contains(CustomerName));
                }
                catch
                {
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(CustomerSname))
            {
                try
                {
                    result = result.Where(x => x.CSurname.Contains(CustomerSname));
                }
                catch
                {
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    result = result.Where(x => x.Email.Contains(email));
                }
                catch
                {
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(user))
            {
                try
                {
                    result = result.Where(x => x.UName.Contains(user));
                }
                catch
                {
                    return null;
                }
            }
            return result;
        }
    }
}