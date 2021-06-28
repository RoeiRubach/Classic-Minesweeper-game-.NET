using System;

namespace ConsoleAppMinesweeper
{
    public class TextManager
    {
        public void PrintWhetherRestartGame()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(20, 3);
            Console.Write("Would you like to restart the game?");
            Console.SetCursorPosition(19, 4);
            Console.Write("Write your answer and press - Enter.");
            Console.SetCursorPosition(29, 7);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("!Yes");
            Console.SetCursorPosition(36, 7);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("/");
            Console.SetCursorPosition(40, 7);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("No!");
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintEdgeYesNo();
        }

        private void PrintEdgeYesNo()
        {
            for (int i = 6; i <= 8; i += 2)
            {
                Console.SetCursorPosition(30, i);
                Console.Write("------------");
            }
        }

        public void PrintGameInstructions()
        {
            Console.SetCursorPosition(25, 1);
            Console.Write("Welcome to my Minesweeper!");
            Console.SetCursorPosition(25, 2);
            Console.Write("-------------------------");
            Console.SetCursorPosition(29, 4);
            Console.Write("Game Instructions:");
            Console.SetCursorPosition(29, 5);
            Console.Write("-----------------");
            Console.SetCursorPosition(14, 7);
            Console.Write("Use the -  Enter button  - to choose a cell/location.");
            Console.SetCursorPosition(14, 8);
            Console.Write("Use the - Insert button  - to place a flag.");
            Console.SetCursorPosition(14, 9);
            Console.Write("Use the - Delete button  - to remove the flag.");
            Console.SetCursorPosition(14, 10);
            Console.Write("Use the - arrows buttons - to move around the field.");
            Console.SetCursorPosition(9, 11);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Above the cursor's position is where will you be clicking on.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(27, 12);
            Console.Write("Write OK to continue - ");
        }

        public void ClearUserInputGameInstructions()
        {
            Console.SetCursorPosition(49, 12);
            Console.WriteLine("\t\t\t\t\t\t");
        }

        public void ClearUserInputRestartGame()
        {
            Console.SetCursorPosition(35, 9);
            Console.WriteLine("\t\t\t\t\t\t\t");
            Console.SetCursorPosition(20, 10);
            Console.WriteLine("Invalid content, please try again.");
            GameManager.SetThreadSleep(2000);
            Console.SetCursorPosition(20, 10);
            Console.WriteLine("\t\t\t\t\t");
        }

        public static void PrintDifficultyInstructions()
        {
            Console.Clear();
            Console.SetCursorPosition(15, 5);
            Console.Write("Choose difficulty by writing it and press - Enter.\n\t\t  Beginner, Amateur, Expert or Customized: ");
        }

        public static void ClearUserInputDifficulty()
        {
            Console.SetCursorPosition(21, 3);
            Console.WriteLine("Invalid content, please try again.");
            GameManager.SetThreadSleep(2000);
            Console.SetCursorPosition(21, 3);
            Console.WriteLine("\t\t\t\t\t");
            Console.Clear();
        }

        public static void PrintUserDifficultyTitle()
        {
            Console.Clear();
            Console.SetCursorPosition(31, 3);
            Console.WriteLine(DifficultyHandler.UserDifficulty + " it is!");
            Console.SetCursorPosition(31, 4);

            if (DifficultyHandler.UserDifficulty == StringUtilities.CUSTOMIZED)
                Console.Write("----------------");
            else
                Console.Write("--------------");
        }
    }
}
