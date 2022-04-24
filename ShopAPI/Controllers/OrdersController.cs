using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.Forms;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(CreateOrderForm form)
        {
            var order = await orderService.CreateAsync(form);
            if (order != null)
                return Ok(order);
            return BadRequest("You dont have a user profile yet");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            return new OkObjectResult(await orderService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            var order = await orderService.GetAsync(id);
            if (order != null)
                return new OkObjectResult(order);
            return new NotFoundResult();
        }
        // En update av ordern är i princip en create, update, delete av en OrderItem kombinerat.
        // Därför blir det smidigare att skapa ytterligare en controller som hanterar just dessa 3.
        // Update av ordern innebär en ändring av order statusen istället.
        [HttpPut]
        public async Task<ActionResult> EditOrderItem(int id, UpdateOrderStatusForm form)
        {
            var order = await orderService.UpdateAsync(id, form);
            if (order != null)
                return new OkObjectResult(order);
            return BadRequest("No such order found");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (await orderService.DeleteAsync(id) == true)
                return Ok();
            return new NotFoundResult();
        }
    }
}
