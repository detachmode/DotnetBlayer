using BlayerUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace DotnetBlayer
{       
    public interface IBlayerTool
    {
        JsonView View();
    }
}