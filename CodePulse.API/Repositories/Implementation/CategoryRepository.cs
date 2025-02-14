using CodePulse.API.Data;
using CodePulse.API.Models.Domian;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateCategory(Category category) 
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);

            if (existingCategory != null)
            {
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(
            string? query = null,
            string? sortBy = null, 
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100)
        {
            // Query
            var categories = dbContext.Categories.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query) == false) 
            {
                categories = categories.Where(cat => cat.Name.Contains(query));
            }

            // sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) 
                        ? true : false;

                    categories = isAsc? categories.OrderBy(cat => cat.Name): categories.OrderByDescending(cat => cat.Name);
                }

                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    categories = isAsc ? categories.OrderBy(cat => cat.UrlHandle) : categories.OrderByDescending(cat => cat.Name);
                }                
            }

            // Pagination

            var skipResults = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);
        }

        public async Task<int> GetCount()
        {
            return await dbContext.Categories.CountAsync();
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory =  await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == category.Id);
            
            if (existingCategory != null) 
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }
    }
}
