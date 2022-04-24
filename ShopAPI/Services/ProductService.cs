using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.ProductEntities;
using ShopAPI.Models;
using ShopAPI.Models.Forms;

namespace ShopAPI.Services
{
    public interface IProductService
    {
        Task<bool> CreateAsync(AddProductForm form);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(string productName);
        Task<Product> UpdateAsync(int articleNumber, AddProductForm form);
        Task<bool> DeleteAsync(int articleNumber);
        Task<int> UpdateQuantity(int articleNumber, int changedQuantity);
    }
    public class ProductService : IProductService
    {
        private readonly DataContext context;

        public ProductService(DataContext context)
        {
            this.context = context;
        }

        // Create-----------------------------------------------------------------------------------
        public async Task<bool> CreateAsync(AddProductForm form)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName == form.CategoryName);
            var product = await context.Products.FirstOrDefaultAsync(x => x.ProductName == form.ProductName);
            if ((category != null) && (product == null))
            {
                var productEntity = new ProductEntity
                {
                    ProductName = form.ProductName,
                    Description = form.Description,
                    Price = form.Price,
                    Quantity = form.Quantity,
                    CategoryId = category.Id,
                };
                context.Products.Add(productEntity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ReadAll----------------------------------------------------------------------------------
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();
            foreach (var product in await context.Products.Include(x => x.Category).ToListAsync())
                products.Add(new Product
                {
                    ArticleNumber = product.ArticleNumber,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity= product.Quantity,
                    CategoryName = product.Category.CategoryName
                });
            return products;
        }

        // Read-------------------------------------------------------------------------------------
        public async Task<Product> GetAsync(string productName)
        {
            var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ProductName == productName);
            if (product != null)
                return new Product(product.ArticleNumber, product.ProductName, product.Description, 
                    product.Price, product.Quantity, product.Category.CategoryName);
            return null!;
        }

        // Update-----------------------------------------------------------------------------------
        public async Task<Product> UpdateAsync(int articleNumber, AddProductForm form)
        {
            var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ArticleNumber == articleNumber);
            if (product != null)
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName == form.CategoryName);
                if (category != null)
                {
                    product.CategoryId = category.Id;
                    product.Category.CategoryName = category.CategoryName;
                    product.ArticleNumber = articleNumber;
                    product.ProductName = form.ProductName;
                    product.Description = form.Description;
                    product.Price = form.Price;
                    product.Quantity = form.Quantity;
                    context.Entry(product).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return new Product(product.ArticleNumber, product.ProductName, product.Description, product.Price, 
                        product.Quantity, product.Category.CategoryName);
                }
            }
            return null!;
        }

        // Delete-----------------------------------------------------------------------------------
        public async Task<bool> DeleteAsync(int articleNumber)
        {
            var product = await context.Products.FindAsync(articleNumber);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // UpdateQuantity---------------------------------------------------------------------------
        public async Task<int> UpdateQuantity(int articleNumber, int changedQuantity)
        {
            var productEntity = await context.Products.FindAsync(articleNumber);
            if (productEntity != null)
            {
                productEntity.Quantity -= changedQuantity;
                context.Entry(productEntity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return productEntity.Quantity;
            }
            return 0;

        }
    }
}
