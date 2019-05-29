using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace BlayerUI.Shared
{
    public class BElement
    {
        public virtual string Tag { get; set; }
        public virtual string InnerText { get; set; }
        public virtual Action OnClick { get; set; }
        public virtual List<BElement> InnerElements { get; set; }
    }
}
