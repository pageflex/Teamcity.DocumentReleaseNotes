using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Business;
using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace Teamcity.DocumentReleaseNotes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<IConfigurationManager, AppSettingsManager>();
            services.AddSingleton<ITeamcityManager, TeamcityManager>();
            services.AddSingleton<IProjectWorker, ProjectWorker>();
            services.AddSingleton<IDocumentFormatterProvider, DocumentFormatterProvider>();
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton(typeof(ApiUrls));
            services.AddSingleton(typeof(ApiConfigManager));
            services.AddSingleton<IProjectListProvider, ProjectListProvider>();

            services.AddSingleton<IProjectDocumentWorker, ProjectDocumentWorker>((ctx) => {
                var formatter = ctx.GetRequiredService<IDocumentFormatterProvider>().GetConfigured();
                var mgr = ctx.GetRequiredService<IConfigurationManager>();
                var logger = services.BuildServiceProvider().GetService<ILogger<ProjectDocumentWorker>>();
                return new ProjectDocumentWorker(formatter, mgr, logger);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("nlog.config");

            app.UseMvc();
        }
    }
}
