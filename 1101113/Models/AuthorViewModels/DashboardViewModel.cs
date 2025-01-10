using Blog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Models.AuthorViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

        // สำหรับ Dashboard ต้องย้าย
        public int TotalPosts { get; set; }
        public int ApprovedPosts { get; set; }
        public int PublishedPosts { get; set; }
        public int TotalViewers { get; set; }
        public IEnumerable<Post> PostsWithComments { get; set; }
        public int TotalComments { get; set; }

        // ข้อมูลกราฟ
        public List<int> PostsPerMonth { get; set; } // ใช้สำหรับกราฟ
        
        public int ApprovedPostsPerMonth { get; set; }
        public int PublishedPostsPerMonth { get; set; }
        public List<int> PostsWithCommentsPerMonth { get; set; }
    }
    

}
