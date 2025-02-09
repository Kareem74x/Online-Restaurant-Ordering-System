using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrderingSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; } //IFormFile is an interface in ASP.NET Core that represents a file 
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150";


        public int CategoryId { get; set; }

        [ValidateNever] // Prevents ASP.NET Core from validating this property
        public Category? Category { get; set; } //A product belongs to a category



        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; } //A product can be in many order items


        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } //A product can have many ingredients
    }
}