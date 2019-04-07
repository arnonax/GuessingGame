using System;
using System.IO;
using System.Xml;

namespace GuessingGame
{
    class Program
    {
        private static Fact s_rootQuestion = 
            new Fact("animal", 
                new Fact("cat"), new Fact("house"));

        private const string DataFilename = "Data.xml";

        static void Main()
        {
            s_rootQuestion = GetExistingKnowledge();
            Answer answer;
            do
            {
                var question = s_rootQuestion;

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

            var doc = new XmlDocument();
            SaveFact(s_rootQuestion, doc, doc, "Root");
            doc.Save(DataFilename);
        }

        private static void SaveFact(Fact fact, XmlDocument doc, XmlNode parent, string elementName)
        {
            if (fact == null)
                return;

            var element = doc.CreateElement(elementName);
            var descriptionAttribute = doc.CreateAttribute("description");
            descriptionAttribute.InnerText = fact.Description;
            element.Attributes.Append(descriptionAttribute);

            SaveFact(fact.Yes, doc, element, "Yes");
            SaveFact(fact.No, doc, element, "No");

            parent.AppendChild(element);
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

        private static Fact GetExistingKnowledge()
        {
            Console.WriteLine("Do you want to load data from file?");
            var answer = GetYesNoAnswer();
            if (answer == Answer.Yes)
            {
                if (!File.Exists(DataFilename))
                    Console.WriteLine("Data file does not exist. Starting with initial data.");
                else
                {
                    LoadData();
                }
            }
            return s_rootQuestion;
        }

        private static void LoadData()
        {
            var doc = new XmlDocument();
            doc.Load(DataFilename);
            var rootElement = doc.DocumentElement;
            s_rootQuestion = ReadFact(rootElement);
        }

        private static Fact ReadFact(XmlElement rootElement)
        {
            if (rootElement == null)
                return null;

            var yesFact = ReadFact(rootElement["Yes"]);
            var noFact = ReadFact(rootElement["No"]);
            var description = rootElement.Attributes["description"].InnerText;
            return new Fact(description, yesFact, noFact);
        }

        public static string GetArticle(string description)
        {
            return "aeiou".Contains(description.ToLower()[0].ToString())
                ? "an " + description
                : "a " + description;
        }
    }

    internal enum Answer
    {
        Yes,
        No
    }

    internal class Fact
    {
        public Fact(string description, Fact yes, Fact no)
        {
            Description = description;
            Yes = yes;
            No = no;
        }

        public Fact(string description)
            : this(description, null, null)
        {
            
        }

        public string Description { get; private set; }

        public Fact Yes { get; private set; }

        public Fact No { get; private set; }

        public string GetQuestion()
        {
            return $"Is it {Program.GetArticle(Description)}?";
        }

        public Fact GetChild(Answer answer)
        {
            return answer == Answer.Yes ? Yes : No;
        }

        public void InsertChild(string property, string description)
        {
            var newFact = new Fact(description);
            var oldDescription = Description;
            Description = property;
            Yes = newFact;
            No = new Fact(oldDescription);
        }
    }
}
