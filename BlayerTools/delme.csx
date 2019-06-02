using BlayerUI.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlayerUI.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components;

int seq = 0;
RenderTreeBuilder builder;

Action tag(string tag, string style = "", string cssclass = "", Action onClick = null, Func<string> content = null, params Action[] inner)
{
    return () =>
    {
     builder.OpenElement(++seq, tag);
     builder.AddAttribute(++seq, "style", style); 
     builder.AddAttribute(++seq, "class", cssclass); 

     if(onClick != null)
     {
           builder.AddAttribute(++seq, "onclick",
                    EventCallback.Factory.Create<UIMouseEventArgs>(new object(), (e) =>
                    {                
                       onClick();
                       StateHasChanged();                                      
                    }));
     }

     

     if(content != null) builder.AddContent(++seq, content());
     foreach(var a in inner)
        a();
    builder.CloseElement();
    };
}


Action html(string s) => () => builder.AddMarkupContent(++seq, s);
Action button(string s) => tag("button", content : () => s, cssclass : "btn btn-primary" );
Action button(string s, Action onClick) => tag("button", content : () => s, cssclass : "btn btn-primary", onClick : onClick );
Action text(string s) => tag("span", content : () => s);
Action h1(string s) => tag("h1", content : () => s);
Action div(string style = "", params Action[] inner) => tag("div", style: style, inner:inner);

int times = 1;


var folder = "/Users/dennismuller/csScripting";


void SaveTemp(string content)
{
    var fname = DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss");
    SaveTemp(content, fname);

}

void LoadTemp(string fname)
{
    var fullpath = $"{folder}/{fname}.csx";
    var txt = File.ReadAllText(fullpath);
    SetEditorText(txt);
}
    

void SaveTemp(string content, string fname)
{
    var fullpath = $"{folder}/{fname}.csx";
    File.WriteAllText(fullpath, content);
    Print("stored as temp file.");
}

async Task SaveTemp()
{
    var txt = await GetEditorText();
    SaveTemp(txt);
}

async Task QuickSave(string fname)
{
    var txt = await GetEditorText();
    SaveTemp(txt, fname);
}

void LoadFileIntoEditor(string fpath)
{
    var txt = File.ReadAllText(fpath);
    SetEditorText(txt);
}


Show(b => {
    seq = 0;
    builder = b;
    string[] fileNames = Directory.GetFiles(folder);

    div(style : "padding:20px",
        h1("UI Demo - Updating"),
        html("<input/>"),             
        div(
            style : "color:green; padding:20px",
            () => {
                foreach(var f in fileNames)
                {
                    html($"<span style='padding:6px'>{f}</span>")();
                    button("clickme", () =>  LoadFileIntoEditor(f))();
                    html("<br/>")();               
                }
            } ,
            button("Store Temp", () => {SaveTemp();} )

        )    
    )();

});
