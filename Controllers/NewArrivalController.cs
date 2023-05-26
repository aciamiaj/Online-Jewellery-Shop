using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineJewelleryShop.Models;
using System;
using System.Security.Claims;
using System.Text;

namespace OnlineJewelleryShop.Controllers
{
    public class NewArrivalController : Controller
    {
        private readonly JjewelleryContext _dbContext;

        public NewArrivalController(JjewelleryContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Displays a page of new arrival products
        public IActionResult NewArrival(int page = 1, int pageSize = 4)
        {
            // Get all products that have a status of "1" (i.e. are active)
            var products = (from p in _dbContext.Products
                            join pd in _dbContext.ProductDetails on p.ProductId equals pd.ProductId
                            where pd.StatusId == "1"
                            orderby p.ProductName
                            select p).ToList();
            // Calculate the total number of products and the number of pages required to display them
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            // Calculate the starting index of the products to display on the current page
            var skip = (page - 1) * pageSize;
            // Get the products to display on the current page
            var productsPage = products.Skip(skip).Take(pageSize).ToList();
            // Set viewbag variables for the current page number and total number of pages
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            // Return the view with the products to display on the current page
            return View(productsPage);
        }

        public IActionResult ViewAllProducts(int page = 1, int pageSize = 4)
        {
            // Get all products and order them by name
            var products = (from p in _dbContext.Products
                            join pd in _dbContext.ProductDetails on p.ProductId equals pd.ProductId
                            orderby p.ProductName
                            select p).ToList();
            // Calculate the total number of products and the total number of pages
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            // Calculate the number of products to skip and take based on the page number and page size
            var skip = (page - 1) * pageSize;
            var productsPage = products.Skip(skip).Take(pageSize).ToList();
            // Set ViewBag properties for pagination
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            // Return the view with the list of products for the specified page
            return View(productsPage);
        }
        
        // This action method requires authentication to access it
        [Authorize]
        // This route specifies the URL pattern for adding a product to the cart
        [Route("/NewArrival/AddToCart/{id}")]
        public IActionResult AddToCart(string id)
        {
            // This route specifies the URL pattern for adding a product to the cart
            string shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4);
            Guid guid = Guid.NewGuid();
            int cartId = guid.GetHashCode();
            // Get the product details for the specified product ID
            var product = _dbContext.Products
                .Include(p => p.ProductDetails)
                .FirstOrDefault(p => p.ProductId == id);
            // Get the user ID from the authentication token
            var userId = User.FindFirstValue(ClaimTypes.Name);
            // Get the existing cart for the current user
            var existingCart = _dbContext.Carts.Where(c => c.UserId == userId);
            // Create a new cart item and update the product details if the product is not already in the cart
            Cart newCart = new Cart();
            var existingProduct = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == id);
            var productDetail = _dbContext.ProductDetails.FirstOrDefault(pd => pd.ProductId == product.ProductId);

            if (existingProduct != null)
            {
                // If the product is already in the cart, increase the quantity and total price by 1
                existingProduct.Quantity += 1; // increase the quantity by 1
                existingProduct.Total += product.Price; // increase the total price by the price of one item

                // Update the available quantity of the product in the product details table
                if (productDetail != null)
                {
                    productDetail.AvailQty -= 1; // update based on quantity added to cart
                }
            }

            else
            {
                // If the product is not already in the cart, create a new cart item with a quantity of 1 and a total price equal to the price of one item
                newCart.CartId = shortGuid;
                newCart.CartOrderid = cartId;
                newCart.ProductId = product.ProductId;
                newCart.UserId = userId;
                newCart.Quantity = 1; // set the quantity to 1 by default
                newCart.Total = product.Price; // set the total price to the price of one item
                // Update the available quantity of the product in the product details table
                if (productDetail != null)
                {
                    productDetail.AvailQty -= newCart.Quantity; // deduct 1 from AvailQty
                }
                // Add the new cart item to the database
                _dbContext.Carts.Add(newCart);
            }

            // Save changes to the database
            _dbContext.SaveChanges();
            // Redirect to the view cart page
            return RedirectToAction("ViewCart");
        }

        [Authorize]
        public IActionResult ViewCart()
        {
            // Get the user ID of the current user
            var userId = User.FindFirstValue(ClaimTypes.Name);
            // Retrieve the user's existing cart items from the database
            var existingCart = _dbContext.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();

            // Retrieve the user's existing cart items from the database
            return View("AddToCart", existingCart);
        }
        [Authorize]
        private Cart GetCart()
        {
            // Generate a new cart ID or retrieve an existing one from the session
            var cartId = HttpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();
            // Store the cart ID in the session
            HttpContext.Session.SetString("CartId", cartId);
            // Retrieve the user's cart from the database
            var cart = _dbContext.Carts
                    .Include(ci => ci.Product)
                .FirstOrDefault(c => c.CartId == cartId);
            // If no cart was found, create a new one and add it to the database
            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = cartId,
                    UserId = User.Identity?.Name
                };
                _dbContext.Carts.Add(cart);
            }
            // Return the user's cart
            return cart;
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateQuantity(string ProductId, string quantityChange)
        {
            // Get the user ID of the current user
            var userId = User.FindFirstValue(ClaimTypes.Name);
            // Retrieve the user's cart and the product being updated from the database
            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == ProductId);
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == ProductId);
            var productDetail = _dbContext.ProductDetails.FirstOrDefault(pd => pd.ProductId == ProductId);

            if (cart != null)
            {
                // Calculate the new quantity of the product in the cart
                int? newQuantity = cart.Quantity;

                if (quantityChange == "-")
                {
                    newQuantity--;
                }
                else if (quantityChange == "+")
                {
                    newQuantity++;
                }
                // If the new quantity is valid, update the cart and the product details
                if (productDetail != null && newQuantity >= 0 && productDetail.AvailQty >= newQuantity)
                {
                    productDetail.AvailQty -= (newQuantity - cart.Quantity);
                    cart.Quantity = newQuantity;
                    cart.Total = newQuantity * product.Price;
                    _dbContext.SaveChanges();
                }
            }
            // Redirect the user back to their cart
            return RedirectToAction("ViewCart");
        }

        [Authorize]
        public IActionResult Remove(string id)
        {
            // Get the user ID of the current user
            var userId = User.FindFirstValue(ClaimTypes.Name);
            // Find the cart item associated with the user and the given ID
            var cartItem = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.CartId == id);

            if (cartItem != null)
            {
                // Remove the cart item from the database
                _dbContext.Carts.Remove(cartItem);
                _dbContext.SaveChanges();
            }
            // Redirect the user back to their cart
            return RedirectToAction("ViewCart");
        }



        public IActionResult Products(string pname, decimal? minPrice, decimal? maxPrice, string sortBy, string sortOrder, int page = 1, int pageSize = 10)
        {
            // Get all products from the database
            IQueryable<Product> products = _dbContext.Products;

            // Filter by product name
            if (!string.IsNullOrEmpty(pname))
            {
                products = products.Where(p => p.ProductName.Contains(pname));
            }
            // Filter by min and max price
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // Sort by selected column and order
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "productname":
                        products = sortOrder.ToLower() == "asc" ? products.OrderBy(p => p.ProductName) : products.OrderByDescending(p => p.ProductName);
                        break;
                    case "productdescription":
                        products = sortOrder.ToLower() == "asc" ? products.OrderBy(p => p.ProductDescription) : products.OrderByDescending(p => p.ProductDescription);
                        break;
                    case "productprice":
                        products = sortOrder.ToLower() == "asc" ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                        break;
                }
            }

            // Paginate the result set
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            int skip = (page - 1) * pageSize;
            products = products.Skip(skip).Take(pageSize);
            // Pass data to the view
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            // Render the view with the filtered and sorted products
            return View(products.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory()
        {
            // Render the view for adding a new category
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                // Check if a category with the same ID already exists
                var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);

                if (existingCategory != null)
                {
                    // Report an error if the category ID is already taken
                    ModelState.AddModelError(nameof(category.CategoryId), $"Category ID {category.CategoryId} already exists.");
                    return View(category);
                }
                // Add the new category to the database
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                // Redirect to the list of products
                return RedirectToAction(nameof(Products));
            }
            // If the model state is not valid, render the view with the validation errors
            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct()
        {
            // Get the list of categories and product types and add them to the view bag
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                // Check if the product already exists in the database
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null)
                {
                    // If the product already exists, add an error message to the model state and return to the view with the same product object and view bag values
                    ModelState.AddModelError(nameof(product.ProductId), $"Product ID {product.ProductId} already exists.");
                    ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
                    ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
                    return View(product);
                }
                // Add the new product to the database and redirect to the product list page
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Products));
            }
            // If the model state is not valid, return to the view with the same product object and view bag values
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProductType()
        {
            // Display the add product type view
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProductType(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                // Check if the product type already exists in the database
                var existingProductType = _dbContext.ProductTypes.FirstOrDefault(pt => pt.ProdTypeId == productType.ProdTypeId);

                if (existingProductType != null)
                {
                    // If the product type already exists, add an error message to the model state and return to the view with the same product type object
                    ModelState.AddModelError(nameof(productType.ProdTypeId), $"Product Type ID {productType.ProdTypeId} already exists.");
                    return View(productType);
                }
                // Add the new product type to the database and redirect to the product list page
                _dbContext.ProductTypes.Add(productType);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Products));
            }
            // If the model state is not valid, return to the view with the same product type object
            return View(productType);
        }

        // Edit action
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            // Retrieve the product by id from the database
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                // If the product is not found, return a 404 error
                return NotFound();
            }

            // Pass the product to the view for editing
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/NewArrival/Edit/{id}")]
        public IActionResult Edit(string id, Product product)
        {
            // Retrieve the product by id from the database
            var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                // If the product is not found, return a 404 error
                return NotFound();
            }

            // Update the existing product with the new values
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductDescription = product.ProductDescription;

            // Check if the price has changed
            if (existingProduct.Price != product.Price)
            {
                // Update the product price in the Products table
                existingProduct.Price = product.Price;

                // Update the product price in the ProductDetails table
                var productDetails = _dbContext.ProductDetails.FirstOrDefault(pd => pd.ProductId == id);

                if (productDetails != null)
                {
                    productDetails.Price = product.Price;
                }
                else
                {
                    // If there is no product details record, create one and set the price
                    var newProductDetail = new ProductDetail
                    {
                        ProductId = id,
                        Price = product.Price
                    };
                    _dbContext.ProductDetails.Add(newProductDetail);
                }
            }

            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");

            // Save changes to the database
            _dbContext.SaveChanges();

            // Redirect to the product details page
            //return RedirectToAction("Details", new { id = existingProduct.ProductId });
            return RedirectToAction("Products");
        }


        // Details action
        [Authorize(Roles = "Admin")]
        public IActionResult Details(string id)
        {
            // Retrieve the product by id from the database
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);

        if (product == null)
            {
                // If the product is not found, return a 404 error
                return NotFound();
            }

            // Pass the product to the view for display
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.ProdTypeId = new SelectList(_dbContext.ProductTypes.ToList(), "ProdTypeId", "ProdTypeName");
            return View(product);
        }

        //Delete Action
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            // Retrieve the product by id from the database
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);

        if (product == null)
            {
                // If the product is not found, return a 404 error
                return NotFound();
            }

            // Pass the product to the view for confirmation of deletion
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/NewArrival/Delete/{id}")]
        public IActionResult DeleteConfirmed(string id)
        {
            // Retrieve the product by id from the database
            //    var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);

            //if (product == null)
            //    {
            //        // If the product is not found, return a 404 error
            //        return NotFound();
            //    }

            //    // Remove the product from the database
            //    _dbContext.Products.Remove(product);
            //    _dbContext.SaveChanges();

            //    // Redirect to the product index page
            //    return RedirectToAction("Products");

            // Retrieve the product by id from the database
            var product = _dbContext.Products.Include(p => p.ProductDetails).FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                // If the product is not found, return a 404 error
                return NotFound();
            }

            // Remove all related product details from the database
            _dbContext.ProductDetails.RemoveRange(product.ProductDetails);

            // Remove the product from the database
            _dbContext.Products.Remove(product);

            // Save changes to the database
            _dbContext.SaveChanges();

            // Redirect to the product index page
            return RedirectToAction("Products");

        }

        [Authorize]
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var cartItems = _dbContext.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();

            // Calculate total cost
            decimal? totalCost = 0;
            foreach (var item in cartItems)
            {
                totalCost += item.Total;
            }

            // Remove cart items from database
            _dbContext.Carts.RemoveRange(cartItems);
            _dbContext.SaveChanges();

            // Pass order summary to the view
           // ViewBag.Message = "Thank you for your order! Here is a summary of your order:";
            ViewBag.CartItems = cartItems;
            ViewBag.TotalCost = totalCost;

            return View();
        }

    }
}