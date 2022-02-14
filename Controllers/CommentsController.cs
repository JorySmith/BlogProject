using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments

        public async Task<IActionResult> OriginalIndex()
        {
            var originalComments = await _context.Comments.ToListAsync();
            return View("Index", originalComments);
        }
        
        public async Task<IActionResult> ModeratedIndex()
        {
            // Filter where comments are moderated, i.e. moderated data is not null
            var moderatedComments = await _context.Comments.Where(c => c.Moderated != null).ToListAsync();
            return View("Index", moderatedComments);
        }        

        public async Task<IActionResult> Index()
        {
            var allComments = await _context.Comments.ToListAsync();
            return View(allComments);
        }

        // GET: Comments/Details and /Create will not be used     

        // POST: Comments/Create
        // Only props to bind are associated PostId and Body of comment
        // The rest can be dome programmatically below
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Store user id of logged in user using user manager
                comment.BlogUserId = _userManager.GetUserId(User);
                
                // Update comment's Created property
                comment.Created = DateTime.Now;

                // Update db with comment
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to Post Details not Comments Index
                return RedirectToAction(nameof(Index));
            }
            
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", comment.BlogUserId);
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Retrieve comment's associated post and first comment where c.Id matches
                // user's posted comment Id
                var newComment = await _context.Comments
                    .Include(c => c.Post)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);

                try
                {
                    // Update newComment's body and updated properties
                    newComment.Body = comment.Body;
                    newComment.Updated = DateTime.Now;

                    // Saved changes to db
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect user to Details action of PostsController 
                // Pass new route obj using newComment's post slug
                // Pass fragment indicating #commentSection 
                return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentSection");
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", comment.BlogUserId);
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Moderator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
