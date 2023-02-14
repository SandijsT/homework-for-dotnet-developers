using Microsoft.AspNetCore.Mvc;
using OrderAPI.BusinessLogic;
using OrderAPI.DTOs;
using OrderAPI.Models;
using System.Globalization;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext OrderContext;

        public OrderController(OrderDbContext orderContext)
        {
            OrderContext = orderContext;
        }

        /// <summary>
        /// Return a list of orders found in the database with the same customer ID
        /// </summary>
        /// <param name="customerId" datatype="int">Customer ID as a whole number<param>
        /// <returns>List of customer orders</returns>
        [HttpGet("{customerId}")]
        public IActionResult GetOrders(int customerId)
        {
            var orderList = OrderContext.Orders.Where(x => x.CustomerId == customerId).ToList();
            
            if(!orderList.Any())
            {
                return NotFound();
            }

            return Ok(orderList);
        }

        /// <summary>
        /// Validate request body with attributes
        /// Calculate total order price, applying a discount
        /// Save the order to the Database
        /// </summary>
        /// <param name="OrderAmount" datatype="string">Required whole number in the range of 1 to 999</param>
        /// <param name="CustomerId" datatype="int">Required</param>
        /// <param name="ExpectedDeliveryDate" datatype="string">Date in the format of "DD/MM/YYYY hh:mm"</param>
        /// <returns>Result response</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState.ToString());
            }

            try
            {
                var customerId = request.CustomerId;
                var orderAmount = int.Parse(request.OrderAmount);
                var expectedDeliveryDate = DateTime.Parse(request.ExpectedDeliveryDate, new CultureInfo("lv-LV"), DateTimeStyles.None);

                var price = Pricing.CalculatePrice(orderAmount);

                var order = new Order
                {
                    CustomerId = customerId,
                    OrderAmount = orderAmount,
                    ExpectedDeliveryDate = expectedDeliveryDate,
                    Price = price
                };

                _ = await OrderContext.Orders.AddAsync(order).ConfigureAwait(false);
                _ = await OrderContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                return StatusCode(500, "Internal error occurred processing your order!");
            }

            return Ok();
        }
    }
}
