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


Show(b => {
    seq = 0;
    builder = b;

    div(style : "padding:20px",
        h1("UI Demo - Updating"),
        div(
            style : "color:green; padding:20px",
            () => {
                for(var i = 0; i < times; i++)
                {
                    html($"<span style='padding:6px'>Duplicate</span>")();
                    button("clickme", () => times++)();
                    html("<br/>")();               
                }
            },
           
            button("Reset", () => times = 1)
        )    
    )();

});
