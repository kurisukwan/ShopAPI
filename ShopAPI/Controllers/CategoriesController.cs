#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Models;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult> PostCategory(string text)
        {
            if (await categoryService.CreateAsync(text) == true)
                return Ok();
            return BadRequest();
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return new OkObjectResult(await categoryService.GetAllAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategory(int id, string text)
        {
            if (await categoryService.UpdateAsync(id, text) == true)
                return Ok();
            return new NotFoundResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (await categoryService.DeleteAsync(id) == true)
                return Ok();
            return new NotFoundResult();
        }
    }
}
