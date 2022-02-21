using BlogProject.Data;
using BlogProject.Enums;
using BlogProject.Models;
using System.Linq;

namespace BlogProject.Services
{
    // Ensure BlgoSearchService is registered in Startup.cs
    public class BlogSearchService
    {
        // Private prop of dbcontext
        private readonly ApplicationDbContext _context;

        // Contructor injection of dbcontext
        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Search method
        public IQueryable<Post> Search(string searchTerm)
        {
            // Get posts from DB that are production ready
            var posts = _context.Posts
                .Where(p => p.ReadyStatus == ReadyStatus.ProductionReady)
                .AsQueryable(); 

            if (searchTerm != null)
            {
                // Search posts based on title, abstract, content, and comments
                // For comments, search within fields: body, moderated body, bloguser firstname
                // lastname, and email. Ensure everything is all the same case.
                searchTerm = searchTerm.ToLower();

                posts = posts.Where(
                    p => p.Title.ToLower().Contains(searchTerm) ||
                    p.Abstract.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(c => c.Body.ToLower().Contains(searchTerm) ||
                                        c.ModeratedBody.ToLower().Contains(searchTerm) ||
                                        c.BlogUser.FirstName.ToLower().Contains(searchTerm) ||
                                        c.BlogUser.LastName.ToLower().Contains(searchTerm) ||
                                        c.BlogUser.Email.ToLower().Contains(searchTerm)));
            }
            // Order posts in descending order (most recent first)
            posts = posts.OrderByDescending(p => p.Created);

            return posts;
        }
    }
        
        
        
}
