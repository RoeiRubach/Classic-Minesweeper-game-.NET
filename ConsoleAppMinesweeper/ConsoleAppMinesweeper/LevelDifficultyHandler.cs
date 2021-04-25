using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppMinesweeper
{
    class LevelDifficultyHandler
    {
        public string GetUserDifficulty()
        {
            string userInputResult;
            bool passCheck = false;

            do
            {
                Console.Clear();
                Console.SetCursorPosition(15, 5);
                Console.Write("Choose difficulty by writing it and press - Enter.\n\t\t  Beginner, Amateur, Expert or Customized: ");

                userInputResult = Console.ReadLine();

                switch (userInputResult.ToLower())
                {
                    case "beginner":
                    case "b":
                        passCheck = true;
                        userInputResult = StringUtilities.BEGINNER;
                        break;
                    case "amateur":
                    case "a":
                        passCheck = true;
                        userInputResult = StringUtilities.AMATEUR;
                        break;
                    case "expert":
                    case "e":
                        passCheck = true;
                        userInputResult = StringUtilities.EXPERT;
                        break;
                    case "customized":
                    case "c":
                        passCheck = true;
                        userInputResult = StringUtilities.CUSTOMIZED;
                        break;
                    default:
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("Invalid content, please try again.");
                        GameManager.SetThread(2000);
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("\t\t\t\t\t");
                        Console.Clear();
                        break;
                }
            } while (!passCheck);

            return userInputResult;
        }

        public static void PrintUserDifficulty(string userInput)
        {
            Console.Clear();
            Console.SetCursorPosition(31, 3);
            Console.WriteLine(userInput + " it is!");
            Console.SetCursorPosition(31, 4);

            if (userInput == StringUtilities.CUSTOMIZED)
                Console.Write("----------------");
            else
                Console.Write("--------------");
        }
    }
}
