using E_commerce_DataModeling.Enums;
using E_commerce_DataModeling.Models;
using E_commerce_DataModeling.ViewModels;

namespace E_commerce_DataModeling.ViewModels
{
    using E_commerce_DataModeling.Enums;
    using E_commerce_DataModeling.Models;

    public class Order_Response
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public Double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderLineDetails_Response> OrderLineDetails { get; set; }
        public Customer Customer { get; set; }
    }
}

