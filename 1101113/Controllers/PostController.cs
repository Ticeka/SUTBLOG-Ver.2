using _1101113.BusinessManagers.Interfaces;
using _1101113.Models.PostViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Controllers
{
    public class PostController : Controller
    {

        private readonly IPostBusinessManager postBusinessManager;

        public PostController(IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }


        public async Task<IActionResult> Edit(int? id)
        {
            var actionResult = await postBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createViewModel)
        {           
            await postBusinessManager.CreatePost(createViewModel, User);
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditViewModel editViewModel)
        {
            var actionResult = await postBusinessManager.UpdatePost(editViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Post.id });


            return actionResult.Result;
        }
    }
}
