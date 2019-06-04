using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.JSInterop;

namespace DotnetBlayer.Pages
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
        public DotnetBlayer.Shared.Editor Editor { get; set; }
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

        protected override async Task OnAfterRenderAsync()
        {
            if (!InitalLoadDone)
            {

                var code = await new HttpClient().GetStringAsync("https://localhost:5556/examples/example1.txt");
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




        public async Task CinKeyDown(UIKeyboardEventArgs arg)
        {
            if (arg.Key == "Enter")
            {
                await Run(Code);
            }
        }

        protected override async Task OnInitAsync()
        {

            Results = new List<CoutRes>();

            CurrentLayout = DefaultLayout;



            await Interactive.Init();

            Api.Clear = () =>
            {
                this.Invoke(() =>
                {
                    Results.Clear();
                    StateHasChanged();
                });
            };

            Api.SetEditorText = (c) =>
            {
                this.Invoke(() =>
                {
                    Editor.SetValue(c);
                });
            };

            Api.GetEditorText = () =>
            {
                return Editor.GetValue();
            };

            Api.OnBlayerTool = (t) =>
            {
                this.Invoke(() =>
                {
                    Tool = t;
                    StateHasChanged();
                });
            };
            Api.StateHasChanged = () =>
            {
                this.Invoke(() =>
                {
                    StateHasChanged();
                });
            };

            Api.Print = (txt) =>
            {
                this.InvokeAsync(async () =>
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

            await this.Invoke(() =>
           {
               StateHasChanged();
           });

            await Task.Delay(10);


            await JSRuntime.ScrollIntoView("lastCout");
        }


    }

}


