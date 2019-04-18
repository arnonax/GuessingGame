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
            var engine = new GameEngine(KnowledgeBase.Instance, ConsoleUI.Instance);
            engine.OnWin += () => ConsoleUI.Instance.DeclareWinning();
            do
            {
                engine.Play();

                Console.WriteLine("Do you want to play again?");
                answer = ConsoleUI.Instance.GetYesNoAnswer();
            } while (answer == Answer.Yes);
        }

        private static void InitKnowledge()
        {
            if (!File.Exists(KnowledgeBase.DataFilename))
            {
                Console.WriteLine("Data file does not exist. Starting with initial data.");
                KnowledgeBase.Instance.InitBasicData();
            }
        }
    }

    public enum Answer
    {
        Yes,
        No
    }
}
