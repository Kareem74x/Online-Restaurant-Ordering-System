using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderingSystem.Data;
using RestaurantOrderingSystem.Models;
using TequliasRestaurant.Models;

namespace RestaurantOrderingSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Repository<Product> _products;
        private Repository<Order> _orders;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _products = new Repository<Product>(context);
            _orders = new Repository<Order>(context);
        }



        [Authorize] //Make sure that user is logged in
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem(int prodId,int ProdQty)
        {
            var product = await _context.Products.FindAsync(prodId);

            if (product == null)
            {
                return NotFound();
            }

            // Retrieve or create new OrderViewModel from session or other state mangaement
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };

            // Check if the product is already in the order, update quantity
            var existingItem = model.OrderItems.FirstOrDefault(oi => oi.ProductId == prodId);

            // if the product already exist , Update the quantity
            if(existingItem !=null)
            {
                existingItem.Quantity += ProdQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Quantity = ProdQty,
                    ProductName = product.Name
                });
            }


            model.TotalAmount = model.OrderItems.Sum(oi => oi.Quantity * oi.Price);

            // Save updated OrderViewModel to session
            HttpContext.Session.Set("OrderViewModel", model);

            // Redirect back to Create to show updated order items
            return RedirectToAction("Create",model);
        }
    }
}
