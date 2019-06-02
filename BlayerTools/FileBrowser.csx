#!/usr/bin/env dotnet-script
#load "./ExperimentalUISyntax1.csx"
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


using System.Runtime.CompilerServices;
// Work-around helper method to get the source file location.
private static string GetSourceFile([CallerFilePath] string file = "") => file;

var folder = Path.GetDirectoryName(GetSourceFile());

Print(folder);


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

async Task SaveCurrentUnder(string fullPath)
{
    var content = await GetEditorText();
    File.WriteAllText(fullPath, content);
    Print("stored as " + fullPath);
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

string currentFile = null;

Show(b => {
    seq = 0;
    builder = b;
    string[] fileNames = Directory.GetFiles(folder);


    div(style : "padding:20px",
        h1("FileBrowser"),
        html("<input/>"),             
        div(
            style : "color:green; padding:20px",
            () => {
                foreach(var f in fileNames)
                {

                    button(f, () =>  
                    {
                        LoadFileIntoEditor(f);
                        currentFile = f;
                    })();
                    html("<br/>")();               
                }
            } ,
            html("<br/>"),
            html($"<p>Open: {currentFile}</p>"),
            button("Save Current Open File", () => {
                if(currentFile != null)
                {
                    SaveCurrentUnder(currentFile);
                }
                else
                {
                  SaveTemp();
                }
  
                
            } ),
            button("Open Code", () => 
            {
                Process.Start("code", folder);               
            })

        )    
    )();

});
