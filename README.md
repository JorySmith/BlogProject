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
     d. Delete Migrations folder, add migration in "Data/Migrations" folder, update database  
 3) Create CRUD scaffolding (controller routes and actions, views, and identity pages)  
     a. Add new controller scaffolding for these models: Blog, Comment, Post, and Tag  
     b. Add new scaffolding to "Areas/Identity/Pages" selecting all that are available  
     c. Update BlogsController HTTP GET and POST for Index/Create/Edit/Details/Delete actions
     c. Update PostsController HTTP GET and POST for Index/Create/Edit/Details/Delete actions
