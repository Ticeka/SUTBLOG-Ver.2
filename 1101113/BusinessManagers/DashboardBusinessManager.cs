using _1101113.BusinessManagers.Interfaces;
using _1101113.Data;
using _1101113.Models.AdminViewModels;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1101113.BusinessManagers
{
    public class DashboardBusinessManager : IDashboardBusinessManager
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardBusinessManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DashboardViewModel> GetDashboardData(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);

            if (applicationUser == null)
                return null;

            var userId = applicationUser.Id;

            var posts = await _context.Posts
                .Where(p => p.Creator.Id == userId)
                .Include(p => p.Comments)
                .ToListAsync();

            var totalPosts = posts.Count();
            var approvedPosts = posts.Count(p => p.Approved);
            var publishedPosts = posts.Count(p => p.published);
            var totalViewers = posts.Sum(p => p.Viewer);
            var postsWithComments = posts.Where(p => p.Comments.Any()).ToList();

            var totalComments = posts.Sum(p => p.Comments != null ? p.Comments.Count() : 0);  // ตรวจสอบ null ด้วย


            var postsPerMonth = posts
                .GroupBy(p => p.CreatedOn.Month)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            var postsWithCommentsPerMonth = posts
                .GroupBy(p => p.CreatedOn.Month)
                .OrderBy(g => g.Key)
                .Select(g => g.Count(p => p.Comments.Any()))  // คำนวณโพสต์ที่มีคอมเมนต์ในแต่ละเดือน
                .ToList();

            var viewModel = new DashboardViewModel
            {
                TotalPosts = totalPosts,
                ApprovedPosts = approvedPosts,
                PublishedPosts = publishedPosts,
                TotalViewers = totalViewers,
                PostsWithComments = postsWithComments,
                PostsPerMonth = postsPerMonth,
                TotalComments = totalComments,
                PostsWithCommentsPerMonth = postsWithCommentsPerMonth  // เพิ่มข้อมูลโพสต์ที่มีคอมเมนต์
            };

            return viewModel;
        }




    }
}
