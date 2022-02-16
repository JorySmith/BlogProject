using System.Collections.Generic;
using BlogProject.Models;

namespace BlogProject.Models
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
