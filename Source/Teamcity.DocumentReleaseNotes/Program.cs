using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Teamcity.DocumentReleaseNotes.Business;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Models;
using Teamcity.DocumentReleaseNotes.Interfaces;

namespace Teamcity.DocumentReleaseNotes
{
    class Program
    {
        private static bool _isInteractive = false;
        private static bool _loadHelp = false;
        //ILogger logger;
        static void Main(string[] args)
        {
            LoadArgs(args);

            if (_loadHelp)
            {
                Console.WriteLine("Valid Parameters are /IsInteractive=[0|1] and /help");
                return;
            }

            Autofac.IContainer container = BuildContainer();
            ConfigureServices(container);

            using (var scope = container.BeginLifetimeScope())
            {
                var pService = scope.Resolve<IProjectService>();

                pService.DoProjectWork();
            }

            if (_isInteractive)
            {
                Console.WriteLine("Done writing configured Documentation. Hit enter to continue.");
                Console.ReadLine();
            }
        }

        static Autofac.IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<AppSettingsManager>().As<IConfigurationManager>();
            builder.RegisterType<ApiConfigManager>();
            builder.RegisterType<TeamcityManager>().As<ITeamcityManager>();
            builder.RegisterType<ProjectWorker>().As<IProjectWorker>();
            builder.RegisterType<ApiUrls>();
            builder.RegisterType<DocumentFormatterProvider>().As<IDocumentFormatterProvider>();
            builder.RegisterType<ProjectService>().As<IProjectService>();
            builder.RegisterType<ProjectListProvider>().As<IProjectListProvider>();




            //Register logging services for resolution
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            builder.Populate(services);

            builder.Register(ctx =>
            {
                var formatter = ctx.Resolve<IDocumentFormatterProvider>().GetConfigured();
                var aMgr = ctx.Resolve<IConfigurationManager>();
                var logger = services.BuildServiceProvider().GetService<ILogger<ProjectDocumentWorker>>();
                return new ProjectDocumentWorker(formatter, aMgr, logger);
            }).As<IProjectDocumentWorker>();

            return builder.Build();
        }

        static void ConfigureServices(Autofac.IContainer container)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                //Add NLog as a log consumer
                ILoggerFactory loggerFactory = scope.Resolve<ILoggerFactory>();
                loggerFactory.AddNLog();
                loggerFactory.ConfigureNLog("nlog.config");
            }
        }

        static void LoadArgs(string[] args)
        {
            if (args == null || args.Length == 0)
                _isInteractive = false;

            foreach (string arg in args)
            {
                var split = arg.Split('=');

                switch(split[0].ToLower().TrimStart('/'))
                {
                    case "isinteractive":
                        if (split[1].ToLower().Trim() == "1")
                            _isInteractive = true;
                        break;
                    case "help":
                        _loadHelp = true;
                        break;
                }
            }
        }
    }
}
