namespace E_Commerce_Application.Interfaces.Services
{
    using E_commerce_DataModeling.Enums;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;

    public interface IOrderService
    {
        Task<Order_Response> GetOrderByID(Guid id);
        Task<List<Order_Response>> GetOrder();
        Task<List<Order_Response>> GetOrderByCustomerID(Guid id);
        Task<Order_Response> AddOrder(OrderCreate_VM orderVM);
        //Task<List<String>> CheckproductExist(List<OrderLineDetails> lineDetails);
        Task<Order_Response> UpdateOrder(Guid OrderId, OrderStatus status);
        Task<Order_Response> CancelOrder(Guid orderId);
    }
}
