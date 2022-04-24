using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.UserEntities;
using ShopAPI.Models;
using ShopAPI.Models.Forms;

namespace ShopAPI.Services
{
    public interface IUserService
    {
        Task CreateAsync(User user);
        Task<IEnumerable<Person>> GetAllUsersAsync();
        Task<Person> GetUserByIdAsync(int id); 
        Task<Person> UpdateAsync(int id, UpdateUserForm form);
        Task<bool> DeleteAsync(int id);
    }
    public class UserService : IUserService
    {
        private readonly DataContext context;
        private readonly IAddressService addressService;

        // Constructor------------------------------------------------------------------------------
        public UserService(DataContext context, IAddressService addressService)
        {
            this.context = context;
            this.addressService = addressService;
        }

        // Create-----------------------------------------------------------------------------------
        public async Task CreateAsync(User user)
        {
            if (!await context.Users.AnyAsync(x => x.Email == user.Email))
            {
                var userEntity = new UserEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    AddressId = (await addressService.CreateAsync(user.Address)).Id
                };
                context.Users.Add(userEntity);
                await context.SaveChangesAsync();
            }
        }

        // ReadAll----------------------------------------------------------------------------------
        public async Task<IEnumerable<Person>> GetAllUsersAsync()
        {
            var users = new List<Person>();
            foreach (var user in await context.Users.Include(x => x.Address).ToListAsync())
                users.Add(new Person
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Street = user.Address.Street,
                    PostalCode = user.Address.PostalCode,
                    City = user.Address.City,
                    Country = user.Address.Country
                });
            return users;
            //return mapper.Map<IEnumerable<Person>>(await context.Users.Include(x => x.Address).ToListAsync());
        }

        // Read-------------------------------------------------------------------------------------
        public async Task<Person> GetUserByIdAsync(int id)
        {
            var user = await context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
                return new Person(user.Id, user.FirstName, user.LastName, user.Email, user.PhoneNumber,
                    user.Address.Street, user.Address.PostalCode, user.Address.City, user.Address.Country);
            return null!;

            //return mapper.Map<Person>(await context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id));
        }

        // Update-----------------------------------------------------------------------------------
        public async Task<Person> UpdateAsync(int id, UpdateUserForm form)
        {
            var user = await context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                user.FirstName = form.FirstName;
                user.LastName = form.LastName;
                user.Email = form.Email;
                user.PhoneNumber = form.PhoneNumber;
                user.Address.Street = form.Street;
                user.Address.PostalCode = form.PostalCode;
                user.Address.City = form.City;
                user.Address.Country = form.Country;
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new Person(user.Id, user.FirstName, user.LastName, user.Email, user.PhoneNumber, 
                    user.Address.Street, user.Address.PostalCode, user.Address.City, user.Address.Country);
            }
            return null!;
        }

        // Delete-----------------------------------------------------------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

}
