using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Filters;
using ShopAPI.Models.Forms;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            this.orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToOrder(TheItemForm form)
        {
            var orderItem = await orderItemService.CreateAsync(form);
            if (orderItem != null)
                return Ok("Item was successfully added to your order!");
            return BadRequest("Wrong order id, to little stock or no such item");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrderItemQuantity(TheItemForm form)
        {
            var orderItem = await orderItemService.UpdateAsync(form);
            if (orderItem != null)
                return new OkObjectResult(orderItem);
            return BadRequest("Stock to low or no such product or wrong order id");
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteOrderItem(DeleteForm form)
        {
            if (await orderItemService.DeleteItemAsync(form) == true)
                return Ok("Item(s) was successfully removed form your order!");
            return BadRequest("No such item in your order or wrong order id");
        }
    }
}
