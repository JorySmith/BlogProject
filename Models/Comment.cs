using BlogProject.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    public class Comment
    {
        // Auto-incrementing primary key/unique id
        public int Id { get; set; }

        // Foreign keys/IDs: associated post, author, and moderator
        public int PostId { get; set; } // Parent
        public string AuthorId { get; set; } // Parent
        public string ModeratorId { get; set; } // Parent

        // Comment original body
        // Data annotations/validation StringLength(), Display()
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 2)]
        [Display(Name = "Comment")]
        public string Body { get; set; }

        // Comment date when created, updated, moderated, or deleted
        // 
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; } // Nullable 
        public DateTime? Moderated { get; set; } // Nullable 
        public DateTime? Deleted { get; set; } // Nullable 

        // Comment moderated body
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 2)]
        [Display(Name = "Moderated Comment")]
        public string ModeratedBody { get; set; }

        // Enum reason/type for a moderated comment 
        public ModerationType ModerationType { get; set; }

        // Navigation properties for foreign keys above
        // They store foreign keys entire record
        public virtual Post Post { get; set; } // Parent of Comment
        public virtual IdentityUser Author { get; set; } // Parent of Comment
        public virtual IdentityUser Moderator { get; set; } // Parent of Comment

    }
}
