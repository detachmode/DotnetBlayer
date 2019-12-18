using System;
using Blayer.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blayer.Shared.Components
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
