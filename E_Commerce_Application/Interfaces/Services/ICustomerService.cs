namespace E_Commerce_Application.Interfaces.Services
{
    using E_commerce_DataModeling.ViewModels;

    public interface ICustomerService
    {
        Task<List<CustomerDTO_Response>> GetAllCustomer();
        Task<CustomerDTO_Response> GetCustomerByID(Guid customerId );
        Task<CustomerDTO_Response> CreateCustomer(CustomerDTO customerDto);
        

    }
}
