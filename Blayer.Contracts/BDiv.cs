using System.Linq;

namespace BlayerUI.Shared
{
    public class BDiv : BElement
    {
        public BDiv(params BElement[] inner)
        {
            InnerElements = inner?.ToList();
        }

        public new string Tag => "div";
    }
}
