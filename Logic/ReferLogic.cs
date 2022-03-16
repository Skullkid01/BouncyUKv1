using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BouncyUKv1.Models;

namespace BouncyUKv1.Logic
{
    public class ReferLogic
    {
        DataContext db = new DataContext();

        public List<Product> Getdata()
        {
            return db.Products.ToList();
        }

        public List<Product> GetAll()
        {
            return Getdata().Select(x => new Product
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                Description = x.Description,
                Category = x.Category,
                Price = x.Price,
                Image = x.Image,
                ImageType = x.ImageType,
            }).ToList();
        }



        public Product FindById(int id)
        {
            return GetAll().FirstOrDefault(x => x.ProductID.Equals(id));
        }


        public IQueryable<Product> coreSearchAdmin(string refer)
        {

            var result = from i in db.Products select i;

            if (!string.IsNullOrEmpty(refer))
            {
                try
                {
                    result = result.Where(x => x.ProductRef.Contains(refer));
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