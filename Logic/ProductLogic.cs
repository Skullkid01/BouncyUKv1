using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BouncyUKv1.Models;

namespace BouncyUKv1.Logic
{
    public class ProductLogic
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


        public IQueryable<Product> coreSearchAdmin(string PName, string PPrice, string PCategory)
        {

            //var result = GetAll().AsQueryable();
            var result = from i in db.Products select i;

            if (!string.IsNullOrEmpty(PName))
            {
                try
                {
                    result = result.Where(x => x.ProductName.Contains(PName.ToLower()));
                }
                catch
                {
                    return null;
                }
            }


            if (!string.IsNullOrEmpty(PPrice))
            {
                if (PPrice == "New")
                {
                    result = result.OrderByDescending(x => x.Price);
                }
                else if (PPrice == "Old")
                {
                    result = result.OrderBy(x => x.Price);
                }
            }

            if (!string.IsNullOrEmpty(PCategory))
            {
                if (PCategory == "Cate1")
                {
                    result = result.Where(x => x.Category == "Boys Castles");
                }
                else if (PCategory == "Cate2")
                {
                    result = result.Where(x => x.Category == "Girls Castles");
                }
                else if (PCategory == "Cate3")
                {
                    result = result.Where(x => x.Category == "Disney Castles");
                }
                else if (PCategory == "Cate4")
                {
                    result = result.Where(x => x.Category == "Superhero Castles");
                }
            }

            return result;
        }
    }
}