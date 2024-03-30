namespace E_commerce_Task.Controllers
{
    using E_Commerce_Application.Interfaces.Services;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public  async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _service.GetProducts();
                return Ok(new APIResponse<List<Product_Response>>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result  });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<Product>>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while getting products. Please try again later.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
        [HttpGet("GetProduct/{ProductId}")]
        public async Task<IActionResult> GetProductsByID(Guid ProductId)
        {
            try
            {
                var result = await _service.GetProductByID(ProductId);
               
                return Ok(new APIResponse<Product_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<Product>>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while getting products. Please try again later.",
                    Errors = new List<string> {ex.Message }
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product_Create_DTO product)
        {
            try
            {
                var result = await _service.AddProduct(product);
                return Ok(new APIResponse<Product_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (DbUpdateException ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while adding the product. Please try again later.",
                    Errors= new List<string>() {ex.Message }
                });
            }
            catch (Exception ex )
            { 
                return BadRequest( new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string>() { ex.Message }

                });
            }
        }

        [HttpPut("{ProductId}")]
        public async Task<IActionResult> UpdateProduct( Guid ProductId , UpdateProduct_VM product)
        {
            try
            {

                var result = await _service.UpdateProduct(ProductId, product);
                return Ok(new APIResponse<Product_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string>() { ex.Message }
                });
            }
        }


    }
}
