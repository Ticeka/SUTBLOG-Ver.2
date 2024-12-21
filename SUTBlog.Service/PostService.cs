using _1101113.Data;
using Blog.Data.Models;
using Blog.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public PostService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Post GetPost(int postId)
        {
            return applicationDbContext.Posts
                .Include(post => post.Creator)
                .Include(post => post.Comments)
                    .ThenInclude(comment => comment.Author)
                .Include(post => post.Comments)
                    .ThenInclude(comment => comment.Comments)
                        .ThenInclude(reply => reply.Parent)
                .FirstOrDefault(post => post.id == postId);
        }

        public IEnumerable<Post> GetPosts(string searchString)
        {
            return applicationDbContext.Posts
                .OrderByDescending(post => post.UpdatedOn)
                .Include(post => post.Creator)
                .Include(post => post.Comments)
                .Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));
                
        }

        public IEnumerable<Post> GetPosts(ApplicationUser applicationUser)
        {
            return applicationDbContext.Posts
                .Include(post => post.Creator)
                .Include(post => post.Approver)
                .Include(post => post.Comments)
                .Where(post => post.Creator == applicationUser);
        }

        public Comment GetComment(int commentId)
        {
            return applicationDbContext.Comments
                .Include(comment => comment.Author)
                .Include(comment => comment.Post)
                .Include(comment => comment.Parent)
                .FirstOrDefault(comment => comment.Id == commentId);
        }

        public async Task<Post> Add(Post post)
        {
            applicationDbContext.Add(post);
            await applicationDbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Comment> Add(Comment comment)
        {
            applicationDbContext.Add(comment);
            await applicationDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Post> Update(Post post)
        {
            applicationDbContext.Update(post);
            await applicationDbContext.SaveChangesAsync();
            return post;
        }

        public async Task Delete(Post post)
        {
            // Delete related comments if needed (optional, depending on your business logic)
            var comments = applicationDbContext.Comments.Where(comment => comment.Post.id == post.id).ToList();
            foreach (var comment in comments)
            {
                applicationDbContext.Comments.Remove(comment);
            }

            // Remove the post itself
            applicationDbContext.Posts.Remove(post);

            // Save changes to apply the deletion
            await applicationDbContext.SaveChangesAsync();
        }

    }
}
