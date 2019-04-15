using System.Xml;

namespace GuessingGame
{
    internal class KnowledgeBase
    {
        public const string DataFilename = "Data.xml";

        private static KnowledgeBase s_instance;
        private bool _isDataLoaded;
        private XmlDocument _doc = new XmlDocument();

        public static KnowledgeBase Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new KnowledgeBase();

                return s_instance;
            }
        }

        public Fact Root
        {
            get
            {
                if (!_isDataLoaded)
                    LoadData();

                return new Fact(_doc.DocumentElement);
            }
        }

        public void LoadData()
        {
            _doc.Load(DataFilename);
            _isDataLoaded = true;
        }

        public void InitBasicData()
        {
            const string initialData = @"
<Root description=""animal"">
  <Yes description=""cat"" />
  <No description=""house"" />
</Root>";
            _doc.LoadXml(initialData);
            _isDataLoaded = true;
        }

        public void SaveData()
        {
            _doc.Save(DataFilename);
        }
    }
}