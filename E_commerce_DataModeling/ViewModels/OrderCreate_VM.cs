namespace E_commerce_DataModeling.ViewModels
{
    public class OrderCreate_VM
    {
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }


        public List<OrderLineDetails_VM> OrderLineDetails { get; set; } = new List<OrderLineDetails_VM>();
    }
}
