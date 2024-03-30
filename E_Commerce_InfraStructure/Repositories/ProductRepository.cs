namespace E_commerce_Task.Repositories
{
    using E_Commerce_Application.Interfaces.Presistences;
    using E_commerce_DataModeling.Context;
    using E_commerce_DataModeling.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
 
        public async Task<Product> GetProductByID(Guid id)
        {
            var result = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductID == id);
            return result;
        }
        public async Task<Product> AddProduct(Product product )
        {
            var result = await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Product> UpdateProduct(Guid productId, Product product)
        {
            var updatedProduct = _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return updatedProduct.Entity;
        }

        public async Task<List<Product>> GetProducts()
        {
            var result = await _dbContext.Products.ToListAsync();
            return result;
        }
    }
}
