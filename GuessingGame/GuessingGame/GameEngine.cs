using System;

namespace GuessingGame
{
    public class GameEngine
    {
        private readonly IKnowledgeBase _knowledgeBase;
        private readonly IUserInterface _consoleUI;

        public GameEngine(IKnowledgeBase knowledgeBase, IUserInterface consoleUI)
        {
            // TEMPORARY: this casting should be removed when the interface will have all neccesary members
            _knowledgeBase = knowledgeBase;
            _consoleUI = consoleUI;
        }

        public void Play()
        { 
            var question = _knowledgeBase.Root;

            _consoleUI.WriteLine("Please think about something and answer the following questions about it");
            while (true)
            {
                _consoleUI.WriteLine(question.GetQuestion());
                var answer = _consoleUI.GetYesNoAnswer();
                var nextQuestion = question.GetChild(answer);
                if (nextQuestion == null)
                {
                    if (answer == Answer.Yes)
                    {
                        OnWin?.Invoke();
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

        private void AddFact(IFact parent)
        {
            var description = _consoleUI.AskQuestion("What was the thing that you thought about?");

            var instruction = $"Please phrase a question to while is true for {TextUtils.GetArticle(description)} but false for {TextUtils.GetArticle(parent.Description)}";
            var property = _consoleUI.AskUserToComplete(instruction, "Is it ");

            parent.InsertChild(property.TrimEnd('?'), description);
        }

        public event Action OnWin;
    }
}