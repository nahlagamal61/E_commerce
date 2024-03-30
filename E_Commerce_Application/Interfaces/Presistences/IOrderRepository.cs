namespace E_Commerce_Application.Interfaces.Presistences
{
    using E_commerce_DataModeling.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderRepository
    {
        Task<Order> GetOrderByID(Guid id);
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrderByCustomerID(Guid id);
        Task<Order> AddOrder(Order order);
        Task<Order> UpdateOrder(Order order);
        Task<Order> CancelOrder(Order order);
        Task SaveLinesDtails(List<OrderLineDetails> lineDetails);
        Task SaveChanges();
    }
}

