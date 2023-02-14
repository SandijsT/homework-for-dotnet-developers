using Order.FunctionalTests.TestClient;
using OrderAPI.DTOs;
using System.Text;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Models;

namespace Order.FunctionalTests
{
    public class OrderControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly OrderControllerFixture orderControllerFixture;
        private readonly CustomWebApplicationFactory<Program> factory;

        public OrderControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            this.factory = factory;
            orderControllerFixture = new OrderControllerFixture(factory);
        }

        [Fact]
        public async void CreateOrder_OKResponse() => await CreateOrder("50", 4, DateTime.Now.AddDays(10).ToString(), "OK", true);

        [Fact]
        public async void CreateOrder_BadRequest() => await CreateOrder("60", 5, DateTime.Now.AddDays(-10).ToString(), "BadRequest", false);

        public async Task CreateOrder(string orderAmount, int customerId, string expectedDeliveryDate, string statusCode, bool savedToDatabase)
        {
            var orderRequest = new OrderRequest()
            {
                OrderAmount = orderAmount,
                CustomerId = customerId,
                ExpectedDeliveryDate = expectedDeliveryDate
            };
            var options = new JsonSerializerOptions();
            var jsonString = JsonSerializer.Serialize(orderRequest, options);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var result = await orderControllerFixture.Client.PostAsync("api/Order", httpContent);

            if(savedToDatabase)
            {
                bool savedOrderInDataBase;
                using (var scope = factory.Services.CreateScope())
                {
                    var orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                    savedOrderInDataBase = orderContext.Orders.Any(x => x.CustomerId == customerId);
                }

                Assert.True(savedOrderInDataBase);
            }

            Assert.Equal(result.StatusCode.ToString(), statusCode);
        }

        [Fact]
        public async Task GetOrders_OKResponse()
        {
            var customerId = 7;
            var order = new OrderAPI.Models.Order()
            {
                OrderAmount = 5,
                CustomerId = customerId,
                ExpectedDeliveryDate = DateTime.Now.AddDays(20),
                Price = 100
            };

            using (var scope = factory.Services.CreateScope())
            {
                var orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                orderContext.Orders.Add(order);
                await orderContext.SaveChangesAsync();
            }
            var result = await orderControllerFixture.Client.GetAsync($"api/Order/{customerId}");

            Assert.True(result.IsSuccessStatusCode);
        }
    }
}
