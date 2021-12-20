# Blog Project
 A full stack blog and content management site built from the ground up.

 ## Features
 - Blogs, posts, comments, and tags
 - C# ASP.NET MVC
 - Object oriented programming
 - Secure via authentication and roles

 ## Build Steps
 1) Create models/C# classes: Blog, Post, Comment, Tag, and BlogUser
     a. Add data validation/annotations  
     b. E.g.: Required, StringLength(), DataType(), Display(), and NotMapped  
     c. Add slugs from post titles to enhance Search Engine Optimizaiton (SEO)
     d. Create virtual navigation properties and relationships between the models
     e. Create enums ReadyStatus for Posts, ModerationType for moderated Comments
     f. Create, integrate, register service, and inject BlogUser subclass of IdentityUser 
 2) Add code-first migration then update PostgreSQL database with applicable tables 
     a. Update default connection string in appsettings.json to reflect PostgreSQL
     b. Output migration to Data/Migrations folder
