using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    public class Tag
    {
        // Tag primary key/unique ID
        public int Id { get; set; }

        // Foreign keys
        public int PostId { get; set; } // Parent
        public string BlogUserId { get; set; } // Parent

        // Tag text
        // Data annotations/validation
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long and less than {1}.", MinimumLength = 2)]
        public string Text { get; set; }

        // Navigation virtual properties that store records of foreign keys
        // Use model name as type or IdentityUser type to store identities
        public virtual Post Post { get; set; } // Parent of Tag 
        public virtual BlogUser BlogUser { get; set; } // Parent of Tag 
    }
}
