using System;

namespace GuessingGame
{
    internal class ConsoleUI
    {
        public static ConsoleUI Instance = new ConsoleUI();

        public Answer GetYesNoAnswer()
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

        public void DeclareWinning()
        {
            Instance.WriteLine("I did it!");
        }

        public string AskQuestion(string question)
        { 
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public string AskUserToComplete(string instruction, string prefix)
        { 
            Console.WriteLine(instruction);
            Console.Write(prefix);
            return Console.ReadLine();
        }

        public void WriteLine(string text)
        { 
            Console.WriteLine(text);
        }
    }
}