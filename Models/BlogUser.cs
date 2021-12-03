using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    // Create BlogUser subclass that inherits from IdentityUser class
    public class BlogUser : IdentityUser
    {
        // Create unique props, no need to duplicate inherited props
        // Add data annotations, form input validation, display labels
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long and no more than {1}.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long and no more than {1}.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // User's profile picture and file type
        public byte[] Image { get; set; }
        public string ContentType { get; set; }

        // Links to user's social media account profiles
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long and no more than {1}.", MinimumLength = 2)]
        public string FacebookUrl { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long and no more than {1}.", MinimumLength = 2)]
        public string TwitterUrl { get; set; }

        // Create Full Name prop/method, ensure not mapped to DB
        [NotMapped]
        public string FullName
        { 
            get
            {
                return $"{FirstName} {LastName}";
            }      
        }

        // Create ICollections of user's blogs and posts by assigning a
        // new instance of a HashSet<Blog/Post>()
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
