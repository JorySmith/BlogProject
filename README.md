# Dev Blogs - A Blog Project
 A blog content management site and full stack web app built with C# ASP.NET MVC, JavaScript, Bootstrap, and PostgreSQL. Deployed and hosted on Heroku.  

 ## Live Demo


 ## Features
 - Dyanmic and searchable blogs, posts, comments, and tags
 - Create, Read, Update, and Delete (CRUD) functionality
 - Model, View, Controller (MVC) and Object Oriented Programming (OOP) principles
 - ASP.NET Core Identity for security, user authentication, and roles
 - PostgreSQL for backend database storage
 - Entity Framework Core and C# code-first models for backend database setup and integration
 - Custom interfaces and services 
 - Pagination using NuGet packages
 - Custom routing with URL slugs created with RegEx 
 - CI/CD between the code repo in GitHub and the web app host Heroku

 ## Build Steps
 1) Create models/C# classes: Blog, Post, Comment, Tag, and BlogUser  
 2) Create code-first migration, connect to local PostgreSQL database via appsettings.json DefaultConnection      
 3) Create CRUD scaffolding (controller routes and actions, views, and identity pages)  
 4) Find and implement a blog template  
 5) Create Data service  
 6) Create Email service, add MailSettings to appsettings.json  
 7) Create Image service  
 8) Create Slug service  
 9) Update Register code behind page  
 10) Update ResendEmailConf and ForgotPassword code behinds  
 11) Update Summernote fonts and Posts Create and Details Views  
 12) Update BlogsController and PostsController to incorporate IFormFile submissions/edits  
 13) Add Tag functionality to PostsController and Post Views  
 14) Add Sweet Alert 2 pop-up functionality  
 15) Update post slug and title validation in PostsController  
 16) Add custom routing endpoints using slugs in Startup.cs  
 17) Update Home Index View to display blogs and add pagination  
 18) Created BlogSearchService  
 19) Added Authorize security annotations with Roles parameter for specific Views  
 20) Add comment functionality on blog posts  
 21) Add moderate comments functionality  
 22) Add delete comments functionality  
 23) Update landing, BlogPostIndex, and Post Details pages  
 24) Set up Heroku hosting, postegres db, and .NET buildpack  
 25) Create Data/ConnectionService to manage postgres connections (local vs Heroku)  
 26) Set up CI/CD between GitHub repo and Heroku  
