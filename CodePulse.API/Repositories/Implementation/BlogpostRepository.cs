using CodePulse.API.Data;
using CodePulse.API.Models.Domian;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogpostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogpostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(blog => blog.Id == id);
            if (existingBlogPost is null)
            {
                return null;
            }

            dbContext.BlogPosts.Remove(existingBlogPost);
            dbContext.SaveChanges();

            return existingBlogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(blog => blog.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetById(Guid id)
        {
            return await dbContext.BlogPosts.Include(blog => blog.Categories).FirstOrDefaultAsync(blog => blog.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandle(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(blog => blog.Categories).FirstOrDefaultAsync(blog => blog.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(blog => blog.Categories)
                .FirstOrDefaultAsync(blog => blog.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }
            // Update BlogPost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();
            
            return existingBlogPost;
        }
    }
}
