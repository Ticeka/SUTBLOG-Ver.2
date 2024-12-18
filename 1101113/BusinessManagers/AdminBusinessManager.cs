using _1101113.BusinessManagers.Interfaces;
using _1101113.Models.AdminViewModels;
using Blog.Data.Models;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1101113.BusinessManagers
{
    public class AdminBusinessManager : IAdminBusinessManager
    {
        private UserManager<ApplicationUser> userManager;
        private IPostService postService;

        public AdminBusinessManager(UserManager<ApplicationUser> userManager,IPostService postService)
        {
            this.userManager = userManager;
            this.postService = postService;
        }

         public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimPrincipal);
            return new IndexViewModel
            {
                Posts = postService.GetPosts(applicationUser)
            };
        }
    }
}
