using _1101113.Configuration;
using _1101113.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1101113
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

    
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultServices(Configuration);
            services.AddCustomServices();
            services.AddCustomerAuthorization();
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddDefaultConfiguration(env);
        }
    }
}
