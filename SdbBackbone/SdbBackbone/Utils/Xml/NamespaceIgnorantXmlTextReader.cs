using System.Xml;

namespace SdbBackbone.Utils.Xml
{
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader)
        {
        }

        public override string NamespaceURI
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
