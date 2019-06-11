using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
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

    public static int Main() => Execute<Build>(x => x.PublishSubtree);

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "gh-pages";
    AbsolutePath TestDirectory => RootDirectory / "src" / "test";
    AbsolutePath ProjectClientSide => RootDirectory / "src" / "Blayer.ClientSide";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .Executes(() =>
        {

            try
            {
                ProcessTasks.StartProcess("git", " worktree remove gh-pages").WaitForExit();
            }
            catch (System.Exception)
            {

            }

            SourceDirectory.GlobDirectories("**/*/bin", "**/*/obj").ForEach(DeleteDirectory);
            DeleteDirectory("_site");
            EnsureCleanDirectory(ArtifactsDirectory);


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
            DotNetPublish(s => s
            .EnableNoBuild()
            .SetOutput(ArtifactsDirectory)
            .SetProject(ProjectClientSide));
        });


    Target PublishSubtree => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {


            // var cmds = new[] {
            //     ("git", "--version", "."),
            //     ("git", "worktree add _site gh-pages", "."),
            //     ("echo", "hello >> hi.txt", "_site"),
            //     ("git", "add -A", "_site"),
            //     ("git", "status", "_site"),

            //     // "git config user.email you@you.com",
            //     // "git config user.email you@you.com",
            //     // "git config user.name \"your name\"",
            //     ("git", "commit -a -m \"Commit from Azure DevOps\"", "_site"),
            //     ("git", "push", "_site"),
            // };

            try
            {
                ProcessTasks.StartProcess("git", "worktree prune").WaitForExit();
                ProcessTasks.StartProcess("git", "worktree add _site gh-pages").WaitForExit();
                // System.Console.WriteLine("Added Working Tree");
                CopyDirectoryRecursively(
                    ArtifactsDirectory / "Blayer.ClientSide" / "dist",
                    RootDirectory / "_site",
                    DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite);
                System.Console.WriteLine("Copied Recusively");

                ProcessTasks.StartProcess("git", "add -A", "_site").WaitForExit();
                ProcessTasks.StartProcess("git", "status", "_site").WaitForExit();
                ProcessTasks.StartProcess("git", " commit -m \"commit from nuke\"", "_site").WaitForExit();
                ProcessTasks.StartProcess("git", "push", "_site").WaitForExit();
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex.ToString());
                throw;
            }
            finally
            {
                //   ProcessTasks.StartProcess("git", "worktree remove gh-pages").WaitForExit();
            }

        });

}
