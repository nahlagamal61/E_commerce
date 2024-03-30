namespace E_commerce_DataModeling.ViewModels
{
    public class Product_Create_DTO
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
    }
    
}
