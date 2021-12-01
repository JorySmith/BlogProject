using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    // Model/C# class "Blog" describes a single instance of a Blog object
    // New classes/objects can inherit the "Blog's" properties and methods
    public class Blog
    {
        // Property: public (availabe for use by other objects)
        // int (data type) Id (prop name) get; (props can be retrieved)
        // set; (props can be set)

        // Blog's primary key/unique IDs
        public int Id { get; set; }        
        // Foreign key/unique ID
        public string AuthorId { get; set; }

        // Blog's name, desecription, and date of creation
        // Add data annotations/validation: Required, StringLength(), DataType(), Display(), NotMapped
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 4)]
        public string Description { get; set; }   
        
        [DataType(DataType.Date)] // Change DataType display to Date
        [Display(Name = "Created Date")] // Data name label
        public DateTime Created { get; set; }

        // Last date blog was updated, add "?" = can store
        // null value if blog hasn't been updated yet
        [DataType(DataType.Date)] // Change DataType display to Date
        [Display(Name = "Updated Date")] // Data name label
        public DateTime? Updated { get; set; }

        // Blog brand image stored in DB as a byte[]
        // Store image file type so it can be restored
        // Capture user's submitted image via IFormFile over HTTP
        [Display(Name = "Blog Brand Image")] // Data name label
        public byte[] ImageData { get; set; }
        [Display(Name = "Blog Brand Image File Type")] // Data name label
        public string ContentType { get; set; }
        [NotMapped] // Don't map user's submitted image to DB, use byte[] of image
        public IFormFile Image { get; set; }

        // Navigation virtual properties (public virtual Type Name {})
        // Use IdentityUser to store Author ID
        // Use interface ICollection for collecting Posts
        // Instantiate a new HashSet<>() of ICollection Posts 
        // A new HashSet concrete class implements the interface ICollection<Post>
        public virtual IdentityUser Author { get; set; } // Parent of Blog 
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>(); // Child of Blog 
    }
}
