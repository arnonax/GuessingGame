using System;

namespace GuessingGame
{
    internal class GameEngine
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly ConsoleUI _consoleUI;

        public GameEngine()
        {
            _knowledgeBase = KnowledgeBase.Instance;
            _consoleUI = ConsoleUI.Instance;
        }

        public void Play()
        { 
            var question = _knowledgeBase.Root;

            _consoleUI.WriteLine("Please think about something and answer the following questions about it");
            while (true)
            {
                Console.WriteLine(question.GetQuestion());
                var answer = _consoleUI.GetYesNoAnswer();
                var nextQuestion = question.GetChild(answer);
                if (nextQuestion == null)
                {
                    if (answer == Answer.Yes)
                    {
                        _consoleUI.DeclareWinning();
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

        private void AddFact(Fact parent)
        {
            var description = _consoleUI.AskQuestion("What was the thing that you thought about?");

            var instruction = $"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}";
            var property = _consoleUI.AskUserToComplete(instruction, "Is it ");

            parent.InsertChild(property.TrimEnd('?'), description);
        }
    }
}