using _1101113.Data;
using Blog.Data.Models;
using Blog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ApplicationUser> Update(ApplicationUser applicationUser)
        {
            applicationDbContext.Update(applicationUser);
            await applicationDbContext.SaveChangesAsync();
            return applicationUser;
        }
    }
}
