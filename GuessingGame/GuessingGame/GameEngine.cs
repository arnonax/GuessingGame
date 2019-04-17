using System;

namespace GuessingGame
{
    internal static class GameEngine
    {
        public static void Play()
        {
            var question = KnowledgeBase.Instance.Root;

            Console.WriteLine("Please think about something and answer the following questions about it");
            while (true)
            {
                Console.WriteLine(question.GetQuestion());
                var answer = ConsoleUI.GetYesNoAnswer();
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
        }

        private static void AddFact(Fact parent)
        {
            Console.WriteLine("What was the thing that you thought about?");
            var description = Console.ReadLine();

            Console.WriteLine($"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}");
            Console.Write("Is it ");
            var property = Console.ReadLine();

            parent.InsertChild(property.TrimEnd('?'), description);
        }
    }
}