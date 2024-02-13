//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace Sheenam.Api.Infrastructure.Build
{
    public class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Sheenam Build Pipeline",

                OnEvents = new Events
                {
                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "build",
                        new Job
                        {
                            RunsOn = BuildMachines.Windows2022,

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV2
                                {
                                    Name = "Checking Out Code"
                                },

                                new SetupDotNetTaskV1
                                {
                                    Name = "Setting up .NET",
                                    TargetDotNetVersion = new TargetDotNetVersion
                                    {
                                        DotNetVersion = "7.0.405"
                                    }
                                },

                                new RestoreTask
                                {
                                    Name = "Restoring Nuget Packages"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Building Project"
                                },

                                new TestTask
                                {
                                    Name = "Running Tests"
                                }
                            }
                        }
                    }
                }
            };

            var client = new ADotNetClient();

            client.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: "../../../../.github/workflows/dotnet.yml");
        }
    }
}
