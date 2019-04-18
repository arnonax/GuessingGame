using System;

namespace GuessingGame
{
    internal static class GameEngine
    {
        public static void Play()
        {
            var question = KnowledgeBase.Instance.Root;

            ConsoleUI.WriteLine("Please think about something and answer the following questions about it");
            while (true)
            {
                Console.WriteLine(question.GetQuestion());
                var answer = ConsoleUI.GetYesNoAnswer();
                var nextQuestion = question.GetChild(answer);
                if (nextQuestion == null)
                {
                    if (answer == Answer.Yes)
                    {
                        ConsoleUI.DeclareWinning();
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
            var description = ConsoleUI.AskQuestion("What was the thing that you thought about?");

            var instruction = $"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}";
            var property = ConsoleUI.AskUserToComplete(instruction, "Is it ");

            parent.InsertChild(property.TrimEnd('?'), description);
        }
    }
}