using Blog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Models.PostViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }

        public Comment Comment { get; set; }
    }
}
