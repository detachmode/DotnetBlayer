using System;
using System.Threading.Tasks;
using BlayerUI.Shared;
using Microsoft.AspNetCore.Components.RenderTree;
using DotnetBlayer;

namespace Blayer.ClientSide.Interactive
{
    public class InteractiveBlayerTool : IBlayerTool
    {

        
        private readonly Action<RenderTreeBuilder> onBuilder;

        public InteractiveBlayerTool(Action<RenderTreeBuilder> builder)
        {
            
            this.onBuilder = builder;
        }

        public void Render(RenderTreeBuilder builder)
        {
         
            System.Console.WriteLine("called RENDER");
            onBuilder(builder);
        }

     
    }

    public class Api {
        private readonly IServiceProvider provider;

        public Api(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public Interactive<Api> Interactive => GetService<Interactive<Api>>();

        public T GetService<T>() => (T)provider.GetService(typeof(T));

        // public Blayer.Shared.Components.Editor Editor { get; set; }
        // public IndexBase Self { get; set; }
        public Action<string> Print { get; set; }
        public  Action StateHasChanged { get; set; }
        public Func<Task<string>> GetEditorText { get; set; }
         public Action<string> SetEditorText { get; set; }

        public Action<IBlayerTool> OnBlayerTool { get; set; }
        public Action Clear { get; set; }

        public void Show(Action<RenderTreeBuilder> onBuilder)
        {
            OnBlayerTool(new InteractiveBlayerTool(onBuilder));
        }

    }
}