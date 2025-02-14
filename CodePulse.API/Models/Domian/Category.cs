using System.Collections;

namespace CodePulse.API.Models.Domian
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }  
        
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
