using System;

namespace GuessingGame
{
    internal class ConsoleUI
    {
        public static ConsoleUI Instance = new ConsoleUI();

        public static Answer GetYesNoAnswer()
        {
            return Instance.GetYesNoAnswer1();
        }

        private Answer GetYesNoAnswer1()
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

        public static void DeclareWinning()
        {
            Instance.DeclareWinning1();
        }

        private void DeclareWinning1()
        { 
            WriteLine("I did it!");
        }

        public static string AskQuestion(string question)
        {
            return Instance.AskQuestion1(question);
        }

        private string AskQuestion1(string question)
        { 
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public static string AskUserToComplete(string instruction, string prefix)
        {
            return Instance.AskUserToComplete1(instruction, prefix);
        }

        private string AskUserToComplete1(string instruction, string prefix)
        { 
            Console.WriteLine(instruction);
            Console.Write(prefix);
            return Console.ReadLine();
        }

        public static void WriteLine(string text)
        {
            Instance.WriteLine1(text);
        }

        public void WriteLine1(string text)
        { 
            Console.WriteLine(text);
        }
    }
}