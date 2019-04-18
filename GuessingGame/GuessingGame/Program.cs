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
                new GameEngine(KnowledgeBase.Instance, ConsoleUI.Instance).Play();

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

    internal enum Answer
    {
        Yes,
        No
    }
}
