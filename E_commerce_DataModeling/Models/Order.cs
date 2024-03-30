namespace E_commerce_DataModeling.Models
{
    using E_commerce_DataModeling.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public Double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderLineDetails> OrderLineDetails { get; set; }
        public Customer Customer { get; set; }


    }

}
