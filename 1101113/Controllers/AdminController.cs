using _1101113.BusinessManagers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager adminBusinessManager;


        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await adminBusinessManager.GetAdminDashboard(User));
        }
    }
}
