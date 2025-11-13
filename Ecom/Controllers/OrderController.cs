
using Ecom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public OrderController(EcomDbContext context) => _context = context;

        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            order.OrderDate = order.OrderDate.Date;

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var maxId = await _context.Orders.MaxAsync(o => (int?)o.OrderId) ?? 0;
                var newId = maxId + 1;

                order.OrderId = newId;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
                return Ok(order); 
            }
            catch (DbUpdateException dbEx)
            {
                await tx.RollbackAsync();
                var msg = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"DB update failed: {msg}");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }
    }
}
