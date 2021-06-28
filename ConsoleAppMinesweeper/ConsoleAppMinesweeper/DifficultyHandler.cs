using System;

namespace ConsoleAppMinesweeper
{
    public class DifficultyHandler
    {
        public static string UserDifficulty { get; private set; }

        public void SetUserDifficulty()
        {
            string userInputResult;
            bool passCheck = false;

            do
            {
                TextManager.PrintDifficultyInstructions();

                userInputResult = Console.ReadLine();

                switch (userInputResult.ToLower())
                {
                    case "beginner":
                    case "b":
                        passCheck = true;
                        UserDifficulty = StringUtilities.BEGINNER;
                        break;

                    case "amateur":
                    case "a":
                        passCheck = true;
                        UserDifficulty = StringUtilities.AMATEUR;
                        break;

                    case "expert":
                    case "e":
                        passCheck = true;
                        UserDifficulty = StringUtilities.EXPERT;
                        break;

                    case "customized":
                    case "c":
                        passCheck = true;
                        UserDifficulty = StringUtilities.CUSTOMIZED;
                        break;

                    default:
                        TextManager.ClearUserInputDifficulty();
                        break;
                }
            } while (!passCheck);
        }
    }
}
