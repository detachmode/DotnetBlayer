namespace Blayer.Shared.Components
{
    static class BrowserInterop
    {
        public static Task<double> GetScrollTop(this IJSRuntime JSRuntime, string id)
        {
            return JSRuntime.InvokeAsync<double>("helper.getScrollTop",id);
        }
        public static Task<bool> IsScrolledToBottom(this IJSRuntime JSRuntime, string id)
        {
            try
            {
                return JSRuntime.InvokeAsync<bool>("helper.isScrolledToBottom", id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BrowserInterop.IsScrolledToBottom: {ex.GetBaseException().Message}");
            }
            return Task.FromResult(false);
        }
        public static Task<bool> ScrollIntoView(this IJSRuntime JSRuntime, string id)
        {
            return JSRuntime.InvokeAsync<bool>("helper.scrollIntoView", id);
        }
        public static Task<bool> SetFocus(this IJSRuntime JSRuntime, ElementRef elementRef)
        {
            return JSRuntime.InvokeAsync<bool>("helper.setFocus", elementRef);
        }
    }
}