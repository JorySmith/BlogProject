using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using BlogProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, 
               IBlogEmailSender emailSender, 
               ApplicationDbContext dbContext)
        {
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        // GET: Index, Async Task<IActionResult> action
        public async Task<IActionResult> Index(int? page) // nullable int okay, set page default
        {
            // Set pageNumber to page or a default of 1, pass to ToPageListAsync
            var pageNumber = page ?? 1;
            // Set default pageSize, pass to ToPageListAsync
            var pageSize = 3;

            
            // Get NuGet package X.PagedList to use ToPagedListAsync and ref IPagedList interface
            var blogs = _dbContext.Blogs
                .Include(b => b.BlogUser)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(await blogs);
        }

        public IActionResult About()
        {
            return View();
        }

        // GET: Contact
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            // Send contact me email from user submitted form inputs
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
