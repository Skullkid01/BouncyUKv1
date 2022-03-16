using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BouncyUKv1.Models;
namespace BouncyUKv1.Logic
{
    public class BookLogic
    {
        DataContext db = new DataContext();

        public List<Book> Getdata()
        {
            return db.Booking.ToList();
        }

        public List<Book> GetAll()
        {
            return Getdata().Select(x => new Book
            {
                BookID = x.BookID,
                BookingDate = x.BookingDate,
                DeliverAddress = x.DeliverAddress,
                PostalCode = x.PostalCode,
                Cell = x.Cell,
                Time = x.Time,
                PayMtd = x.PayMtd,
                Refer = x.Refer,
                TotalCost = x.TotalCost,
                id = x.id,
                username = x.username,
                Booking = x.Booking,
            }).ToList();
        }
        public Book FindById(int id)
        {
            return GetAll().FirstOrDefault(x => x.BookID.Equals(id));
        }

        public IQueryable<Book> coreSearchAdmin(string refer)
        {

            var result = from i in db.Booking select i;

            if (!string.IsNullOrEmpty(refer))
            {
                try
                {
                    result = result.Where(x => x.Booking.Contains(refer));
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