using System;
using BlayerUI.Shared;
using DotnetBlayer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blayer.Shared
{
    public class BlayerBase : ComponentBase
    {
        [Parameter] public IBlayerTool Tool { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Tool.Render(builder);
            base.BuildRenderTree(builder);
        }

    }
}
