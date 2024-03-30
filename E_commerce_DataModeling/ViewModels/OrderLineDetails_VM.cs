namespace E_commerce_DataModeling.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class OrderLineDetails_VM
    {
        [Required(ErrorMessage = "Product is required")]
        public Guid ProductID { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    } 

}
