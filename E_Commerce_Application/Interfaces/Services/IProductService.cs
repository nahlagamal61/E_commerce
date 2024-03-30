namespace E_Commerce_Application.Interfaces.Services
{
    using E_commerce_DataModeling.ViewModels;

    public interface IProductService
    {
        Task<Product_Response> GetProductByID(Guid id);
        Task<List<Product_Response>> GetProducts();
        Task<Product_Response> AddProduct(Product_Create_DTO productDto);
        Task<Product_Response> UpdateProduct(Guid productId, UpdateProduct_VM productDto);
    }
}
