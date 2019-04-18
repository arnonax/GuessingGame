using System;

namespace GuessingGame
{
    internal static class ConsoleUI
    {
        public static Answer GetYesNoAnswer()
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
            WriteLine("I did it!");
        }

        public static string AskQuestion(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public static string AskUserToComplete(string instruction, string prefix)
        {
            Console.WriteLine(instruction);
            Console.Write(prefix);
            return Console.ReadLine();
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}