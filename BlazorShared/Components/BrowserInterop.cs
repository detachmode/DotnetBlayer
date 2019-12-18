using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blayer.Shared.Components
{
    static class BrowserInterop
    {
        public static async Task<double> GetScrollTop(this IJSRuntime JSRuntime, string id)
        {
            return await JSRuntime.InvokeAsync<double>("helper.getScrollTop",id);
        }
        public static async Task<bool> IsScrolledToBottom(this IJSRuntime JSRuntime, string id)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>("helper.isScrolledToBottom", id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BrowserInterop.IsScrolledToBottom: {ex.GetBaseException().Message}");
            }
            return false;
        }
        public static async Task<bool> ScrollIntoView(this IJSRuntime JSRuntime, string id)
        {
            return await JSRuntime.InvokeAsync<bool>("helper.scrollIntoView", id);
        }
        public static async Task<bool> SetFocus(this IJSRuntime JSRuntime, ElementReference elementRef)
        {
            return await JSRuntime.InvokeAsync<bool>("helper.setFocus", elementRef);
        }
    }
}