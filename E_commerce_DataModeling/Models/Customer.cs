namespace E_commerce_DataModeling.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class Customer
    {
        public Guid CustomerId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; }

    }
}
