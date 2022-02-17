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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OriginalIndex()
        {
            var originalComments = await _context.Comments.ToListAsync();
            return View("Index", originalComments);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ModeratedIndex()
        {
            // Filter where comments are moderated, i.e. moderated data is not null
            var moderatedComments = await _context.Comments.Where(c => c.Moderated != null).ToListAsync();
            return View("Index", moderatedComments);
        }
        [Authorize(Roles = "Administrator")]
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
        public async Task<IActionResult> Create([Bind("PostId,Body")] Comment comment, string slug)
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

                // Redirect user to Details action of PostsController 
                // Pass new route obj using slug argument
                // Pass fragment indicating #commentSection 
                return RedirectToAction("Details", "Posts", new { slug }, "commentSection");
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

        // POST: Moderate Comments
        // Bind info needed from user to moderate comment on posting of the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Moderate(int id, [Bind("Id,Body,ModeratedBody,ModerationType")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments
                    .Include(c => c.Post)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);

                // Try updating comment and save it to db, catch any DbUpdateConcurrencyException
                try
                {
                    // Update comment's ModeratedBody, type, time, moderator Id
                    newComment.ModeratedBody = comment.ModeratedBody;
                    newComment.ModerationType = comment.ModerationType;
                    newComment.Moderated = DateTime.Now;
                    // Assign moderator id of user logged in via user manager
                    newComment.ModeratorId = _userManager.GetUserId(User);

                    // Save changes to db
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
                // Redirect user to action Details of PostsController
                // Pass in route value via a new slug obj from newComment.Post.Slug
                // Pass in fragment commentSection to start user at that section of the page
                return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentSection");
            }
            return View(comment);
        }


        // GET: Comments/Delete/id
        [Authorize(Roles = "Administrator")]
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
        // ActionName can be just Delete
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] Comment comment, string slug)
        {
            // Find comment in db using its id, remove it, save changes to db
            var deleteComment = await _context.Comments.FindAsync(comment.Id);
            _context.Comments.Remove(deleteComment);
            await _context.SaveChangesAsync();

            // Return user to action Details in the PostsController
            // Pass route value as a new slug obj
            // Pass fragment commentSection to start user at that section of the page
            return RedirectToAction("Details", "Posts", new { slug }, "commentSection");
        }

        // Comment exists in db or not
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
