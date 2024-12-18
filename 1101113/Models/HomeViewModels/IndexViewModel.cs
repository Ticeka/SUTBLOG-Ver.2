using Blog.Data.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IPagedList<Post> Posts { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
    }
}
