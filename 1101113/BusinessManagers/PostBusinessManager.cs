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
            int pagesize = 3;
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
                string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{post.id}\HeaderImage.jpg";

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
            var BlogId = id.Value;
            var post = postService.GetPost(BlogId);

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
    }
}
