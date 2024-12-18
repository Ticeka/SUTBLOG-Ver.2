using _1101113.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _1101113.BusinessManagers.Interfaces;

namespace _1101113.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;

        public HomeController(IPostBusinessManager blogBusinessManager)
        {
            this.postBusinessManager = blogBusinessManager;
        }

        public IActionResult Index(string searchString,int? page)
        {
            return View(postBusinessManager.GetIndexViewModel(searchString,page));
        }
    }
}
