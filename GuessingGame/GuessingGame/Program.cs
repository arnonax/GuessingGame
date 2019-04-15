using System;
using System.IO;
using System.Xml;

namespace GuessingGame
{
    class Program
    {
        static void Main()
        {
            InitKnowledge();
            Answer answer;
            do
            {
                var question = KnowledgeBase.Instance.Root;

                Console.WriteLine("Please think about something and answer the following questions about it");
                while (true)
                {
                    Console.WriteLine(question.GetQuestion());
                    answer = GetYesNoAnswer();
                    var nextQuestion = question.GetChild(answer);
                    if (nextQuestion == null)
                    {
                        if (answer == Answer.Yes)
                        {
                            Console.WriteLine("I did it!");
                        }
                        else
                        {
                            AddFact(question);
                        }
                        break;
                    }
                    question = nextQuestion;
                }

                Console.WriteLine("Do you want to play again?");
                answer = GetYesNoAnswer();
            } while (answer == Answer.Yes);
        }

        private static void AddFact(Fact parent)
        {
            Console.WriteLine("What was the thing that you thought about?");
            var description = Console.ReadLine();

            Console.WriteLine($"Please phrase a question to while is true for {GetArticle(description)} but false for {GetArticle(parent.Description)}");
            Console.Write("Is it ");
            var property = Console.ReadLine();

            parent.InsertChild(property, description);

            SaveKnowledge();
        }

        private static void SaveKnowledge()
        {
            Console.WriteLine("Do you want to save the data?");
            var answer = GetYesNoAnswer();
            if (answer == Answer.No)
                return;

            KnowledgeBase.Instance.SaveData();
        }

        private static Answer GetYesNoAnswer()
        {
            while (true)
            {
                var input = Console.ReadLine().Trim().ToLower();
                switch (input)
                {
                    case "y":
                    case "yes":
                        return Answer.Yes;
                    case "n":
                    case "no":
                        return Answer.No;
                    default:
                        Console.WriteLine("Please enter either 'Yes', 'Y', 'No' or 'No' (case is insignificant)");
                        break;
                }
            }
        }

        private static void InitKnowledge()
        {
            Console.WriteLine("Do you want to load data from file?");
            var answer = GetYesNoAnswer();
            if (answer == Answer.Yes)
            {
                if (!File.Exists(KnowledgeBase.DataFilename))
                {
                    Console.WriteLine("Data file does not exist. Starting with initial data.");
                    KnowledgeBase.Instance.InitBasicData();
                }
                else
                {
                    KnowledgeBase.Instance.LoadData();
                }
            }
            else
            {
                KnowledgeBase.Instance.InitBasicData();
            }
        }

        public static string GetArticle(string description)
        {
            return "aeiou".Contains(description.ToLower()[0].ToString())
                ? "an " + description
                : "a " + description;
        }
    }

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

    internal enum Answer
    {
        Yes,
        No
    }

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
