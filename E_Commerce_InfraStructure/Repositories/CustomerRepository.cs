namespace E_commerce_Task.Repositories
{
    using E_Commerce_Application.Interfaces.Presistences;
    using E_commerce_DataModeling.Context;
    using E_commerce_DataModeling.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var result = await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Customer> GetCustomerById(Guid guid)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == guid);
            return customer;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _dbContext.Customers.Include(x => x.Orders).ToListAsync();
        }


    }
}
