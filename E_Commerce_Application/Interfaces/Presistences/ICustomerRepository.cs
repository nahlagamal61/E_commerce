namespace E_Commerce_Application.Interfaces.Presistences
{
    using E_commerce_DataModeling.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> GetCustomerById(Guid guid);
        Task<List<Customer>> GetCustomers();
    }
}

