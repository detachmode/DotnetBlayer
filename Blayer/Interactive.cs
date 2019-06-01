using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetBlayer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;

namespace DotnetBlayer
{
    static class BrowserInterop
    {
        public static Task<double> GetScrollTop(this IJSRuntime JSRuntime, string id)
        {
            return JSRuntime.InvokeAsync<double>("helper.getScrollTop",id);
        }
        public static Task<bool> IsScrolledToBottom(this IJSRuntime JSRuntime, string id)
        {
            try
            {
                return JSRuntime.InvokeAsync<bool>("helper.isScrolledToBottom", id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BrowserInterop.IsScrolledToBottom: {ex.GetBaseException().Message}");
            }
            return Task.FromResult(false);
        }
        public static Task<bool> ScrollIntoView(this IJSRuntime JSRuntime, string id)
        {
            return JSRuntime.InvokeAsync<bool>("helper.scrollIntoView", id);
        }
        public static Task<bool> SetFocus(this IJSRuntime JSRuntime, ElementRef elementRef)
        {
            return JSRuntime.InvokeAsync<bool>("helper.setFocus", elementRef);
        }
    }

    public static class Extension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
     (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    public class Interactive<T>
    {
        private ScriptState state;
        private object _lock = new object();
        private readonly T globals;

        public Interactive(T globals)
        {
            this.globals = globals;
        }

        public System.Collections.Generic.List<ScriptVariable> GetAllVariables()
        {
            return state.Variables
                .Reverse()
                .DistinctBy(x => x.Name)
                .Reverse()
                .ToList();
        }




        public async Task Init()
        {
            state = await InitStateAsync();
        }

        public async Task<string> Eval(string code)
        {
            try
            {
                state = await state.ContinueWithAsync(code);
                return state.ReturnValue?.ToString() ?? "<empty>";

            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        private async Task<ScriptState> InitStateAsync()
        {
            var options = ScriptOptions.Default
                  .WithReferences(typeof(Program).Assembly)
                  .WithImports(
                      "System.Collections.Generic",
                      "System",
                      "System.Linq"
                      );

            var finalScript = CSharpScript.Create("", options, typeof(T));
            finalScript.Compile();
            ScriptState<object> result = await finalScript.RunAsync(globals);
            return result;
        }


    }
}