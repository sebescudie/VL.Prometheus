using System;
using System.Linq;
using Fallout.Common;
using Fallout.Common.CI;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Solutions;
using Fallout.Common.Tooling;
using Fallout.Common.Utilities.Collections;
using static Fallout.Common.EnvironmentInfo;
using static Fallout.Common.IO.PathConstruction;
using Fallout.Common.Tools.NuGet;
using System.IO;
using Fallout.Common.Tools.Git;

class Build : FalloutBuild
{
    public static int Main () => Execute<Build>(x => x.Clean);

    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath NuspecFile => RootDirectory / "deployment/VL.Prometheus.nuspec";
    AbsolutePath VersionFile => RootDirectory / "version.txt";

    [Parameter("Nuget feed URL")]
    readonly string Source = "https://api.nuget.org/v3/index.json";

    [Parameter("Nuget feed API Key")]
    [Secret]
    readonly string ApiKey;

    string Version => File.ReadAllText(VersionFile).Trim();

    Target Clean => _ => _
        .Before()
        .Executes(() =>
        {
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Pack => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            NuGetTasks.NuGetPack(f => f
                .SetTargetPath(NuspecFile)
                .SetVersion(Version)
                .SetOutputDirectory(OutputDirectory));
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            NuGetTasks.NuGetPush(p => p
                .SetSource(Source)
                .SetApiKey(ApiKey));
        });

    Target Tag => _ => _
        .Executes(() =>
        {
            GitTasks.Git($"tag -a {Version} -m \"VL Prometheus {Version}\"");
        });

    Target Release => _ => _
    .DependsOn(Tag, Push)
    .Requires(() => GitTasks.GitHasCleanWorkingCopy())
    .Executes(() =>
    {
        // Meta target, only calls Tag and Push, which themselves
        // make the whole pipeline run
    });
}