using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.JSInterop;

namespace DotnetBlayer
{
    public class Interactive<T>
    {
        public ScriptState State { get; private set; }
        private readonly T globals;

        public Interactive(T globals)
        {
            this.globals = globals;
        }

        public System.Collections.Generic.List<ScriptVariable> GetAllVariables()
        {
            return State.Variables
                .Reverse()
                .DistinctBy(x => x.Name)
                .Reverse()
                .ToList();
        }




        public async Task Init()
        {
            SearchPaths = new List<string>(new[] {
                "DotnetBlayer",
                "DotnetBlayer/BlayerTools",
            });
            State = await InitStateAsync();

        }
        public async Task<IEnumerable<Diagnostic>> GetErrors(string code)
        {
            var opts = ScriptOptions.Default.
                AddImports("System");

            var script = CSharpScript.Create(code, opts);
            var compilation = script.GetCompilation();
            var diagnostics = compilation.GetDiagnostics();
            return diagnostics;

        }




        

        


        public async Task<string> Eval(string code)
        {
            try
            {
                State = await State.ContinueWithAsync(code);

                return State.ReturnValue?.ToString() ?? "OK";

            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public List<string> SearchPaths { get; set; }

        private async Task<ScriptState> InitStateAsync()
        {
            var paths = ImmutableArray<string>.Empty.AddRange(SearchPaths);


            var options = ScriptOptions.Default
                .WithReferences(typeof(Program).Assembly)
                .WithSourceResolver(new SourceFileResolver(
                    paths, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
                .WithImports(
                    "System",
                    "System.Linq",
                    "System.Threading.Tasks",
                    "BlayerUI.Shared",
                    "DotnetBlayer",
                    "Microsoft.AspNetCore.Components.RenderTree",
                    "Microsoft.Extensions.DependencyInjection",
                    "System.Collections.Generic"
                      );

            var finalScript = CSharpScript.Create("", options, typeof(T));
            finalScript.Compile();
            ScriptState<object> result = await finalScript.RunAsync(globals);
            return result;
        }


    }
}