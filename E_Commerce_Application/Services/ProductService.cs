namespace E_Commerce_Application.Services
{
    using AutoMapper;
    using E_Commerce_Application.Interfaces.Presistences;
    using E_Commerce_Application.Interfaces.Services;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository  , IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Product_Response> AddProduct(Product_Create_DTO productDto)
        {
             var product = _mapper.Map<Product>(productDto);
            var result =await _repository.AddProduct(product);
            return _mapper.Map<Product_Response>(result);
        }

        public async Task<Product_Response> GetProductByID(Guid id)
        {
            var result = await _repository.GetProductByID(id);
            if (result == null)
                throw new Exception("there isno product with this Guid ");
            return _mapper.Map<Product_Response>(result);
        }

        public async Task<List<Product_Response>> GetProducts()
        {
            var result = await _repository.GetProducts();
            return _mapper.Map<List<Product_Response>>(result);
        }

        public async Task<Product_Response> UpdateProduct(Guid productId, UpdateProduct_VM productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.ProductID = productId;
            await GetProductByID(productId);
            var result = await _repository.UpdateProduct(productId , product);
            return _mapper.Map<Product_Response>(result);
        }


    }
}
