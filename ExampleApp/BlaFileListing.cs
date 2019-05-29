using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BlayerUI.Shared;

namespace DotnetBlayer
{
    public class BlaFileListing : IBlayerTool
    {
        public string[] fileNames { get; set; }

        public JsonView View()
        {
            return new JsonView(
                new BDiv(
                    new BBHeader("List Downloads"),
                    new BButton("List files in folder")
                    {
                        OnClick = () =>
                        {
                            fileNames = Directory.GetFiles(@".");
                        }
                    },
                    new BDiv
                    (
                        fileNames?
                            .Select(f => (BElement)new BBText(f)
                            {
                                OnClick = () => Process.Start("explorer", f)
                            })
                            .ToArray()
                    )
                )
            );
        }
    }
}
