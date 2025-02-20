using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderingSystem.Data;
using RestaurantOrderingSystem.Models;

namespace RestaurantOrderingSystem.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment; 
        }


        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
 
            if (id == 0)
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product> { Includes = "ProductIngredients.Ingredient, Category" });

                ViewBag.Operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEdit(Product product, int[] IngredientIds, int CatID)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrl = uniqueFileName;
                }

                if(product.ProductId == 0) // Add Operation
                {
                    ViewBag.Ingredients = await ingredients.GetAllAsync();
                    ViewBag.Categories = await categories.GetAllAsync();
                    product.CategoryId = CatID;


                    //Add Ingredients
                    foreach (int id in IngredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }


                    await products.AddAsync(product);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product> { Includes = "ProductIngredients" });

                    if(existingProduct== null)
                    {

                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = CatID;


                    //Update Ingredients
                    existingProduct.ProductIngredients?.Clear();
                    foreach(int id in IngredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }


                    try
                    {
                        await products.UpdateAsync(existingProduct);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return Redirect("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Product not found.");
                return RedirectToAction("Index");
            }
        }
    }
}
