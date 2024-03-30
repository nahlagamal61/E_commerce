namespace E_commerce_DataModeling.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Quantity InStock is required")]
        public int StockQuantity { get; set; }

    }
}
