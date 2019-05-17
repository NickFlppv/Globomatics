using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Globomatics.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Globomatics
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public Startup(IHostingEnvironment env)
        {
            this.env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            if (!env.IsDevelopment())
            {
                services.Configure<MvcOptions>(o =>
                    o.Filters.Add(new RequireHttpsAttribute()));
            }

            services.AddSingleton<IConferenceService, ConferenceMemoryService>();
            services.AddSingleton<IProposalService, ProposalMemoryService>();
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
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller=Conference}/{action=Index}/{Id?}"));
        }
    }
}
