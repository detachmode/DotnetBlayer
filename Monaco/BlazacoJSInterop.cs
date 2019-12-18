using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Blazaco.Editor
{
	public static class BlazacoJSInterop
    {
    public static async Task<bool> InitializeEditor(IJSRuntime runtime, EditorModel editorModel)
    {
      return await runtime.InvokeAsync<bool>("Blazaco.Editor.InitializeEditor", new[] { editorModel });
    }

    public static async Task<string> GetValue(IJSRuntime runtime, string id)
    {
      return await runtime.InvokeAsync<string>("Blazaco.Editor.GetValue", new[] { id });
    }

    public static async Task<bool> SetValue(IJSRuntime runtime, string id, string value)
    {
      return await runtime.InvokeAsync<bool>("Blazaco.Editor.SetValue", new[] { id, value });
    }

    public static async Task<bool> SetTheme(IJSRuntime runtime, string id, string theme)
    {
      return await runtime.InvokeAsync<bool>("Blazaco.Editor.SetTheme", new[] { id, theme });
    }
    }
}
