namespace E_commerce_Task.Controllers
{
    using E_Commerce_Application.Interfaces.Services;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service  )
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
            try
            {
                var result = await _service.CreateCustomer(customer);
                return Ok(new APIResponse<CustomerDTO_Response>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<Customer>() {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while getting Orders. Please try again later.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("GetCsustomer")]
        public async Task<IActionResult> GetCsustomer()
        {
            try
            {
                var result = await _service.GetAllCustomer();
                return Ok(new APIResponse<List<CustomerDTO_Response>>() { Success = true, ResponseCode = StatusCodes.Status200OK, Result = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse<List<Product>>()
                {
                    Success = false,
                    ResponseCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while getting Orders. Please try again later.",
                    Errors = new List<string> { ex.Message }
                   
                });
            }
        }

    }
}
