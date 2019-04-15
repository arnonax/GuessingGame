using System;
using System.IO;

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

            Console.WriteLine($"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}");
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
    }

    internal enum Answer
    {
        Yes,
        No
    }
}
