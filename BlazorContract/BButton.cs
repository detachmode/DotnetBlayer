namespace Blayer.Contracts
{
    public class BButton : BElement
    {

        public BButton(string text)
        {
            InnerText = text;
        }

        public override string Tag => "button";
    }
}
