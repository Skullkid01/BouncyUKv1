using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BouncyUKv1.Models
{
    public class DataContext:DbContext
    {
        public DataContext() : base("name=conn") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);



        }

        public DbSet<UserAccount> userAccount { get; set; }

        public DbSet<Product> Products { get; set; }


        public DbSet<Book> Booking { get; set; }

        public DbSet<Invoice> Invoices { get; set; }


    }
}