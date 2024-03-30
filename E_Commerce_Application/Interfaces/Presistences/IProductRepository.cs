namespace E_Commerce_Application.Interfaces.Presistences
{
    using E_commerce_DataModeling.Models;
    using System;
    using System.Threading.Tasks;

    public interface IProductRepository
    {
        Task<Product> GetProductByID(Guid id);
        Task<List<Product>> GetProducts();
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Guid productId, Product product);
    }
}
