# Blog Project
 A full stack Blog web app built with C# ASP.NET MVC.  

 ## Features
 - Blogs, posts, comments, and tags
 - C# ASP.NET MVC
 - Object oriented programming
 - Secure via authentication and roles

 ## Build Steps
 1) Create models/C# classes: Blog, Post, Comment, Tag, and BlogUser  
     a. Download Entity Framework Core Tools, AspNetCore Identity UI, and Identity EFC NuGet packages  
     b. Add data validation/annotations  
     c. E.g.: Required, StringLength(), DataType(), Display(), and NotMapped  
     d. Add slugs from post titles to enhance Search Engine Optimizaiton (SEO)  
     e. Create virtual navigation properties and relationships between the models  
     f. Create enums ReadyStatus for Posts, ModerationType for moderated Comments  
     g. Create, integrate, register service, and inject BlogUser subclass of IdentityUser  
 2) Create code-first migration then update local PostgreSQL database  
     a. Download NPGSQL and NPGQL Entity Framework NuGet packages  
     b. Add UseNpgsql service option to Startup.cs  
     c. Update default connection string in appsettings.json to reflect PostgreSQL  
     d. Delete Migrations folder, add migration in "Data/Migrations" folder, update database  
     
