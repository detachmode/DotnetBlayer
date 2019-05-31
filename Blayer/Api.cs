using System;
using BlayerUI.Shared;

namespace DotnetBlayer
{
    public class InteractiveBlayerTool : IBlayerTool
    {
        private readonly JsonView view;

        public InteractiveBlayerTool(JsonView view)
        {
            this.view = view;
        }

        public JsonView View()
        {
            System.Console.WriteLine("called VIEW");
            return view;
        }
    }

    public class Api {
        public Action<string> Print { get; set; }
        public Action<IBlayerTool> OnBlayerTool { get; set; }
        public Action Clear { get; set; }

        public void Show(JsonView view)
        {
            OnBlayerTool(new InteractiveBlayerTool(view));
        }

    }
}