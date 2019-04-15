using System.Xml;

namespace GuessingGame
{
    internal class Fact
    {
        private XmlElement _xmlElement;

        public Fact(XmlElement xmlElement)
        {
            _xmlElement = xmlElement;
        }

        public string Description
        {
            get { return _xmlElement.GetAttribute("description"); }
        }

        public string GetQuestion()
        {
            return $"Is it {Program.GetArticle(Description)}?";
        }

        public Fact GetChild(Answer answer)
        {
            if (answer == Answer.Yes && _xmlElement["Yes"] != null)
                return new Fact(_xmlElement["Yes"]);

            if (answer == Answer.No && _xmlElement["No"] != null)
                return new Fact(_xmlElement["No"]);

            return null;
        }

        public void InsertChild(string property, string description)
        {
            var newElement = _xmlElement.OwnerDocument.CreateElement("Yes");
            var oldDescription = Description;
            _xmlElement.Attributes["description"].InnerText = property;
            var descriptionAttribute = _xmlElement.OwnerDocument.CreateAttribute("description");
            descriptionAttribute.Value = oldDescription;
            newElement.Attributes.Append(descriptionAttribute);
            _xmlElement.AppendChild(newElement);
        }
    }
}