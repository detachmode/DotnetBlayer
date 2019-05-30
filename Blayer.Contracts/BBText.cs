using System.Linq;

namespace BlayerUI.Shared
{
    public class BBText : BElement
    {
        public BBText()
        {
        }

        public BBText(string text)
        {
            InnerText = text;
        }

        public override string Tag => "p";
    }
}
