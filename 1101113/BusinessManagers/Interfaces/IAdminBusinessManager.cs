using _1101113.Models.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1101113.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager
    {
        Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal);
        Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal);
        Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
    }
}
