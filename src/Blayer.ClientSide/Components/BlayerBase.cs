using DotnetBlayer;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blayer.ClientSide.Components
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
