using _1101113.Models.PostViewModels;
using _1101113.Models.HomeViewModels;
using Blog.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1101113.BusinessManagers.Interfaces
{
    public interface IPostBusinessManager
    {
        Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        IndexViewModel GetIndexViewModel(string searchString, int? page);
    }
}
