using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BlogProject.Enums;
using X.PagedList;

namespace BlogProject.Controllers
{
    public class PostsController : Controller
    {
        // Private PostsController properties, store instances of DB, Slug, and Image services
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly BlogSearchService _blogSearchService;

        // Constructor injection for the dependencies above
        public PostsController(ApplicationDbContext context,
            ISlugService slugService,
            IImageService imageService,
            UserManager<BlogUser> userManager,
            BlogSearchService blogSearchService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
            _blogSearchService = blogSearchService;
        }

        // GET: SearchIndex 
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            // Save searchTerm in ViewData
            ViewData["SearchTerm"] = searchTerm;

            // Pagination defaults, pass to ToPagedListAsync below
            var pageNumber = page ?? 1;
            var pageSize = 5;

            // Create blog search service with search function
            var posts = _blogSearchService.Search(searchTerm);

            // Await sending posts to View, also invoke ToPagedListAsync
            return View(await posts.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Posts, a list of all posts for all blogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog).Include(p => p.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPostIndex, a list of all posts for a specific blog
        public async Task<IActionResult> BlogPostIndex(int? id, int? page) // nullable ints
        {
            if (id is null)
            {
                return NotFound();
            }

            // Set pageNumber to page or a default of 1, pass to ToPageListAsync
            // Set default pageSize (num of posts to dispaly per page), pass to ToPageListAsync
            var pageNumber = page ?? 1;
            var pageSize = 5;

            // Get associated posts based on post blogid and productionready status
            // send to ToPageListAsync
            var posts = await _context.Posts
                .Where(p => p.BlogId == id && p.ReadyStatus == ReadyStatus.ProductionReady)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            // Display posts in the Posts Index View
            return View(posts);
        }

        // GET: Posts/Details/id (e.g. 5)
        public async Task<IActionResult> Details(string slug)
        {
            // If no slug provided, return NotFound()
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Find the post that belongs to slug in context DB
            // Include p where p is parent blog, post author/BlogUser, post tags, and
            // comments (with associated BlogUsers)
            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.BlogUser)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            // Store ViewData including BlogID ID and Name, and BlogUserId Id.
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // Update Bind to reflect the data the user will HTTP POST after submitting the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,ReadyStatus,Image")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                // Update the post's created datetime property
                post.Created = DateTime.Now;

                // Store current user's id to associate with Post later
                var authorId = _userManager.GetUserId(User);
                post.BlogUserId = authorId;

                // Store the post's image data and content type using _imageService 
                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ContentType = _imageService.ContentType(post.Image);

                // Create post slug, ensure it's unique, update post's slug property
                var slug = _slugService.UrlFriendly(post.Title);

                // Track whether a post error has occurred
                var postError = false;

                // If slug is null, raise model state error, update postError            
                if (string.IsNullOrEmpty(slug))
                {
                    postError = true;
                    ModelState.AddModelError("", "Please create a title.");
                }

                // If slug isn't unique via slug service, raise model state error for "Title", update postError                     
                else if (!_slugService.IsUnique(slug))
                {
                    postError = true;
                    ModelState.AddModelError("Title", "This title is already being used. Please create a different title.");
                }

                // If postError, capture ViewData of TagValues to send to the View
                // Turn tagValues list into a string joined by ","
                if (postError)
                {
                    ViewData["TagValues"] = string.Join(",", tagValues);
                    return View(post);
                }

                post.Slug = slug;

                // Add post to the DB, await save changes
                _context.Add(post);
                await _context.SaveChangesAsync();

                // Add post's tags, loop over each submitted tag in tagValues
                foreach (var tagText in tagValues)
                {
                    // Add new tag to context DB, update PostId, BlogUserId, and Text
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        BlogUserId = authorId,
                        Text = tagText
                    });
                }

                // Await save changes to context DB
                await _context.SaveChangesAsync();

                // Redirect user to Posts Index
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/id (such as 5)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ensure post tags and associated blog id are captured
            var post = await _context.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            // Caputure ViewData for post tags and associated blog id, to make data available to View
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
            return View(post);
        }

        // POST: Posts/Edit/id
        // Update Bind to reflect the data the user will HTTP POST after submitting the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,ReadyStatus")] Post post, IFormFile newImage, List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve and store first post from context DB async so post's props can be updated
                    // Include associated post tags
                    var newPost = await _context.Posts
                        .Include(p => p.Tags)
                        .FirstOrDefaultAsync(p => p.Id == post.Id);

                    // Update post's Updated, Title, Abstract, Content, and ReadyStatus properties with user's edits/inputs
                    newPost.Updated = DateTime.Now;
                    newPost.Title = post.Title;
                    newPost.Abstract = post.Abstract;
                    newPost.Content = post.Content;
                    newPost.ReadyStatus = post.ReadyStatus;

                    // Send post.Title to slug service, store resulting slug
                    // See if result slug matches newPost.Slug
                    // if so, update newPost.Slug and .Title
                    var newSlug = _slugService.UrlFriendly(post.Title);
                    if (newSlug != newPost.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            newPost.Slug = newSlug;
                            newPost.Title = post.Title;
                        }
                        else
                        {
                            // Otherwise, raise model state error for "Title", include ViewData for TagValues
                            // Return user to View(post) - data they submitted
                            ModelState.AddModelError("Title", "This title is already being used. Please create a different title.");
                            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
                            return View(post);
                        }
                    }

                    // If user submitted a new post image, if so, encode and store using image service
                    // Retrieve and store image's content type 
                    if (newImage != null)
                    {
                        newPost.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newPost.ContentType = _imageService.ContentType(newImage);
                    }

                    // Remove old tags from DB, update DB with new tags and associated postId, BloguserId, and text
                    _context.RemoveRange(newPost.Tags);
                    foreach (var tagValue in tagValues)
                    {
                        _context.Add(new Tag()
                        {
                            PostId = post.Id,
                            BlogUserId = newPost.BlogUserId,
                            Text = tagValue
                        });
                    }

                    // Save changes to context DB
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect user back to Post Index
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", post.BlogUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
