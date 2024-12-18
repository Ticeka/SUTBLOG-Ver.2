using Blog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static _1101113.Authorization.Operation;

namespace _1101113.Authorization
{
    public class PostAuthorizationhandler : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PostAuthorizationhandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Post resource)
        {
            var applicationUser = await userManager.GetUserAsync(context.User);
           if((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator)
            {
                context.Succeed(requirement);
            }
        }
    }
}
