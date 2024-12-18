using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Blog.Data.Models
{
    public class Post
    {
        public int id { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Approved { get; set; }
        public bool published { get; set; }
        public ApplicationUser Approver { get; set; }
        public string Tag { get; set; }
        public int Viewer { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
