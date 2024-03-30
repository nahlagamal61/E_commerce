namespace E_commerce_Task.Repositories
{
    using E_Commerce_Application.Interfaces.Presistences;
    using E_commerce_DataModeling.Context;
    using E_commerce_DataModeling.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  async Task<List<Order>> GetOrders()
        {
            return await _dbContext.Orders
                .Include(x => x.OrderLineDetails).ThenInclude(x => x.Product)
                .Include(c => c.Customer).ToListAsync();
        }

        public async Task<Order> GetOrderByID(Guid id)
        {
            var result =await _dbContext.Orders
                            .Include( x=> x.Customer)
                            .Include(x => x.OrderLineDetails).ThenInclude(x => x.Product)
                            .FirstOrDefaultAsync(x => x.OrderId ==id);
            return result;
        }
        public async Task<List<Order>> GetOrderByCustomerID(Guid id)
        {
            var result = await _dbContext.Orders
                            .Include(o => o.OrderLineDetails)
                            .ThenInclude(x => x.Product)
                            .Where(o => o.CustomerId == id)
                            .ToListAsync(); 
            return result;
        }
  
        public async Task<Order> AddOrder(Order order)
        {
            var result = await _dbContext.Orders.AddAsync(order);
            return result.Entity;
        }


        public async Task<Order> UpdateOrder(Order order)
        {
           var updatedOrder = _dbContext.Orders.Update(order);

            return updatedOrder.Entity;
        }

        public async Task<Order> CancelOrder(Order order)
        {
            var cancelledOrder =  _dbContext.Orders.Update(order);
            return cancelledOrder.Entity;
        }
        

        public async Task SaveLinesDtails(List<OrderLineDetails> lineDetails )
        {
            await _dbContext.OrderLineDetails.AddRangeAsync(lineDetails);   
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
