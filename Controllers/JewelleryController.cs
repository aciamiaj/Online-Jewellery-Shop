using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineJewelleryShop.Models;
using System.Data;

namespace OnlineJewelleryShop.Controllers
{
    public class JewelleryController : Controller
    {
        private readonly JjewelleryContext _dbContext;

        public JewelleryController(JjewelleryContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Action to display all products
        public IActionResult Products()
        {
            // Retrieve all products from the database
            var products = _dbContext.Products.ToList();
            return View(products);
        }

        // Action to display all earrings
        public IActionResult Earrings()
        {
            // Retrieve all products with ProdTypeName equal to "Earrings" from the database, including ProdType navigation property
            var products = _dbContext.Products.Include(p => p.ProdType).Where(p => p.ProdType.ProdTypeName == "Earrings").ToList();
            return View(products);
        }

        // Action to display the form for adding earrings, only accessible to users with the "Admin" role
        [Authorize(Roles = "Admin")]
        public IActionResult AddEarrings()
        {
            // Retrieve all categories and product types with ProdTypeName equal to "Earrings" from the database to populate the dropdown lists in the form
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Earrings").ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        // Action to add a new product to the database, only accessible to users with the "Admin" role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEarrings(Product product)
        {
            // Check if the submitted product ID already exists in the database
            if (ModelState.IsValid)
            {
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null)
                {
                    // If the product ID already exists, display an error message and repopulate the dropdown lists in the form
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Earrings").ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product);
                }

                // If the submitted product ID is unique, add the new product to the database and redirect to the Products action with a success message
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                ViewBag.success = true;

                return RedirectToAction(nameof(Products));
            }

            // If the submitted product data is not valid, repopulate the dropdown lists in the form
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Earrings").ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }

        // Action to display all necklaces
        public IActionResult Necklaces()
        {
            // Retrieve all products that belong to the "Necklace" product type, and include the related product type entity
            var products = _dbContext.Products.Include(p => p.ProdType).Where(p => p.ProdType.ProdTypeName == "Necklace").ToList();
            return View(products);
        }

        // Action to display the form for adding necklaces, only accessible to users with the "Admin" role
        [Authorize(Roles = "Admin")]
        public IActionResult AddNecklaces()
        {
            // Prepare the data needed for the view
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Necklace").ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNecklaces(Product product)
        {
            if (ModelState.IsValid)
            {
                // Check if a product with the same ID already exists in the database
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null)
                {
                    // If the product ID already exists, add an error message to the model state and return the view with the same product data
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Necklace").ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product);
                }

                // If the product is valid and doesn't already exist, add it to the database and redirect to the Products page
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Products));
            }
            // If the product data is invalid, return the view with the same product data and the necessary data for the dropdowns
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Necklace").ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }

        // This method retrieves all products with the product type "Rings" from the database and displays them on the Rings view.
        public IActionResult Rings()
        {
            var products = _dbContext.Products.Include(p => p.ProdType).Where(p => p.ProdType.ProdTypeName == "Rings").ToList();
            return View(products);
        }

        // This method is only accessible by users with the "Admin" role and displays a form for adding new Rings products to the database.
        [Authorize(Roles = "Admin")]
        public IActionResult AddRings()
        {
            // Set the ViewBag properties for CategoryId and ProdTypeId to populate the drop-down lists in the AddRings view.
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Rings").ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        // This method is used to handle the form submission when a new Rings product is added to the database.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRings(Product product)
        {
            if (ModelState.IsValid) // Check if the model state is valid.
            {
                // Check if a product with the same ID already exists in the database.
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null)
                {
                    // Add a model error message to the ModelState dictionary if a product with the same ID already exists.
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    // Set the ViewBag properties for CategoryId and ProdTypeId to populate the drop-down lists in the AddRings view.
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Rings").ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product);
                }
                // Add the new product to the database and save changes.
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                // Redirect to the Products action to display all products.
                return RedirectToAction(nameof(Products));
            }
            // Set the ViewBag properties for CategoryId and ProdTypeId to populate the drop-down lists in the AddRings view.
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.Where(p => p.ProdTypeName == "Rings").ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }
    }
}
