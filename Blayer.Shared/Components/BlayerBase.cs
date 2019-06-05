using DotnetBlayer;

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
