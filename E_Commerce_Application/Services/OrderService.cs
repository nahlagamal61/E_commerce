namespace E_Commerce_Application.Services
{
    using AutoMapper;
    using E_Commerce_Application.Interfaces.Presistences;
    using E_Commerce_Application.Interfaces.Services;
    using E_commerce_DataModeling.Enums;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repo;
        private readonly ICustomerRepository _customer;
        private readonly IProductRepository _productRepo;

        public OrderService(IMapper mapper, IOrderRepository repo , ICustomerRepository customer , IProductRepository productRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _customer = customer;
            _productRepo = productRepo;
        }


        public async Task<Order_Response> AddOrder(OrderCreate_VM orderVM )
        {
            var order = _mapper.Map<Order>(orderVM);
            if (order.OrderLineDetails == null || order.OrderLineDetails.Count ==0)
                throw new Exception("at least add one product to Order");
            await CheckproductExist(order.OrderLineDetails);
            order.Status = OrderStatus.pending;
            order.TotalAmount = ClaculateOrderCost(order.OrderLineDetails).Result;
            var result = await _repo.AddOrder(order);
            await SaveLinesDtails(result.OrderLineDetails , result.OrderId);
            await _repo.SaveChanges();
            return _mapper.Map<Order_Response>(result);
        }

        public async Task<Order_Response> CancelOrder(Guid orderId)
        {
            var order =await GetOrderById(orderId);
            order.Status = OrderStatus.cancelled;
            await ReturnQuntityToStck(order.OrderLineDetails);
           var result = await _repo.CancelOrder(order);
            await _repo.SaveChanges();
            return _mapper.Map<Order_Response>(result);
        }
        public async Task<List<Order_Response>> GetOrder()
        {
            var  result = await _repo.GetOrders();
            return _mapper.Map<List<Order_Response>>(result);
        }

        public async Task<List<Order_Response>> GetOrderByCustomerID(Guid id)
        {
            var customer = await _customer.GetCustomerById(id);
            if (customer == null)
                throw new Exception("this customer with Id Not exist");
            var order =await _repo.GetOrderByCustomerID(id);
            return _mapper.Map<List<Order_Response>>(order);

        }
        public async Task<Order_Response> GetOrderByID(Guid id)
        {
            var order =await _repo.GetOrderByID(id);
            if (order == null)
                throw new Exception("this customer with Id Not exist");
            return _mapper.Map<Order_Response>(order);
        }

        public async Task<Order_Response> UpdateOrder(Guid OrderId, OrderStatus status)
        {
            var order =await _repo.GetOrderByID(OrderId);
            if (order == null)
                throw new Exception("Not Exist Order with this ID");
            order.Status = status;
            var result = await _repo.UpdateOrder(order);
            await _repo.SaveChanges();
            return _mapper.Map<Order_Response>(result);
        }
        private async Task<double> ClaculateOrderCost(List<OrderLineDetails> lineDetails)
        {
            double totalCost = 0;
            foreach (var line in lineDetails)
            {
                var product = await _productRepo.GetProductByID(line.ProductID);
                if (product != null)
                    totalCost += (product.Price * line.Quantity);
            }
            return totalCost;
        }

        private  async Task SaveLinesDtails(List<OrderLineDetails> lineDetails, Guid orderId)
        {
            foreach (var line in lineDetails)
            {
                var quantity = line.Quantity == 0 ? line.Quantity : 1;
                line.TotalLineCost = (line.Product.Price * quantity);
                line.OrderID = orderId;
            }
            await _repo.SaveLinesDtails(lineDetails);
        }
        private async Task CheckproductExist(List<OrderLineDetails> lineDetails)
        {
            List<string> productErrormassage = new();
            foreach (var line in lineDetails)
            {
                string errorMassage = string.Empty;
                var product = await _productRepo.GetProductByID(line.ProductID);
             
                if ((product != null && (line.Quantity == 0 || line.Quantity > product.StockQuantity)))
                {
                    errorMassage = line.ProductID.ToString();
                    errorMassage += ", this product quantity not valid on stock";
                    productErrormassage.Add(errorMassage);
                }
                else if (product == null)
                {
                    errorMassage = line.ProductID.ToString();
                    productErrormassage.Add(errorMassage + " not found ");
                }

            }
            if(productErrormassage.Count > 0)
            {
                throw new Exception("This products with issues :" + productErrormassage);
            }

        }

        private async void handelUpdatedOrder (Order order)
        {

            if (order.Status == OrderStatus.delivered)
            {
                foreach (var lineItem in order.OrderLineDetails)
                {
                    var product =await _productRepo.GetProductByID (lineItem.ProductID);
                    if (product != null)
                    {
                        product.StockQuantity -= lineItem.Quantity;
                        await _productRepo.UpdateProduct(product.ProductID , product);
                    }
                }
            }
        }
        private async Task<Order> GetOrderById(Guid guid)
        {
            var order = await _repo.GetOrderByID(guid);
            if (order == null)
                throw new Exception("this customer with Id Not exist");
            return order;
        }
        private async Task ReturnQuntityToStck(List<OrderLineDetails> orderLine)
        {
            foreach (var lineItem in orderLine)
            {
                var product =await _productRepo.GetProductByID(lineItem.ProductID);
                if (product != null)
                {
                    product.StockQuantity += lineItem.Quantity;
                    await _productRepo.UpdateProduct(product.ProductID, product);
                }
            }
        }
    }
}
