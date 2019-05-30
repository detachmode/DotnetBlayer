namespace BlayerUI.Shared
{
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
}
