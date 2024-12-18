using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113.Models.AdminViewModels
{
    public class AboutViewModel
    {
        [Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }

        [Display(Name = "SubHeader")]
        public string SubHeader { get; set; }
        public string Content { get; set; }

    }
}
