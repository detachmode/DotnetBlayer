using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Publish);

   AbsolutePath SourceDirectory => RootDirectory / "src";
   AbsolutePath TestDirectory => RootDirectory / "src" / "test";
   AbsolutePath ProjectClientSide => RootDirectory / "src" / "Blayer.ClientSide";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/*/bin", "**/*/obj").ForEach(DeleteDirectory);
        });


    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(ProjectClientSide));
         
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            // DotNetTest(s => s
            // .SetArgumentConfigurator(a => a
            //     .Add("/p:CollectCoverage=true")
            //     .Add("/p:CoverletOutputFormat=\\\"opencover,lcov\\\"")
            //     .Add("/p:CoverletOutput=../lcov"))
            // .SetProjectFile(TestDirectory));
        });
    Target Publish => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            // DotNetPublish(s => s
            // .EnableNoBuild()
            // .SetOutput(ArtifactsDirectory)
            // .SetProject(Solution));
        });

}