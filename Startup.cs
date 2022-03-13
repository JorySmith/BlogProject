using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using BlogProject.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // ConfigureServices adds required interfaces with their specific dependency injections 
    // This method gets called by the runtime. Use this method to add services to the container.
    // GetConnectionString() looks in the appsettings.json for the value to the key DefaultConnection        
    public void ConfigureServices(IServiceCollection services)
    {
      // Use Npgsql after getting both NPGSQL NuGet packages
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseNpgsql(ConnectionService.GetConnectionString(Configuration)));

      services.AddDatabaseDeveloperPageExceptionFilter();

      // Register BlogUser in place of IdentityUser
      // removed: services.AddDefaultIdentity<IdentityUser>
      // Add default UI and Token Providers services
      services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
          .AddDefaultUI()
          .AddDefaultTokenProviders()
          .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddControllersWithViews();

      // Add Razor pages service
      services.AddRazorPages();

      // Regsiter and add as scoped the DataService + BlogSearchService
      services.AddScoped<DataService>();
      services.AddScoped<BlogSearchService>();

      // Register and configure a MailSettings instance from appsettings.json
      services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

      // Add scoped interface service IBlogEmailSender, implement via EmailService class
      services.AddScoped<IBlogEmailSender, EmailService>();

      // Register interface image service and its implementation BasicImageService
      services.AddScoped<IImageService, BasicImageService>();

      // Register interface slug service and its implementation BasicSlugService
      services.AddScoped<ISlugService, BasicSlugService>();
    }

    // Configure is used to add middleware
    // This method gets called by the runtime
    // Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        // Add custom routing using slugs
        // Add new endpoint and map controller route
        endpoints.MapControllerRoute(
            name: "SlugRoute",
            pattern: "BlogPosts/Post/{slug}",
            defaults: new { controller = "Posts", action = "Details" });

        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
