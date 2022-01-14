# Blog Project
 A full stack Blog web app built with C# ASP.NET MVC and PostgreSQL.  

 ## Features
 - Blogs, posts, comments, and tags
 - Create, read, update, and delete (CRUD) functionality
 - Back end integration with a PostgreSql database
 - C# ASP.NET MVC design pattern
 - Object oriented programming (OOP)
 - Secure web app using authentication and roles

 ## Build Steps
 1) Create models/C# classes: Blog, Post, Comment, Tag, and BlogUser  
     a. Download Entity Framework Core Tools, AspNetCore Identity UI, and Identity EFC NuGet packages  
     b. Add data validation/annotations  
     c. E.g.: Required, StringLength(), DataType(), Display(), and NotMapped  
     d. Add slugs from post titles to enhance Search Engine Optimizaiton (SEO)  
     e. Create virtual navigation properties and relationships between the models  
     f. Create enums ReadyStatus for Posts, ModerationType for moderated Comments  
     g. Create, integrate, register service, and inject BlogUser subclass of IdentityUser  
 2) Create code-first migration then integrate and update local PostgreSQL database  
     a. Download NPGSQL and NPGQL Entity Framework NuGet packages  
     b. Add UseNpgsql service option to Startup.cs  
     c. Update default connection string in appsettings.json to reflect PostgreSQL  
     d. Delete Migrations folder, add migration to "Data/Migrations" folder, update database  
 3) Create CRUD scaffolding (controller routes and actions, views, and identity pages)  
     a. Add new controller scaffolding for: Blog, Comment, Post, and Tag  
     b. Add new scaffolding to "Areas/Identity/Pages" selecting all that are available  
     c. Update BlogsController HTTP GET and POST for Index/Create/Edit/Details/Delete actions  
     d. Update PostsController HTTP GET and POST for Index/Create/Edit/Details/Delete actions  
     e. Update CommentsController HTTP GET and POST for Index/Create/Edit/Details/Delete actions  
     f. Remove TagsController and Views  
4) Find and implement a blog template  
     a. Update Views: Shared _Layout and Home Index  
     b. Update CSS, JS, and IMG files and links  
     c. Update Views: Post Details  
5) Create Data service  
     a. Create BlogRole enums  
     b. Create DataService class in a new Services folder to seed roles (admin and mod) and users  
     c. Inject ApplicationDbContext, RoleManager, and UserManager  
     d. Register DataService in Startup.cs, add async call in Program.cs  
     e. Add create DB from migrations to DataService       
6) Create Email service  
     a. Create MailSettings class in new ViewModels folder  
     b. Update settings.json and Startup.cs  
     c. Create interface IBlogEmailSender in Services folder  
     d. Update Register.cshtml code behind and Startup.cs with IBlogEmailSender  
     e. Implement IBlogEmailSender with a new EmailService class  
     f. Use MimeMessage() to store email sender/receiver/subject properties  
     g. Use BodyBuilder() and SmtpClient() to configure email body message and authentication  
     h. Update ResendEmailConfirmation.cshtml with IBlogEmailSender  
     i. Create ContactMe view model, add to Home controller  
     j. Update Contact View, add Summernote editor to form  
7) Create Image service  
     a. Create interface IImageService  
     b. Create and register concrete class BasicImageService that implements IImageService  