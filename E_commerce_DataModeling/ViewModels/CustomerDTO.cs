namespace E_commerce_DataModeling.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CustomerDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
