// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using BlayerUI.Shared;
// using Microsoft.AspNetCore.Components;

// namespace DotnetBlayer
// {
//     public class MyBlaTool : ComponentBase, IBlayerTool
//     {
//         public JsonView View()
//         {
//             throw new NotImplementedException();
//         }

//         protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
//         {
//             var seq = 0;
//             builder.OpenElement(++seq, "h2");
//             builder.AddContent(++seq, "hello world");
//             builder.CloseElement();
//         }
//     }

//     public class MyBlaTool2 : BlayerBase, IBlayerTool
//     {

//         public string CurrentTime { get; private set; }
//         public string SomeText { get; set; } = "hi";
//         public bool ShowTime { get; set; }
//         protected string title = "BuildRenderTree Action";
//         private int counter = 0;

//         protected override JsonView View()
//         {
//             BElement time = new BBText("");
//             if (ShowTime)
//             {
//                 time = new BDiv
//                 {
//                     InnerElements = new List<BElement>
//                     {
//                         new BBText(CurrentTime)
//                     }
//                 };
//             }

//             return new JsonView(
//                 new BDiv
//                 {
//                     InnerElements = new List<BElement>
//                     {
//                         new BBHeader(SomeText ),
//                         new BBText(title),
//                         new BButton
//                         {
//                             InnerText = "Click Me!",
//                             OnClick = () => {}
//                         },

//                         new BButton
//                         {
//                             InnerText = "Toogle Show Time",
//                             OnClick = () => ShowTime = !ShowTime
//                         },
//                         time
//                     }
//                 }
//             );

//         }

//         JsonView IBlayerTool.View()
//         {
//             throw new NotImplementedException();
//         }

//         public MyBlaTool2()
//         {
//             CurrentTime = DateTime.Now.ToLongTimeString();

//             System.Timers.Timer aTimer = new System.Timers.Timer();
//             aTimer.Elapsed += (a, b) =>
//             {
//                 CurrentTime = DateTime.Now.ToLongTimeString();
//                 this.Invoke(() => StateHasChanged());

//             };
//             aTimer.Interval = 500;
//             aTimer.Enabled = true;

//         }

//     }
// }
