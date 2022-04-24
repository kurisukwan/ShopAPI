using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.ProductEntities;
using ShopAPI.Models;

namespace ShopAPI.Services
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(string text);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<bool> UpdateAsync(int id, string text);
        Task<bool> DeleteAsync(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly DataContext context;

        public CategoryService(DataContext context)
        {
            this.context = context;
        }

        // Create------------------------------------------------------------------------
        public async Task<bool> CreateAsync(string text)
        {
            if (!await context.Categories.AnyAsync(x => x.CategoryName == text))
            {
                var categoryEntity = new CategoryEntity
                {
                    CategoryName = text,
                };
                context.Categories.Add(categoryEntity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ReadAll-----------------------------------------------------------------------
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            foreach (var category in await context.Categories.ToListAsync())
                categories.Add(new Category
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            return categories;
        }

        // Update------------------------------------------------------------------------
        public async Task<bool> UpdateAsync(int id, string text)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                category.CategoryName = text;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Delete------------------------------------------------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
