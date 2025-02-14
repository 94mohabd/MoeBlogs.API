using CodePulse.API.Models.Domian;

namespace CodePulse.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<List<BlogImage>> GetAll();
    }
}
