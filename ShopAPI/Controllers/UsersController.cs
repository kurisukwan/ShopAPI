using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Filters;
using ShopAPI.Models;
using ShopAPI.Models.Forms;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[UseApiKey]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(SignUpForm form)
        {
            try
            {
                await userService.CreateAsync(new User
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    Address = new Address
                    {
                        Street = form.Street,
                        PostalCode = form.PostalCode,
                        City = form.City,
                        Country = form.Country
                    }
                });
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadAll()
        {
            return new OkObjectResult(await userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user != null)
                return new OkObjectResult(user);
            return new NotFoundResult();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UpdateUserForm form)
        {
            var user = await userService.UpdateAsync(id, form);
            if (user != null)
                return new OkObjectResult(user);
            return new NotFoundResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            if (await userService.DeleteAsync(id) == true)
                return Ok();
            return new NotFoundResult();
        }
    }
}
