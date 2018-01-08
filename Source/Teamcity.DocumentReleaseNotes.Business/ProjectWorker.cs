using Teamcity.DocumentReleaseNotes.Interfaces;
using Teamcity.DocumentReleaseNotes.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ProjectWorker: IProjectWorker
    {
        const int QueueBoundedCapacity = 4;
        const int LoadBalancingDegreeOfConcurrency = 2;
        List<string> _projects;
        ITeamcityManager _tMgr;
        const TaskCreationOptions options = TaskCreationOptions.LongRunning;
        ILogger _logger;

        public ProjectWorker(IConfigurationManager cMgr, IProjectListProvider projectListProvider, ILogger<ProjectWorker> logger = null, ITeamcityManager tMgr = null)
        {
            _projects = projectListProvider?.GetConfigured();
            _tMgr = tMgr;
            _logger = logger;
        }

        public List<TeamCityProject> LoadProjects(CancellationToken token)
        {
            _logger?.LogInformation("Starting to load Projects.");
            List<TeamCityProject> pList = new List<TeamCityProject>();
            foreach (var pStr in _projects)
            {
                var p = LoadProjectAsync(pStr, token).GetAwaiter().GetResult();
                if (p != null) pList.Add(p);
            }

            _logger?.LogInformation("Ending Project Load.");
            return pList;
        }

        public List<TeamCityProject> LoadProjectsInParallel(CancellationToken token)
        {
            // TODO: Need to re-think this in the terms of producers and consumers
            try {
                //var f = new TaskFactory(CancellationToken.None, options, TaskContinuationOptions.None, TaskScheduler.Default);
                Task<TeamCityProject>[] tasks = new Task<TeamCityProject>[_projects.Count];
                List<TeamCityProject> projects = new List<TeamCityProject>();

                for (int i = 0; i < _projects.Count; i++)
                {
                    if (token.IsCancellationRequested)
                        break;
                    tasks[i] = LoadProjectAsync(_projects[i], token);
                }

                if (token.IsCancellationRequested) return projects;


                // TODO: instead of a WaitAll, it would be nice if we could do a Queue
                Task.WaitAll(tasks);

                Array.ForEach<Task<TeamCityProject>>(tasks, t => 
                {
                    /// TODO: Error Check to see if each task was successful
                    projects.Add(t.Result);
                });

                return projects;
            }
            finally {

            }


        }

        public TeamCityProject LoadProject(string projectName, CancellationToken token)
        {
            _logger?.LogInformation("Starting to Load Project {0}", projectName);
            var p = LoadProjectAsync(projectName, token).GetAwaiter().GetResult();
            _logger?.LogInformation("Ending Project Load {0}", projectName);

            return p;
        }

        public async Task<TeamCityProject> LoadProjectAsync(string projectName, CancellationToken token)
        {
            // Builds
            // Changes
            // Change Details

            using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                // Get Project Data that includes Build Types
                _logger?.LogInformation("Starting Load of {0}.", projectName);
                try
                {
                    var pData = await _tMgr.GetProjectData(projectName);

                    _logger?.LogInformation("Finished High Level Project data load for {0}.", projectName);

                    pData.ParallelBuildTypes = LoadBuildTypesCollection(pData, QueueBoundedCapacity);

                    _logger?.LogInformation("{0} has {1} build types.", projectName, pData.ParallelBuildTypes.Count);
                    // Load Builds for each Build Type
                    var p1 = Parallel.ForEach<BuildType>(pData.ParallelBuildTypes, bt =>
                    {
                        _logger?.LogInformation("Starting to get builds for Build Type {0}.", bt.Name);
                        bt.Builds = _tMgr.GetBuilds(bt.Id).GetAwaiter().GetResult()?.Build;

                        if (token.IsCancellationRequested)
                        {
                            throw new OperationCanceledException();
                        }

                        if (bt.Builds != null)
                        {
                            _logger?.LogInformation("Retrieved {0} builds for BuildType {1}.", bt.Builds.Count(), bt.Name);
                            foreach (var b in bt.Builds)
                            {
                                b.Changes = _tMgr.GetChanges(b.Id).GetAwaiter().GetResult()?.Change;

                                if (token.IsCancellationRequested)
                                {
                                    throw new OperationCanceledException();
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation("There were zero builds for BuildType {0}.", bt.Name);
                            bt.Builds = new List<Build>();
                        }
                    });

                    return pData;
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    if (!(ex is OperationCanceledException))
                    {
                        _logger?.LogError("Error building project.", ex);
                        throw;
                    }
                    return null;
                }
            }
        }

        private static BlockingCollection<BuildType> LoadBuildTypesCollection(TeamCityProject Project, int QueueSize)
        {
            BlockingCollection<Models.BuildType> buildTypes = new BlockingCollection<Models.BuildType>();
            // Each Build Type
            try
            {
                foreach (var bt in Project.BuildTypes.BuildType)
                {
                    buildTypes.Add(bt);
                }
            }
            finally
            {
                buildTypes.CompleteAdding();
            }

            return buildTypes;
        }

        private static BlockingCollection<Build> LoadBuildsCollection(TeamCityBuilds Builds, int QueueSize)
        {
            BlockingCollection<Build> builds = new BlockingCollection<Build>(QueueSize);

            try
            {
                foreach (var b in Builds.Build)
                {
                    builds.Add(b);
                }
            }
            finally
            {
                builds.CompleteAdding();
            }

            return builds;
        }

        private static BlockingCollection<Change> LoadChangesCollection(TeamCityChanges Changes, int QueueSize)
        {
            BlockingCollection<Change> changes = new BlockingCollection<Change>(QueueSize);

            try
            {
                foreach (var c in Changes.Change)
                {
                    changes.Add(c);
                }
            }
            finally
            {
                changes.CompleteAdding();
            }

            return changes;
        }
    }
}
