using System.Linq;

namespace Blayer.Contracts
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
