using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Globomatics.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Globomatics
{
    public class Startup
    {
        private readonly IHostingEnvironment env;
        private readonly IConfiguration config;

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            this.env = env;
            this.config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //шифрование данных
            services.AddDataProtection();
            if (!env.IsDevelopment())
            {
                services.Configure<MvcOptions>(o =>
                    o.Filters.Add(new RequireHttpsAttribute()));
            }

            services.AddSingleton<IConferenceService, ConferenceMemoryService>();
            services.AddSingleton<IProposalService, ProposalMemoryService>();
            services.AddSingleton<PurposeStringConstants>();

            services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("GlobomaticsConnection"),
             b => b.MigrationsAssembly("Globomatics")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Password.RequireNonAlphanumeric = false)
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                                ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts(h => h.MaxAge(days: 365));
            }
            app.UseCsp(options => options.DefaultSources(s => s.Self()));
            app.UseXfo(h => h.Deny());
            app.UseStatusCodePages();
            app.UseStaticFiles();
            logger.Log(LogLevel.Information,
                $"Connection string:{config.GetConnectionString("GlobomaticsConnection")}");
            app.UseAuthentication();
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller=Account}/{action=Login}/{Id?}"));
        }
    }
}
