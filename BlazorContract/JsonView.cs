using System.Collections.Generic;
using System.Linq;

namespace Blayer.Contracts
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
