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
List<TabModel> OpenTabs;




class TabModel
{
    static int TabSeq = 0;
    public TabModel()
    {
        Seq = ++TabSeq;
    }

    
    public int Seq {get; set;}
    public string Title {get; set;} = "<new>";
    public string FullPath {get; set;}
    public DotnetBlayer.Shared.Editor EditorRef {get; set;}
}

TabModel CurrentTab;





public void AddClickEvent( Action onClick)
{
   builder.AddAttribute(++seq, "onclick",
        EventCallback.Factory.Create<UIMouseEventArgs>(new object(), (e) =>
        {                
           onClick();
           StateHasChanged();                                      
        }));
}

if(OpenTabs == null)
{
    OpenTabs = new List<TabModel>();
    OpenTabs.Add(new TabModel{
        Title = "temp.csx"
    });
}

void AddButton(string name, Action onClick)
{
     builder.OpenElement(++seq, "button");
     builder.AddAttribute(++seq, "class", "btn btn-primary");
     builder.AddAttribute(++seq, "onclick",
     EventCallback.Factory.Create<UIMouseEventArgs>(new object(), (e) =>
        {                
           onClick();
           StateHasChanged();                                      
        }));
     builder.AddContent(++seq, name);
     builder.CloseElement();
}



void AddButton(string name, Func<Task> onClick)
{
     builder.OpenElement(++seq, "button");
     builder.AddAttribute(++seq, "class", "btn btn-primary");
     builder.AddAttribute(++seq, "onclick",
     EventCallback.Factory.Create<UIMouseEventArgs>(new object(), async (e) =>
        {                
           await onClick();
           StateHasChanged();                                      
        }));
     builder.AddContent(++seq, name);
     builder.CloseElement();
}

void AddTabButton(TabModel model)
{
    builder.OpenElement(++seq, "span");
    builder.AddAttribute(++seq, "style", "padding:4px 3px");
    AddButton(model.Title, () => CurrentTab = model);
    AddButton("x", () => {OpenTabs.Remove(model);});
    
    builder.CloseElement();
}


Show(b => {
    seq = 0;
    builder = b;

    builder.OpenElement(++seq,"div");

     foreach(var t in OpenTabs)
     {
        AddTabButton(t);
     }

     AddButton("+", () => {OpenTabs.Add(new TabModel{
         Title = "new"
     });});


     builder.CloseElement();
   #region tabsedtior

     var seq2 = 0;
     
     foreach(var t in OpenTabs)
     {

        builder.OpenElement(t.Seq, "div");
        var vis = CurrentTab != null && CurrentTab == t ? "visibility :visible" : "visibility :collapse; height: 0px;";
       
        builder.AddAttribute(t.Seq, "style", vis);
        
      
         builder.OpenComponent<DotnetBlayer.Shared.Editor>(t.Seq);
         builder.AddComponentReferenceCapture(t.Seq, (x) => {
            t.EditorRef = (DotnetBlayer.Shared.Editor)x;
         });
         builder.CloseComponent();
         builder.CloseElement();

     }
     #endregion  tabsedtior

    

});
