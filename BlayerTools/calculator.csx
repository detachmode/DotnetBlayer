#!/usr/bin/env dotnet-script
#load "/Users/dennismuller/DotnetBlayer/BlayerTools/OtherUISyntax.csx"
using BlayerUI.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components;

int sum = 0;
string exp = "";
var topDisplayText = "";

Action DisplayText()
{
     return  text(topDisplayText);
}

void AddToExpression(string input)
{
    var show = "";
    switch(input)
    {
        case "C" :
            sum = 0;
            exp = "";
            show = "";
            break;
        case "=" :
            sum += int.Parse(exp);
            exp = "";
            show = sum.ToString();     
            break;
        case "+" :
            sum += int.Parse(exp);
            exp = "";
            show = sum.ToString();     
            break;
        default: 
            exp += input;
            show = exp;
            break;

    }
 
    Print(show);
    topDisplayText = show;
}

Action Button(string number) 
{
    return button(number,  () => AddToExpression(number)); 

}




Show(b => {
    builder = b;
    seq = 0;

 div(
     style : "",
        h1("Calculator"),
        text( "Result: " + topDisplayText),
  
        div( 
            style : "",
           Button("1"), Button("2"), Button("3")
        ),
        div(
           style : "",
           Button("4"), Button("5"), Button("6")  
        ),
        div(
            style : "",
           Button("7"), Button("8"), Button("9")  
        )  ,
        div(
            style : "",
           Button("+"),  Button("=") , Button("C") 
        )  
    )();

});





