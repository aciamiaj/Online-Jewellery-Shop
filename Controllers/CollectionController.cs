using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineJewelleryShop.Models;
using System.Data;

namespace OnlineJewelleryShop.Controllers
{
    public class CollectionController : Controller
    {
        private readonly JjewelleryContext _dbContext;

        // Constructor to inject the DbContext
        public CollectionController(JjewelleryContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Action method to display all products
        public IActionResult Products()
        {
            var products = _dbContext.Products.ToList();
            return View(products);
        }

        // Action method to display only diamond products
        public IActionResult Diamonds()
        {
            var products = _dbContext.Products.Include(c => c.Category).Where(c => c.Category.CategoryName == "Diamond").ToList();
            return View(products);
        }

        // Action method to add new diamond products (only accessible to admins)
        [Authorize(Roles = "Admin")]
        public IActionResult AddDiamonds()
        {
            // Set ViewBag to populate dropdown lists for category and product type
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Diamond").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        // POST method to handle the submission of new diamond products (only accessible to admins)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDiamonds(Product product)
        {
            // Validate the submitted product data
            if (ModelState.IsValid)
            {
                // Check if the product ID already exists in the database
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                // Display error message if product ID already exists
                if (existingProduct != null)
                {
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    // Set ViewBag to populate dropdown lists for category and product type
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Diamond").ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product);
                }

                // Add the new product to the database
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                // Redirect to the Products action method to display all products
                return RedirectToAction(nameof(Products));
            }
            // If validation fails, return the view with error messages and dropdown lists pre-populated
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Diamond").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }
        // Action method to display only gold products
        public IActionResult Gold()
        {
            var products = _dbContext.Products.Include(c => c.Category).Where(c => c.Category.CategoryName == "Gold").ToList();
            return View(products);
        }

        // Action method to add new gold products (only accessible to admins)
        [Authorize(Roles = "Admin")]
        public IActionResult AddGold()
        {
            // Set ViewBag to populate dropdown lists for category and product type
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Gold").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        // This action is called when a new Gold product is added
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGold(Product product)
        {
            if (ModelState.IsValid) // Check if the submitted form data is valid
            {
                // Check if a product with the same ID already exists in the database
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null) // If a product with the same ID exists, display an error message
                {
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Diamond").ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product); // Return the same view with the error message and the same form data
                }

                _dbContext.Products.Add(product); // Add the new product to the database
                _dbContext.SaveChanges(); // Save changes to the database

                return RedirectToAction(nameof(Products)); // Redirect to the Products page
            }
            // If the submitted form data is invalid, display the same view with the error messages and the same form data
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Gold").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }

        // This action is called when the Pearl page is loaded
        public IActionResult Pearl()
        {
            // Get all products that belong to the Pearl category
            var products = _dbContext.Products.Include(c => c.Category).Where(c => c.Category.CategoryName == "Pearl").ToList();
            return View(products); // Display the Pearl view with the products
        }

        // This action is called when the Add Pearl page is loaded
        [Authorize(Roles = "Admin")]
        public IActionResult AddPearl()
        {
            // Display the Add Pearl view with the categories and product types in dropdown lists
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Pearl").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        // This action is called when a new Pearl product is added
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddPearl(Product product)
        {
            if (ModelState.IsValid) // Check if the submitted form data is valid
            {
                // Check if a product with the same ID already exists in the database
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null) // If a product with the same ID exists, display an error message
                {
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Pearl").ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product); // Return the same view with the error message and the same form data
                }

                _dbContext.Products.Add(product); // Add the new product to the database
                _dbContext.SaveChanges(); // Save changes to the database

                return RedirectToAction(nameof(Products)); // Redirect to the Products page
            }
            // Display the Add Pearl view with the categories and product types in dropdown lists
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.Where(c => c.CategoryName == "Pearl").ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }
    }
}
