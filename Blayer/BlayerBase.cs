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
        // private int RenderElement(RenderTreeBuilder builder, int seq, BElement uiElement)
        // {
        //     builder.OpenElement(++seq, uiElement.Tag);

        //     if (uiElement.Tag == "button")
        //     {
        //         builder.AddAttribute(++seq, "class", "btn btn-primary");
        //     }

        //     if (uiElement.OnClick != null)
        //     {
        //         builder.AddAttribute(++seq, "onclick",
        //             EventCallback.Factory.Create<UIMouseEventArgs>(this, (e) =>
        //             {
        //                 WithErrorHandling(() => {
        //                     uiElement.OnClick();
        //                     this.Invoke(() => StateHasChanged());
        //                 });
                        
        //             }));
        //     }

        //     if (uiElement.InnerText != null)
        //     {
        //         builder.AddContent(++seq, uiElement.InnerText);
        //     }
        //     if (uiElement.InnerElements != null)
        //     {
        //         foreach (var item in uiElement.InnerElements)
        //         {
        //             RenderElement(builder, seq, item);
        //         }
        //     }
        //     builder.CloseElement();
        //     return seq;
        // }

        // private void WithErrorHandling(Action f)
        // {
        //     try
        //     {
        //         f();
        //     }
        //     catch (System.Exception e)
        //     {
                
        //         Print(e);
        //     }
        // }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Tool.Render(builder);

            base.BuildRenderTree(builder);
        }


    }
}
