using System;

namespace GuessingGame
{
    internal static class GameEngine
    {
        public static void Play()
        {
            var question = KnowledgeBase.Instance.Root;

            ConsoleUI.Instance.WriteLine("Please think about something and answer the following questions about it");
            while (true)
            {
                Console.WriteLine(question.GetQuestion());
                var answer = ConsoleUI.Instance.GetYesNoAnswer();
                var nextQuestion = question.GetChild(answer);
                if (nextQuestion == null)
                {
                    if (answer == Answer.Yes)
                    {
                        ConsoleUI.Instance.DeclareWinning();
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
            var description = ConsoleUI.Instance.AskQuestion("What was the thing that you thought about?");

            var instruction = $"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}";
            var property = ConsoleUI.Instance.AskUserToComplete(instruction, "Is it ");

            parent.InsertChild(property.TrimEnd('?'), description);
        }
    }
}