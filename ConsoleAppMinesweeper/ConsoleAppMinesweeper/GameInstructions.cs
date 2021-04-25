using System;

namespace ConsoleAppMinesweeper
{
    public class GameInstructions
    {
        public void InitializeInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            bool isApproved = false;

            while (!isApproved)
            {
                PrintInstructions();

                switch (Console.ReadLine().ToLower())
                {
                    case "ok":
                    case "k":
                        isApproved = true;
                        break;

                    default:
                        EraseUserInput();
                        break;
                }
            }
        }

        private void PrintInstructions()
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

        private void EraseUserInput()
        {
            Console.SetCursorPosition(49, 12);
            Console.WriteLine("\t\t\t\t\t\t");
        }
    }
}
