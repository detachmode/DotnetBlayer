using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Blayer.Shared.Utils;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;

namespace Blayer.ClientSide.Interactive
{


    public class Interactive<T>
    {
        private readonly T globals;
        public ScriptState State { get; private set; }
        private CSharpCompilation _previousCompilation;
        private IEnumerable<MetadataReference> _references;
        private object[] _submissionStates;
        private int _submissionIndex = 0;

        public Interactive(T api)
        {
            globals = api;
            _submissionStates = new object[] { globals, null };
        }

        public async Task Init()
        {
            var refs = AppDomain.CurrentDomain.GetAssemblies();
            var client = new HttpClient
            {
                BaseAddress = new Uri(WebAssemblyUriHelper.Instance.GetBaseUri())
            };

            var references = new List<MetadataReference>();

            foreach (var reference in refs.Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location)))
            {
                var stream = await client.GetStreamAsync($"_framework/_bin/{reference.Location}");
                references.Add(MetadataReference.CreateFromStream(stream));
            }

            _references = references;
        }

        public async Task<CompilatonResult> RunSubmission(string code)
        {
            var previousOut = Console.Out;
            try
            {
                if (TryCompile(code, out var script, out var errorDiagnostics))
                {
                    var writer = new StringWriter();
                    Console.SetOut(writer);



                    var entryPoint = _previousCompilation.GetEntryPoint(CancellationToken.None);
                    var type = script.GetType($"{entryPoint.ContainingNamespace.MetadataName}.{entryPoint.ContainingType.MetadataName}");

                    var entryPointMethod = type.GetMethod(entryPoint.MetadataName);
                    // System.Console.WriteLine(entryPointMethod.Name);
                    // foreach (var e in entryPointMethod.GetParameters())
                    // {
                    //     System.Console.WriteLine(e.Name);
                    // }

                    var submission = (Func<object[], Task>)entryPointMethod
                        .CreateDelegate(typeof(Func<object[], Task>));



                    if (_submissionIndex >= _submissionStates.Length)
                    {
                        Array.Resize(ref _submissionStates, Math.Max(_submissionIndex, _submissionStates.Length * 2));
                    }


                    var returnValue = await ((Task<object>)submission(_submissionStates));
                    if (returnValue != null)
                    {
                        Console.WriteLine(CSharpObjectFormatter.Instance.FormatObject(returnValue));
                    }

                    var output = HttpUtility.HtmlEncode(writer.ToString());
                    return CompilatonResult.Success(output);

                }
                else
                {
                    var errors = "";
                    foreach (var diag in errorDiagnostics)
                    {
                        errors += HttpUtility.HtmlEncode(diag);
                    }
                    return CompilatonResult.Error(errorDiagnostics.Select(HttpUtility.HtmlEncode).ToArray());
                }
            }
            catch (Exception ex)
            {
                var err = HttpUtility.HtmlEncode(CSharpObjectFormatter.Instance.FormatException(ex));
                return CompilatonResult.Error(err);
            }
            finally
            {
                Console.SetOut(previousOut);
            }
        }

        private List<(object state, FieldInfo info)> GetVariablesInternal()
        {

            return _submissionStates.SelectMany(state =>
            {
                var result = new List<(object state, FieldInfo info)>();
                System.Console.WriteLine(state.GetType().Name);
                System.Console.WriteLine(state.GetType().GetTypeInfo().Name);
                var sta = state.GetType().GetTypeInfo().DeclaredFields;
                foreach (var field in sta)
                {
                    System.Console.WriteLine("Field - " + field.Name);
                    if (field.IsPublic && field.Name.Length > 0 && (char.IsLetterOrDigit(field.Name[0]) || field.Name[0] == '_'))
                    {
                        result.Add((state, field));
                    }
                }
                return result;
            }).Reverse()
                .DistinctBy(x => x.info.Name)
                .Reverse()
                .ToList();

        }

        public List<(string name, string value)> GetVariables()
        {
            var vars = GetVariablesInternal();
            return vars
            .Select( v => (v.info.Name, v.info.GetValue(v.state).ToString()))
            .ToList();
        }



        private bool TryCompile(string source, out Assembly assembly, out IEnumerable<Diagnostic> errorDiagnostics)
        {
            assembly = null;
            var scriptCompilation = CSharpCompilation.CreateScriptCompilation(
                Path.GetRandomFileName(),
                CSharpSyntaxTree.ParseText(source, CSharpParseOptions.Default.WithKind(SourceCodeKind.Script).WithLanguageVersion(LanguageVersion.Preview)),
                _references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, usings: new[]
                {
                    "System",
                    "System.IO",
                    "System.Collections.Generic",
                    "System.Console",
                    "System.Diagnostics",
                    "System.Dynamic",
                    "System.Linq",
                    "System.Linq.Expressions",
                    "System.Net.Http",
                    "System.Text",
                    "System.Threading.Tasks"
                }),
                _previousCompilation,
                globalsType: typeof(T)
            );

            errorDiagnostics = scriptCompilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error);
            if (errorDiagnostics.Any())
            {
                return false;
            }

            using (var peStream = new MemoryStream())
            {
                var emitResult = scriptCompilation.Emit(peStream);

                if (emitResult.Success)
                {
                    _submissionIndex++;
                    _previousCompilation = scriptCompilation;
                    assembly = Assembly.Load(peStream.ToArray());
                    return true;
                }
            }

            return false;
        }

        internal async Task<string> Eval(string code)
        {
            var res = await this.RunSubmission(code);
            return res.IsSuccess ? res.Output : res.Errors.First();
        }
    }

    public class CompilatonResult
    {

        private CompilatonResult() { }
        public static CompilatonResult Success(string output)
        {

            return new CompilatonResult()
            {
                IsSuccess = true
                ,
                Output = output
            };
        }

        public static CompilatonResult Error(List<string> errors)
        {

            return new CompilatonResult()
            {
                IsSuccess = false,
                Errors = errors
            };
        }
        public static CompilatonResult Error(params string[] errors)
        {

            return new CompilatonResult()
            {
                IsSuccess = false,
                Errors = errors.ToList()
            };
        }
        public string Output { get; private set; }
        public bool IsSuccess { get; private set; }
        public List<string> Errors { get; set; }

    }
}