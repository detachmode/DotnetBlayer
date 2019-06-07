using BlayerUI.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

string[] fileNames;


var div = new BDiv();
var jsonview = 
new JsonView(
    new BDiv(
        new BBHeader("List Downloads"),
        new BButton("List it fooo!")
        {
            OnClick = () =>
            {
                Print("hello");
                fileNames = Directory.GetFiles(@".");
                
                 var inner = fileNames?
                        .Select(f => (BElement)new BBText(f)
                        {
                            OnClick = () => Process.Start("explorer", f)
                        })
                        .ToList();
                div.InnerElements = inner;                
                
            }
        },
        div
        
    )
);

Show(jsonview);
