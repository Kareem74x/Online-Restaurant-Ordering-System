using Microsoft.AspNetCore.Mvc;
using RestaurantOrderingSystem.Data;
using RestaurantOrderingSystem.Models;

namespace RestaurantOrderingSystem.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> prodcuts;

        public ProductController(ApplicationDbContext context)
        {
            prodcuts = new Repository<Product>(context);
        }


        public async Task<IActionResult> Index()
        {
            return View(await prodcuts.GetAllAsync());
        }
    }
}
