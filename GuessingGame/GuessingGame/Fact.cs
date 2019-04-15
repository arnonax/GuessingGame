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
            return $"Is it {TextUtils.GetArticle(Description)}?";
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
            var yesElement = _xmlElement.OwnerDocument.CreateElement("Yes");
            var noElement = _xmlElement.OwnerDocument.CreateElement("No");
            var oldDescription = Description;
            _xmlElement.Attributes["description"].InnerText = property;
            AddDescriptionAttribute(noElement, oldDescription);
            AddDescriptionAttribute(yesElement, description);
            _xmlElement.AppendChild(yesElement);
            _xmlElement.AppendChild(noElement);
        }

        private void AddDescriptionAttribute(XmlElement element, string description)
        {
            var descriptionAttribute = _xmlElement.OwnerDocument.CreateAttribute("description");
            descriptionAttribute.Value = description;
            element.Attributes.Append(descriptionAttribute);
        }
    }
}