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
    public class BBHeader : BElement
    {
        public BBHeader()
        {
        }

        public BBHeader(string text)
        {
            InnerText = text;
        }

        public override string Tag => "h1";
    }
    public class BDiv : BElement
    {
        public new string Tag => "div";
    }
}
