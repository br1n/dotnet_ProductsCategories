using Products_Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace Products_Categories
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}
        //set DB models here
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<Association> Associations {get; set;}
    }
}