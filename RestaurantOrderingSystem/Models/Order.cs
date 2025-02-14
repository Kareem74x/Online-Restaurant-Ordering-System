namespace RestaurantOrderingSystem.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }





        // Foreign Key (every order has 1 user)
        public string? UserId { get; set; }
        //Navigation Property
        public ApplicationUser User { get; set; }
    }
}
