using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Teamcity.DocumentReleaseNotes.Business;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Models;
using Teamcity.DocumentReleaseNotes.Interfaces;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ProjectService: IProjectService
    {
        private readonly ILogger _logger;
        private readonly IProjectWorker _worker;
        private readonly IDocumentFormatterProvider _formatProvider;
        private readonly IProjectDocumentWorker _documentWorker;

        public ProjectService(IProjectWorker Worker, IProjectDocumentWorker DocumentWorker, ILogger<ProjectService> Logger = null)
        {
            _logger = Logger;
            _worker = Worker;
            _documentWorker = DocumentWorker;
        }

        private List<TeamCityProject> GetConfiguredProjectData(CancellationTokenSource cts)
        {
            List<TeamCityProject> projects = new List<TeamCityProject>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                _logger?.LogInformation("Starting Configured Project Load.");

                projects = _worker.LoadProjectsInParallel(cts.Token);
                watch.Stop();

                _logger?.LogInformation("Elapsed Time for semi-parallel data load of all projects: {0}", watch.Elapsed.TotalSeconds);

                return projects;
            }
            catch (Exception ex)
            {
                cts.Cancel();
                if (!(ex is OperationCanceledException))
                {
                    _logger.LogError("Error in Loading Projects", ex);
                    throw;
                }
                return projects;
            }
            finally
            {
                watch.Stop();
            }
        }

        public List<TeamCityProject> GetConfiguredProjectData()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                List<TeamCityProject> projects = new List<TeamCityProject>();
                try
                {
                    projects = GetConfiguredProjectData(cts);
                    return projects;
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    if (!(ex is OperationCanceledException))
                    {
                        _logger.LogError("Error in Loading Projects", ex);
                        throw;
                    }
                    return projects;
                }

            }
        }

        public List<TeamCityProject> DoProjectWork()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                List<TeamCityProject> projects = new List<TeamCityProject>();

                try
                {
                    projects = GetConfiguredProjectData(cts);
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    if (!(ex is OperationCanceledException))
                    {
                        _logger.LogError("Error in Loading Projects", ex);
                        throw;
                    }
                    return projects;
                }

                Parallel.ForEach<TeamCityProject>(projects, p => {
                    var w1 = new Stopwatch();
                    w1.Start();

                    _logger?.LogInformation("Starting to write project: {0}", p.Name);

                    _documentWorker.Write(p);

                    w1.Stop();
                    _logger?.LogInformation("Ending write project: {0}, elapsed Time in Seconds: {1} ", p.Name, w1.Elapsed.TotalSeconds);
                });

                watch.Stop();
                _logger?.LogInformation("Finished loading and writing documents in {0} seconds.", watch.Elapsed.TotalSeconds);
                return projects;
            }

        }

    }
}
