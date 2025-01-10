using _1101113.Models.AdminViewModels;
using _1101113.Models.AuthorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1101113.BusinessManagers.Interfaces
{
    public interface IDashboardBusinessManager
    {
        Task<DashboardViewModel> GetDashboardData(ClaimsPrincipal claimsPrincipal);
    }
}
