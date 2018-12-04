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
    [Route("categories")]
    public class CategoryController : Controller
    {
        private MyContext dbContext;
        public CategoryController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllCategories = dbContext.Categories;
            return View();
        }

        [HttpPost("add_category")]
        public IActionResult Create(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(newCategory);
                dbContext.SaveChanges();
            }
            ViewBag.AllCategories = dbContext.Categories;
            return RedirectToAction("Index", "Category");
        }

        [HttpGet("{categoryId}")]
        public IActionResult Show(int categoryId)
        {
            var category = dbContext.Categories
            .Include(c => c.categoryAssociations)
            .ThenInclude(a => a.Product)
            .FirstOrDefault(c => c.CategoryId == categoryId);

            var unrealatedProducts = dbContext.Products
                .Include(c => c.productAssociations)
                .Where(p => p.productAssociations.All(a => a.CategoryId != categoryId));

            ViewBag.CategoryInfo = category;
            ViewBag._products = unrealatedProducts;

            return View();
        }

        [HttpPost("Add_Category_Product")]
        public IActionResult AddProductToCategory(Association newAssociation)
        {
            dbContext.Associations.Add(newAssociation);
            dbContext.SaveChanges();
            int CategoryId = newAssociation.CategoryId;
            return RedirectToAction("Show", new{categoryId = CategoryId});
        }

    }
}