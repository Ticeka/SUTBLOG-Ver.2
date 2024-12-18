using Blog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> Update(ApplicationUser applicationUser);
    }
}
