using System.Collections.Generic;
using System.Linq;

namespace BlayerUI.Shared
{
    public class JsonView
    {
        public JsonView()
        {
        }

        public JsonView(params BElement[] elements)
        {
            Elements = elements.ToList();
        }

        public List<BElement> Elements { get; set; }
    }
}
