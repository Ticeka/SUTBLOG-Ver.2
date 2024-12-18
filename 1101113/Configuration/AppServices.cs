using _1101113.BusinessManager;
using _1101113.BusinessManagers.Interfaces;
using _1101113.Data;
using Blog.Data.Models;
using Blog.Service; // นำเข้า namespace ที่มี BlogService
using Blog.Service.Interfaces; // นำเข้า namespace ของ IBlogService
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blog.Service.Interfaces;
using Blog.Service;
using Microsoft.Extensions.FileProviders;
using System.IO;
using _1101113.BusinessManagers;
using Microsoft.AspNetCore.Authorization;
using _1101113.Authorization;

namespace _1101113.Configuration
{
    public static class AppServices
    {
        public static void AddDefaultServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            serviceCollection.AddControllersWithViews().AddRazorRuntimeCompilation();
            serviceCollection.AddRazorPages();

            serviceCollection.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        }

        public static void AddCustomServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPostBusinessManager, PostBusinessManager>();
            serviceCollection.AddScoped<IAdminBusinessManager, AdminBusinessManager>();

            serviceCollection.AddScoped<IPostService, PostService>(); // ลงทะเบียน BlogService
        }

        public static void AddCustomerAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthorizationHandler, PostAuthorizationhandler>();
        }
    }
}
