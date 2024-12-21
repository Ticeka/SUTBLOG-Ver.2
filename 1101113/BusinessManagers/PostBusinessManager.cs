using _1101113.BusinessManagers.Interfaces;
using Blog.Data.Models;
using _1101113.Models.PostViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _1101113.Authorization;
using static _1101113.Authorization.Operation;
using _1101113.Models.HomeViewModels;
using PagedList.Core;
using System.Linq;

namespace _1101113.BusinessManager
{
    public class PostBusinessManager : IPostBusinessManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly IAuthorizationService authorizationService;

        

        public PostBusinessManager(UserManager<ApplicationUser> userManager,IPostService postService,IWebHostEnvironment webHostEnvironment,IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.postService = postService;
            this.webHostEnviroment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }

        public IndexViewModel GetIndexViewModel(string searchString ,int? page)
        {
            int pagesize = 9;
            int pageNumber = page ?? 1;
            var posts = postService.GetPosts(searchString ?? string.Empty)
                .Where(post => post.published);

            return new IndexViewModel
            {
                Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pagesize).Take(pagesize),pageNumber,pagesize,posts.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };
        }

        public async Task<ActionResult<PostViewModel>> GetPostViewModel(int? id,ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var postId = id.Value;

            var post = postService.GetPost(postId);

            if (id is null)
                return new NotFoundResult();

            if (!post.published)
            {
               var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Read);

                if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);
            }

            // เพิ่มจำนวนผู้เข้าชม (ViewCount) หลังจากดึงโพสต์
            await IncrementViewCount(postId, claimsPrincipal);

            return new PostViewModel
            {
                Post = post
            };
        }

        public async Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {

            Post post = createViewModel.Post;
            post.Creator = await userManager.GetUserAsync(claimsPrincipal);
            post.CreatedOn = DateTime.Now;
            post.UpdatedOn = DateTime.Now;


            post = await postService.Add(post);

            string webRootPath = webHostEnviroment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using(var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.HeaderImage.CopyToAsync(fileStream);
            }

            return post;   
        }

        public async Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal)
        {
            if (postViewModel.Post is null || postViewModel.Post.id == 0)
                return new BadRequestResult();

            var post = postService.GetPost(postViewModel.Post.id);

            if (post is null)
                return new NotFoundResult();

            var comment = postViewModel.Comment;

            comment.Author = await userManager.GetUserAsync(claimsPrincipal);
            comment.Post = post;
            comment.CreatedOn = DateTime.Now;

            if(comment.Parent != null)
            {
                comment.Parent = postService.GetComment(comment.Parent.Id);
            }

            return await postService.Add(comment);
        }

        public async Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var post = postService.GetPost(editViewModel.Post.id);

            if (post is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            post.published = editViewModel.Post.published;
            post.Title = editViewModel.Post.Title;
            post.Content = editViewModel.Post.Content;
            post.UpdatedOn = DateTime.Now;

            if (editViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnviroment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.id}\HeaderImage.jpg";

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.HeaderImage.CopyToAsync(fileStream);
                }
            }
            return new EditViewModel
            {
                Post = await postService.Update(post)
            };
        }

        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if(id is null)
                return new BadRequestResult();

            var postId = id.Value;

            var post = postService.GetPost(postId);

            if(id is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            return new EditViewModel
            {
                Post = post
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
                return new ForbidResult();
            else
                return new ChallengeResult();
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if(directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        public async Task IncrementViewCount(int postId, ClaimsPrincipal claimsPrincipal)
        {
            // ดึงโพสต์จาก service
            var post = postService.GetPost(postId);

            if (post != null)
            {
                // ตรวจสอบว่าโพสต์นี้เป็นโพสต์ของผู้ใช้ที่ล็อกอินอยู่หรือไม่
                var currentUser = await userManager.GetUserAsync(claimsPrincipal);

                // ถ้าโพสต์เป็นของผู้ใช้ที่ล็อกอินอยู่, ไม่เพิ่ม ViewCount
                if (post.Creator != currentUser)
                {
                    // เพิ่ม ViewCount ทีละ 1
                    post.Viewer++;

                    // บันทึกการเปลี่ยนแปลงในฐานข้อมูล
                    await postService.Update(post);
                }
            }
        }

    }
}
