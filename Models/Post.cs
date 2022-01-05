using BlogProject.Enums;
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
    public class Post
    {
        // Store unique ID/primary key for each Post
        public int Id { get; set; }
        // Foreign keys for the associated Blog and Author
        [Display(Name = "Blog Name")]
        public int BlogId { get; set; } // Parent
        public string BlogUserId { get; set; } // Parent

        // Post title, abstract, and content
        // Data annotations/validation
        [Required]
        [StringLength(75, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 4)]
        public string Abstract { get; set; }
        [Required]
        public string Content { get; set; }

        // Date post was created and updated
        // Data annotations for DataType and Display labels
        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; } // DateTiime? = nullable is okay

        // Add enum ReadyStatus for different states available for Posts
        public ReadyStatus ReadyStatus { get; set; }
        // Convert post title to a slug for URL implimentaiton for SEO benefit
        public string Slug { get; set; }
        // Post image file stored as byte[] data
        public byte[] ImageData { get; set; }
        // Post image content/file type
        public string ContentType { get; set; }
        // User submited image file via IFormFile over HTTP
        [NotMapped] // Don't map image to DB, convert to byte[] for storage
        public IFormFile Image { get; set; }

        // Navigation virtual properties for BlogID and AuthorID
        public virtual Blog Blog { get; set; } // Parent of Post
        public virtual BlogUser BlogUser { get; set; } // Parent of Post

        // Instantiate a new HashSet<>() of an ICollection of Tags and Comments
        // A new HashSet concrete class implements the interface ICollection<Model>
        // These are Child elements of Post
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();


    }
}
