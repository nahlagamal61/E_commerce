namespace E_Commerce_Test.Services
{
    using AutoMapper;
    using E_Commerce_Application.Interfaces.Presistences;
    using E_Commerce_Application.Interfaces.Services;
    using E_Commerce_Application.Services;
    using E_commerce_DataModeling.Enums;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using Moq;
    using System.Threading.Tasks;

    public class OrderServiceTest
    {
        public Mock<IMapper> mapperMock { get; set; }
        public Mock<IOrderRepository> orderRepoMock { get; set; }
        public Mock<ICustomerRepository> customerRepoMock { get; set; }
        public Mock<IProductRepository> productRepoMock { get; set; }
        public OrderService orderService { get; set; }
        public OrderServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            orderRepoMock = new Mock<IOrderRepository>();
            customerRepoMock = new Mock<ICustomerRepository>();
            productRepoMock = new Mock<IProductRepository>();
            orderService = new OrderService(mapperMock.Object, orderRepoMock.Object, customerRepoMock.Object, productRepoMock.Object);

        }
        [Fact]
        public async Task AddOrder_WithValidInput_ShouldAddOrder()
        {
            // Arrange
           var products =  await AddProdcut();
            var orderVM = new OrderCreate_VM() { 
                OrderDate = new DateTime(),
                OrderLineDetails =  {
                    new OrderLineDetails_VM()
                    {
                        ProductID = new Guid(),
                        Quantity = 5
                    }
                }
            };
            var order = new Order()
            {
                OrderId = new Guid(),
                OrderDate = new DateTime(),
                OrderLineDetails = new List<OrderLineDetails>
                {
                    new OrderLineDetails()
                    {
                        Quantity = 5,
                        ProductID = new Guid(), 
                        Product = products[0]
                    }
                }
            };
            var orderResponse = new Order_Response()
            {
                OrderId = new Guid(),
                OrderDate = new DateTime(),
                OrderLineDetails = new List<OrderLineDetails_Response>
                {
                    new OrderLineDetails_Response()
                    {
                        Quantity = 5,
                        ProductID = new Guid(),
                        Name = products[0].Name , 
                        Description = products[0].Description ,
                        Price = products[0].Price ,

                    }
                }
            };
            orderRepoMock.Setup(repo => repo.AddOrder(order))
                .ReturnsAsync(order);
            mapperMock.Setup(mapper => mapper.Map<Order>(It.IsAny<OrderCreate_VM>()))
                .Returns(order);
            mapperMock.Setup(mapper => mapper.Map<Order_Response>(It.IsAny<Order>()))
                .Returns(orderResponse);
            productRepoMock.Setup(repo => repo.GetProductByID(new Guid()))
                .ReturnsAsync(products[0]);
            orderRepoMock.Setup(x => x.SaveChanges());
            // Act
            var result = await orderService.AddOrder(orderVM);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.pending, result.Status);
      
        }


        [Fact]
        public async Task AddOrder_WithouProductLine_returnError()
        {
            // Arrange
            var products = await AddProdcut();
            var orderVM = new OrderCreate_VM()
            {
                OrderDate = new DateTime(),
                OrderLineDetails =  {}
            };
            var order = new Order()
            {
                OrderId = new Guid(),
                OrderDate = new DateTime(),
                OrderLineDetails = new List<OrderLineDetails>{}
            };
            var orderResponse = new Order_Response()
            {
                OrderId = new Guid(),
                OrderDate = new DateTime(),
                OrderLineDetails = new List<OrderLineDetails_Response>{}
            };

            mapperMock.Setup(mapper => mapper.Map<Order>(It.IsAny<OrderCreate_VM>()))
                .Throws(new Exception());
     
            productRepoMock.Setup(repo => repo.GetProductByID(new Guid()))
                .ReturnsAsync(products[0]);
            orderRepoMock.Setup(x => x.SaveChanges());
            // Act
            await Assert.ThrowsAsync<Exception>(async () =>
                        await orderService.AddOrder(orderVM));

        }


        [Fact]
        public async Task CancelOrder_WithValidOrderId_ShouldCancelOrderAndReturnOrderResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                OrderLineDetails = new List<OrderLineDetails>(),
                Status = OrderStatus.pending
            };

            var orderResponse = new Order_Response
            {
                OrderId = orderId,
                Status = OrderStatus.cancelled
            };

            orderRepoMock.Setup(repo => repo.GetOrderByID(orderId)).ReturnsAsync(order);
            orderRepoMock.Setup(repo => repo.CancelOrder(order)).ReturnsAsync(order);
            orderRepoMock.Setup(repo => repo.SaveChanges());
            mapperMock.Setup(mapper => mapper.Map<Order_Response>(It.IsAny<Order>()))
                .Returns(orderResponse);
            // Act
            var result = await orderService.CancelOrder(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.cancelled, result.Status);
            orderRepoMock.Verify(repo => repo.CancelOrder(order), Times.Once);
            orderRepoMock.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CancelOrder_WithInValidOrderId_ThrowException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                OrderLineDetails = new List<OrderLineDetails>(),
                Status = OrderStatus.pending
            };

            var orderResponse = new Order_Response
            {
                OrderId = orderId,
                Status = OrderStatus.cancelled
            };
            productRepoMock.Setup(repo => repo.GetProductByID(new Guid())).Throws(new Exception());
            // Act
            await Assert.ThrowsAsync<Exception>(async () =>
                  await  orderService.CancelOrder(orderId));

        }

        [Fact]
        public async Task GetOrderByCustomerID_WithValidCustomerId_ShouldReturnListOfOrderResponse()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                CustomerId = customerId,
                Name = "Test Customer"
            };
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = customerId,
                    OrderLineDetails = new List<OrderLineDetails>(),
                    Status = OrderStatus.pending
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = customerId,
                    OrderLineDetails = new List<OrderLineDetails>(),
                    Status = OrderStatus.delivered
                }
            };
            var orderResponses = orders.Select(order => new Order_Response
            {
                OrderId = order.OrderId,
                Status = order.Status
            }).ToList();

            customerRepoMock.Setup(repo => repo.GetCustomerById(customerId)).ReturnsAsync(customer);
            orderRepoMock.Setup(repo => repo.GetOrderByCustomerID(customerId)).ReturnsAsync(orders);
            mapperMock.Setup(mapper => mapper.Map<List<Order_Response>>(orders)).Returns(orderResponses);

            // Act
            var result = await orderService.GetOrderByCustomerID(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orders.Count, result.Count);
            Assert.Equal(orderResponses, result);
       }

        [Fact]
        public async Task GetOrderByCustomerID_WithInvalidCustomerId_ShouldThrowException()
        {
            // Arrange
            var invalidCustomerId = Guid.NewGuid();
            customerRepoMock.Setup(repo => repo.GetCustomerById(invalidCustomerId)).ReturnsAsync((Customer)null);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await orderService.GetOrderByCustomerID(invalidCustomerId));
            customerRepoMock.Verify(repo => repo.GetCustomerById(invalidCustomerId), Times.Once);
        }

        [Fact]
        public async Task UpdateOrder_WithValidOrderId_ShouldUpdateOrderStatusAndReturnUpdatedOrderResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                OrderLineDetails = new List<OrderLineDetails>(),
                Status = OrderStatus.pending
            };
            var updatedStatus = OrderStatus.delivered;

            var updatedOrder = new Order
            {
                OrderId = orderId,
                OrderLineDetails = new List<OrderLineDetails>(),
                Status = updatedStatus
            };
            var updatedOrderResponse = new Order_Response
            {
                OrderId = orderId,
                Status = updatedStatus
            };

            orderRepoMock.Setup(repo => repo.GetOrderByID(orderId)).ReturnsAsync(order);
            orderRepoMock.Setup(repo => repo.UpdateOrder(order)).ReturnsAsync(updatedOrder);
            orderRepoMock.Setup(repo => repo.SaveChanges());
            mapperMock.Setup(mapper => mapper.Map<Order>(It.IsAny<OrderCreate_VM>()))
               .Returns(order);
            mapperMock.Setup(mapper => mapper.Map<Order_Response>(It.IsAny<Order>()))
                .Returns(updatedOrderResponse);

            // Act
            var result = await orderService.UpdateOrder(orderId, updatedStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedStatus, result.Status);
   
        }

        [Fact]
        public async Task UpdateOrder_WithInvalidOrderId_ShouldThrowException()
        {
            // Arrange
            var invalidOrderId = Guid.NewGuid();
            orderRepoMock.Setup(repo => repo.GetOrderByID(invalidOrderId)).ReturnsAsync((Order)null);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await orderService.UpdateOrder(invalidOrderId, OrderStatus.delivered));
            orderRepoMock.Verify(repo => repo.GetOrderByID(invalidOrderId), Times.Once);
        }

        [Fact]
        public async Task GetOrderByID_WithValidOrderId_ShouldReturnOrderResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                OrderLineDetails = new List<OrderLineDetails>(),
                Status = OrderStatus.pending
            };

            var orderResponse = new Order_Response
            {
                OrderId = orderId,
                Status = OrderStatus.pending
            };

            orderRepoMock.Setup(repo => repo.GetOrderByID(orderId)).ReturnsAsync(order);
            mapperMock.Setup(mapper => mapper.Map<Order_Response>(order)).Returns(orderResponse);

            // Act
            var result = await orderService.GetOrderByID(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderResponse.OrderId, result.OrderId);
            Assert.Equal(orderResponse.Status, result.Status);
          
        }

        [Fact]
        public async Task GetOrderByID_WithInvalidOrderId_ShouldThrowException()
        {
            // Arrange
            var invalidOrderId = Guid.NewGuid();
            orderRepoMock.Setup(repo => repo.GetOrderByID(invalidOrderId)).ReturnsAsync((Order)null);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await orderService.GetOrderByID(invalidOrderId));
        }

        private async Task<List<Product>> AddProdcut()
            {
                var proucts = new List<Product>
                {
                    new Product()
                    {
                        Name = "prouct 1",
                        Description = "description",
                        ProductID = new Guid(),
                        Price = 10,
                        StockQuantity = 10

                    }
                };
         
                productRepoMock.Setup(repo => repo.GetProducts())
                    .ReturnsAsync(proucts);
                return proucts;
            }
        }
}
