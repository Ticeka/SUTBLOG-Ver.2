using Blog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Interfaces
{
    public interface IPostService
    {
        Post GetPost(int postId);

        Task<Post> Add(Post post);

        IEnumerable<Post> GetPosts(ApplicationUser applicationUser);

        Task<Post> Update(Post post);

        IEnumerable<Post> GetPosts(string searchString);
    }
}

