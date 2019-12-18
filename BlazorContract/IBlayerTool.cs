using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blayer.Contracts
{       
    public interface IBlayerTool
    {
        void Render(RenderTreeBuilder builder);
    }
}