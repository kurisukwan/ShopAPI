using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.UserEntities;
using ShopAPI.Models;

namespace ShopAPI.Services
{
    public interface IAddressService
    {
        Task<Address> CreateAsync(Address address);
    }
    public class AddressService : IAddressService
    {
        private readonly DataContext context;

        public AddressService(DataContext context)
        {
            this.context = context;
        }
        public async Task<Address> CreateAsync(Address address)
        {
            var addressEntity = await context.Addresses.FirstOrDefaultAsync(x => x.Street == address.Street &&
            x.PostalCode == address.PostalCode && x.City == address.City);
            if (addressEntity == null)
            {
                addressEntity = new AddressEntity
                {
                    Street = address.Street,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    Country = address.Country
                };
                context.Addresses.Add(addressEntity);
                await context.SaveChangesAsync();
            }
            return new Address(addressEntity.Id, addressEntity.Street, addressEntity.PostalCode, addressEntity.City, addressEntity.Country);
        }
    }
}
