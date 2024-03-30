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

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper mapper;

        public CustomerService(ICustomerRepository repository , IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }
        public async Task<CustomerDTO_Response> CreateCustomer(CustomerDTO customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            await _repository.CreateCustomer(customer);
            return mapper.Map<CustomerDTO_Response>(customer);
        }

        public async Task<List<CustomerDTO_Response>> GetAllCustomer()
        {
            var result = await _repository.GetCustomers();
            return mapper.Map<List<CustomerDTO_Response>>(result);
        }

        public async Task<CustomerDTO_Response> GetCustomerByID(Guid customerId)
        {
            var result = await _repository.GetCustomerById(customerId);
            if (result == null)
            {
                throw new Exception("this customer with GUID not Exist");
            }
            return mapper.Map<CustomerDTO_Response>(result);
        }
    }
}
