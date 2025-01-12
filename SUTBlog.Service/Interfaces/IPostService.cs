﻿using Blog.Data.Models;
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
        Comment GetComment(int commentId);
        Task<Comment> Add(Comment comment);
        Task<Post> Update(Post post);
        Task Delete(Post post);

        IEnumerable<Post> GetPosts(string searchString);
    }
}

