using _1101113.BusinessManagers.Interfaces;
using _1101113.Data;
using _1101113.Models.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IAuthorBusinessManager adminBusinessManager;
        private readonly ApplicationDbContext _context;  


        public AuthorController(IAuthorBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await adminBusinessManager.GetAuthorDashBoard(User));
        }

        public async Task<IActionResult> About()
        {
            return View(await adminBusinessManager.GetAboutViewModel(User));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await adminBusinessManager.UpdateAbout(aboutViewModel, User);
            return RedirectToAction("About");

        }

    }

}

