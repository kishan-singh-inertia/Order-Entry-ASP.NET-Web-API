using Ecom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public OrderItemController(EcomDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateOrderItems([FromBody] List<OrderItem> orderItems)
        {
            try
            {
                if (orderItems == null || orderItems.Count == 0)
                    return BadRequest("No order items provided.");

                var orderId = orderItems[0].OrderId;
                var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderId);
                if (!orderExists)
                    return BadRequest($"Order {orderId} does not exist.");

                _context.OrderItems.AddRange(orderItems);
                await _context.SaveChangesAsync();
                return Ok(new { count = orderItems.Count });
            }
            catch (DbUpdateException dbEx)
            {
                var msg = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"DB update failed: {msg}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
