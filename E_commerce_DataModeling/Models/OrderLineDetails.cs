namespace E_commerce_DataModeling.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class OrderLineDetails
    {
        [Key]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Order ID is required")]
        public Guid OrderID { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        public double TotalLineCost { get; set; }

        public Guid ProductID { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
