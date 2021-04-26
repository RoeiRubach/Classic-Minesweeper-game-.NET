using System;

namespace ConsoleAppMinesweeper
{
    public class LevelDifficultyHandler
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
    }
}
