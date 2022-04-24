namespace ShopAPI.Models
{
    public class Address
    {
        public Address()
        {
        }
        public Address(int id, string street, string postalCode, string city, string country)
        {
            Id = id;
            Street = street;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
