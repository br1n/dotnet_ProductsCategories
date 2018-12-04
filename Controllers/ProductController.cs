using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products_Categories.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Products_Categories.Controllers
{
    public class ProductController : Controller
    {
         private MyContext dbContext;
        public ProductController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllProducts = dbContext.Products;
            return View();
        }


        [HttpPost("add_product")]
        public IActionResult Create(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
            }
            ViewBag.AllProducts = dbContext.Products;
            return RedirectToAction("Index");
        }

        [HttpGet("products/{productId}")]
        public IActionResult Show(int productId)
        {
            var product = dbContext.Products
                .Include(p => p.productAssociations)
                .ThenInclude(a => a.Category)
                .FirstOrDefault(p => p.ProductId == productId);

            var unrealatedCategories = dbContext.Categories
                .Include(c => c.categoryAssociations)
                .Where(c => c.categoryAssociations.All(a => a.ProductId != productId));

            ViewBag.ProductInfo = product;
            ViewBag._categories = unrealatedCategories;

            return View();
        }

        [HttpPost("Add_Product_Category")]
        public IActionResult AddCategoryToProduct(Association newAssociation)
        {
            dbContext.Associations.Add(newAssociation);
            dbContext.SaveChanges();
            int ProductId = newAssociation.ProductId;
            return RedirectToAction("Show",new{productId = ProductId});
        }
    }
}
