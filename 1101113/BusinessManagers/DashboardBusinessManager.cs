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
            // ดึงข้อมูลผู้ใช้งานจาก ClaimsPrincipal
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);

            // ตรวจสอบว่า applicationUser เป็น null หรือไม่
            if (applicationUser == null)
                return null;

            // ใช้ applicationUser.Id เพื่อกรองโพสต์ตาม UserId
            var userId = applicationUser.Id;

            // ดึงโพสต์ที่เป็นของผู้ใช้งานนั้น
            var posts = await _context.Posts
                .Where(p => p.Creator.Id == userId)  // กรองโพสต์ที่มี UserId ตรงกับผู้ใช้งาน
                .Include(p => p.Comments)
                .ToListAsync();

            var totalPosts = posts.Count();
            var approvedPosts = posts.Count(p => p.Approved);
            var publishedPosts = posts.Count(p => p.published);
            var totalViewers = posts.Sum(p => p.Viewer);
            var postsWithComments = posts.Where(p => p.Comments.Any()).ToList();

            var postsPerMonth = posts
                .GroupBy(p => p.CreatedOn.Month)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            var viewModel = new DashboardViewModel
            {
                TotalPosts = totalPosts,
                ApprovedPosts = approvedPosts,
                PublishedPosts = publishedPosts,
                TotalViewers = totalViewers,
                PostsWithComments = postsWithComments,
                PostsPerMonth = postsPerMonth
            };

            return viewModel;
        }
    }
}
