using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using bla.BlazorShared.Components;
using bla.Monaco;
using Blayer.Contracts;
using Blayer.Shared.Components;
using Blayer.Shared.Interactive;
using Blazaco.Editor;
using Blazaco.Editor.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.JSInterop;

namespace DotnetBlayer.pages
{

    public class IndexBase : ComponentBase
    {
        [Inject] public Interactive<Api> Interactive { get; set; }
        [Inject] Api Api { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }


        public class LayoutTemplate
        {
            public string Area { get; set; }
            public string Columns { get; set; }
            public string Rows { get; set; }
        }

        public bool IsFullScreen { get; set; }


        private LayoutTemplate DefaultLayout = new LayoutTemplate
        {
            Area = @"
        'top top'
        'edit ui'
        'vars cout '
        'vars cin '
        ",
            Rows = "30px 1fr 100px 30px",
            Columns = "50vw 50vw"
        };

        private LayoutTemplate FullUi = new LayoutTemplate
        {
            Area = @"
        'top top'
        'edit ui'
        'vars cout '
        'vars cin '
        ",
            Rows = "30px 1fr 0px 0px",
            Columns = "0vw 100vw"
        };

        public IBlayerTool Tool { get; set; }
        public Editor Editor { get; set; }
        public string Code { get; set; }
        public LayoutTemplate CurrentLayout { get; set; }
        public List<CoutRes> Results { get; set; }
        public CoutRes LastResult { get; set; }
        public List<ScriptVariable> AllVariables { get; set; }
        public bool InitalLoadDone { get; set; }


        public void ToggleEditor()
        {
            CurrentLayout = CurrentLayout == DefaultLayout ? FullUi : DefaultLayout;
        }




        public async Task ToogleFullScreen()
        {
            var jsfun = IsFullScreen ? "closeFullscreen" : "openFullscreen";
            IsFullScreen = !IsFullScreen;
            StateHasChanged();
            await JSRuntime.InvokeAsync<bool>(jsfun);
            await Editor.Layout();
        }

        protected override async Task OnAfterRenderAsync(bool first)
        {
            if (!InitalLoadDone)
            {

                var code = await new HttpClient().GetStringAsync("https://localhost:5556/samples/calc.txt");
                await Editor.SetValue(code);
                Api.Editor = Editor;


                await Editor.RegisterAction(
                "dotnetblayer.editor.eval",
                "Eval",
                () => Eval());


                await Editor.RegisterAction(
                    "dotnetblayer.editor.run", "Run", () => Run());


                InitalLoadDone = true;


            }
        }
        public async Task EvalSel()
        {
            var sel = await Editor.GetSelectedText();
            await Run(sel);
        }
        public async Task EvalLine()
        {
            var sel = await Editor.GetCurrentLineText();
            await Run(sel);
        }

        public async Task Eval()
        {
            var code = await Editor.GetSelectedText();
            if (string.IsNullOrWhiteSpace(code))
                code = await Editor.GetCurrentLineText();

            await Run(code);
        }



        public async Task ShowErrors(IEnumerable<Diagnostic> errors)
        {
            // var code = await Editor.GetValue();
            // var errors = await Interactive.GetErrors(code);
            // await Editor.ResetDecorations();

        }




        public async Task CinKeyDown(KeyboardEventArgs arg)
        {
            if (arg.Key == "Enter")
            {
                await Run(Code);
            }
        }



        protected override async Task OnInitializedAsync()
        {

        

            Results = new List<CoutRes>();

            CurrentLayout = DefaultLayout;



            await Interactive.Init();

            Api.Clear = () =>
            {
                InvokeAsync(() =>
                {
                    Results.Clear();
                    StateHasChanged();
                });
            };

            Api.SetEditorText = async (c) =>
            {
                await InvokeAsync(() =>
                {
                    Editor.SetValue(c);
                });
            };

            Api.GetEditorText = () => Editor.GetValue();

            Api.OnBlayerTool = (t) =>
            {
                this.InvokeAsync(() =>
                {
                    Tool = t;
                    StateHasChanged();
                });
            };
            Api.StateHasChanged = async () =>
            {
                await this.InvokeAsync(StateHasChanged);
            };

            Api.Print = async (txt) =>
            {
                await this.InvokeAsync(async () =>
                {
                    await Print(txt);
                });
            };

            Api.Self = this;







            //MainViewModel.InteractiveOut = (s) => Results.Add(s);
        }



        public class CoutRes
        {
            public string Text { get; set; }
        }

        public async Task Run()
        {
            var val = await Editor.GetValue();
            await Run(val);
        }
        public async Task Run(string code)
        {
            var res = await Interactive.Eval(code);
            await Print(res);

            AllVariables = Interactive.GetAllVariables();

        }

        public async Task Print(string res)
        {
            var cout = new CoutRes
            {
                Text = res
            };
            Results.Add(cout);
            LastResult = cout;

            await InvokeAsync(StateHasChanged);

            await Task.Delay(10);


            await JSRuntime.ScrollIntoView("lastCout");
        }


    }

}


