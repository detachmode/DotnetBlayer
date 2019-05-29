using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlayerUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace DotnetBlayer
{
    public class MyBlaTool : ComponentBase, IBlayerTool
    {
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(++seq, "h2");
            builder.AddContent(++seq, "hello world");
            builder.CloseElement();
        }
    }

    public class MyBlaTool2 : ComponentBase, IBlayerTool
    {

        private int counter = 0;
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            BElement time = new BBText("");
            if (ShowTime)
            {
                time = new BDiv
                {
                    InnerElements = new List<BElement>
                    {
                        new BBText(CurrentTime)
                    }
                };
            }
            _ui = new JsonView(
                new BDiv
                {
                    InnerElements = new List<BElement>
                    {
                        new BBHeader(SomeText ),
                        new BBText(title),
                        new BButton
                        {
                            InnerText = "Click Me!",
                            OnClick = () => Print("hi")
                        },

                        new BButton
                        {
                            InnerText = "Toogle Show Time",
                            OnClick = () => ShowTime = !ShowTime
                        },
                        time

                    }
                }
            );

            System.Diagnostics.Debug.WriteLine(counter++);
            var seq = 0;

            try
            {
                foreach (var uiElement in _ui.Elements)
                {
                    RenderElement(builder, seq, uiElement);
                }
            }
            catch (System.Exception e)
            {
                Print(e);
            }

            base.BuildRenderTree(builder);
        }

        public string SomeText { get; set; } = "hi";
        public bool ShowTime { get; set; }

        private static void Print(object e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        private int RenderElement(RenderTreeBuilder builder, int seq, BElement uiElement)
        {
            builder.OpenElement(++seq, uiElement.Tag);

            if (uiElement.OnClick != null)
            {
                builder.AddAttribute(++seq, "class", "btn btn-primary");
                builder.AddAttribute(++seq, "onclick",
                    EventCallback.Factory.Create<UIMouseEventArgs>(this, (e) =>
                    {
                        uiElement.OnClick();
                    }));
            }

            if (uiElement.InnerText != null)
            {
                builder.AddContent(++seq, uiElement.InnerText);
            }
            if (uiElement.InnerElements != null)
            {
                foreach (var item in uiElement.InnerElements)
                {
                    RenderElement(builder, seq, item);
                }
            }
            builder.CloseElement();
            return seq;
        }

        protected string title = "BuildRenderTree Action";

        public MyBlaTool2()
        {
            CurrentTime = DateTime.Now.ToLongTimeString();

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += (a, b) =>
            {
                System.Console.WriteLine("executed TIMER");
                SomeText = Guid.NewGuid().ToString();
                CurrentTime = DateTime.Now.ToLongTimeString();
                this.Invoke(() => StateHasChanged());

            };
            aTimer.Interval = 500;
            aTimer.Enabled = true;

        }

        public JsonView _ui { get; private set; }
        public string CurrentTime { get; private set; }
    }
}
