namespace E_commerce_Task.Controllers
{
    using AutoMapper;
    using E_Commerce_Application.Interfaces.Services;
    using E_commerce_DataModeling.Enums;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using E_commerce_Task.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var result =await _service.GetOrder();
                return Ok(new APIResponse<List<Order_Response>>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (Exception ex)
            {
                return BadRequest( new APIResponse<List<Order_Response>>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status400BadRequest,
                    Message = "An error occurred while getting Orders. Please try again later.",
                    Errors = new List<string>() { ex.Message }
                });
            }
        }
        [HttpGet("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrderById( Guid orderId ) {
            try
            {
                var result = await _service.GetOrderByID(orderId);
                return Ok(new APIResponse<Order_Response>() { 
                    Success = true, 
                    ResponseCode = StatusCodes.Status200OK, 
                    Result = result });
  
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Product_Response>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status400BadRequest,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string>(){ex.Message}
                });
            }
        }

        [HttpGet("GetOrderforCustomer/{CustomerId}")]
        public async Task<IActionResult> GetOrderforCustomer(Guid CustomerId)
        {
            try
            {
                var result =await _service.GetOrderByCustomerID(CustomerId);
                return Ok(new APIResponse<List<Order_Response>>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });

            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status400BadRequest,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string>() { ex.Message }
                });
            }
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreate_VM order) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new APIResponse<Product>()
                    {
                        Success = false,
                        ResponseCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid input data. Please check the provided information.",
                        Result = null,
                        Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                    });
                }
                var result = await _service.AddOrder(order);
                return Ok(new APIResponse<Order_Response>() { 
                    Success = true, 
                    ResponseCode = StatusCodes.Status200OK, 
                    Result = result 
                });
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
        [HttpPut("UpdateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus( Guid orderId ,[FromQuery] OrderStatus orderStatus )
        {
            try
            {

                var result = await _service.UpdateOrder(orderId, orderStatus);
                return Ok(new APIResponse<Order_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
           
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status400BadRequest,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors  = new List<string>() { ex.Message }
                }) ;
            }
        }
        [HttpPut("CancelOrder/{orderId}")]
        public IActionResult CancelOrderById( Guid orderId)
        {
            try
            {
                var result = _service.CancelOrder(orderId).Result;
                return Ok(new APIResponse<Order_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
               
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Product>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status400BadRequest,
                    Message = "An unexpected error occurred. Please try again later.",
                    Errors = new List<string>() { ex.Message }
                });
            }
        }


    }
}
