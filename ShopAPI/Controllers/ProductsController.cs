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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService puroductService)
        {
            this.productService = puroductService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(AddProductForm form)
        {
            if (await productService.CreateAsync(form) == true)
                return Ok();
            return BadRequest("No such product category or product already exists. " +
                "Therefore product could not be created");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return new OkObjectResult(await productService.GetAllAsync());
        }

        [HttpGet("{productName}")]
        public async Task<ActionResult> GetProduct(string productName)
        {
            var product = await productService.GetAsync(productName);
            if (product != null)
                return new OkObjectResult(product);
            return new NotFoundResult();
        }

        [HttpPut]
        public async Task<ActionResult> PutProduct(int articleNumber, AddProductForm form)
        {
            var product = await productService.UpdateAsync(articleNumber, form);
            if (product != null)
                return new OkObjectResult(product);
            return BadRequest("No such product found or no such product category");
        }

        [HttpDelete("{articleNumber}")]
        public async Task<ActionResult> DeleteProduct(int articleNumber)
        {
            if (await productService.DeleteAsync(articleNumber) == true)
                return Ok();
            return new NotFoundResult();
        }
    }
}
