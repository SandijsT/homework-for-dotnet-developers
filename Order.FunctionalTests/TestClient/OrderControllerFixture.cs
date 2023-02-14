using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Models;
using Xunit;

namespace Order.FunctionalTests.TestClient
{
    public class OrderControllerFixture : IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> factory;
        private static HttpClient client;
        private static OrderDbContext orderContext;

        public OrderControllerFixture(CustomWebApplicationFactory<Program> factory)
        {
            this.factory = factory;

            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            using (var scope = factory.Services.CreateScope())
            {
                orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            }
        }

        public HttpClient Client
        {
            get => client;
            set => client = value;
        }

        public OrderDbContext OrderContext
        {
            get => orderContext;
        }

        public void Dispose()
        {
            client.Dispose();
            factory.Dispose();
        }
    }
}
