using System;
using BlayerUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace DotnetBlayer
{
    public class BlayerBase : ComponentBase
    {
        [Parameter]
        public IBlayerTool Tool { get; set; }
        private int RenderElement(RenderTreeBuilder builder, int seq, BElement uiElement)
        {
            builder.OpenElement(++seq, uiElement.Tag);

            if (uiElement.Tag == "button")
            {
                builder.AddAttribute(++seq, "class", "btn btn-primary");
            }

            if (uiElement.OnClick != null)
            {
                builder.AddAttribute(++seq, "onclick",
                    EventCallback.Factory.Create<UIMouseEventArgs>(this, (e) =>
                    {
                        WithErrorHandling(() => {
                            uiElement.OnClick();
                            this.Invoke(() => StateHasChanged());
                        });
                        
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

        private void WithErrorHandling(Action f)
        {
            try
            {
                f();
            }
            catch (System.Exception e)
            {
                
                Print(e);
            }
        }

        public JsonView _ui { get; private set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            _ui = Tool.View();
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

        private static void Print(object e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

    }
}
