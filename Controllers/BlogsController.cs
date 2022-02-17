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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BlogProject.Controllers
{
    public class BlogsController : Controller
    {
        // Dependency injection of ApplicationDbContext into BlogsController starting state
        // Enables communication with DB
        // Inject image service and user manager to enable their functionality here
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;

        // Constructor, context/instance of DB and image service
        public BlogsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
        }

        // Controller actions/methods
        // GET: Blogs
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            // Return View of a _context list of Blogs including the parent of BlogUser (Author)
            var applicationDbContext = _context.Blogs.Include(b => b.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        // Lock down to Administrators that are logged in
        // Use Authorize security annotation with a Roles parameter
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // Binds help protect against overposting attacks by selecting the only
        // accepted properties to be sent via HTTP POST
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Image")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now;
                blog.BlogUserId = _userManager.GetUserId(User);
                blog.ImageData = await _imageService.EncodeImageAsync(blog.Image);
                blog.ContentType = _imageService.ContentType(blog.Image);
                _context.Add(blog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));                
            }
            
            // Error state, collect submitted data so it can be used
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Blogs/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            
            return View(blog);
        }

        // POST: Blogs/Edit/id
        // Capture any new blog image changes
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Blog blog, IFormFile newImage)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Capture current instance of blog id from DB, update its properties where needed
                    var newBlog = await _context.Blogs.FindAsync(blog.Id);

                    // Update 'Updated' to now
                    newBlog.Updated = DateTime.Now;

                    // If user changed blog name, description, or image, update them
                    if (newBlog.Name != blog.Name)
                    {
                        newBlog.Name = blog.Name;
                    }
                    
                    if (newBlog.Description != blog.Description)
                    {
                        newBlog.Description = blog.Description;
                    }

                    if (newImage != null)
                    {
                        // Encode and save image to DB using image service
                        newBlog.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newBlog.ContentType = _imageService.ContentType(newImage);
                    }

                    // Save changes to DB                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Return user back to Blogs Index View
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        // ActionName to assign a new Action name instead of DeleteConfirmed
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
