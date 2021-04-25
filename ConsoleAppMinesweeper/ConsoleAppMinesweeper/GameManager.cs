using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppMinesweeper
{
    public class GameManager
    {
        public string UserInput { get; private set; }
        
        private int rows, columns, mines;

        public void InitializeGame()
        {
            GetInstructions();
            GetDifficulty();
            LevelDifficultyHandler.PrintUserDifficulty(UserInput);
            GetMeasures(out rows, out columns, out mines, UserInput);
        }

        private void GetDifficulty()
        {
            LevelDifficultyHandler difficultyHandler = new LevelDifficultyHandler();
            UserInput = difficultyHandler.GetUserDifficulty();
        }

        private void GetInstructions()
        {
            GameInstructions gameInstructions = new GameInstructions();
            gameInstructions.InitializeInstructions();
        }

        public static void SetThread(int amountInMS)
        {
            Thread.Sleep(amountInMS);
        }

        private void GetMeasures(out int rowLength, out int colLength, out int mineCount, string userInput)
        {
            if (userInput == StringUtilities.BEGINNER)
            {
                rowLength = 11;
                colLength = 11;
                mineCount = 10;
            }
            else if (userInput == StringUtilities.AMATEUR)
            {
                rowLength = 18;
                colLength = 18;
                mineCount = 40;
            }
            else if (userInput == StringUtilities.EXPERT)
            {
                rowLength = 18;
                colLength = 32;
                mineCount = 99;
            }

            else
            {
                rowLength = GetUserRow();

                const int MAX_WIDTH = 40;
                const int MIN_WIDTH = 6;
                string lower = "lower";
                string higher = "higher";

                while (rowLength > MAX_WIDTH || rowLength < MIN_WIDTH)
                {
                    if (rowLength > MAX_WIDTH)
                        rowLength = GetValidLengthByUser(MAX_WIDTH, lower);

                    else if (rowLength < MIN_WIDTH)
                        rowLength = GetValidLengthByUser(MIN_WIDTH, higher);
                }

                colLength = GetUserColumn();

                while (colLength > MAX_WIDTH || colLength < MIN_WIDTH)
                {
                    if (colLength > MAX_WIDTH)
                        colLength = GetValidLengthByUser(MAX_WIDTH, lower);

                    else if (colLength < MIN_WIDTH)
                        colLength = GetValidLengthByUser(MIN_WIDTH, higher);
                }

                mineCount = GetMinesNumByUser();

                while (mineCount >= (rowLength * colLength) / MIN_WIDTH)
                {
                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(9, 4);
                        Console.Write("The number of mines you've entered is greater than the field.");
                        Console.SetCursorPosition(9, 6);
                        Console.Write("Please enter a smaller mine number: ");
                    } while (!int.TryParse(Console.ReadLine(), out mineCount));
                }
            }
        }

        private int GetMinesNumByUser()
        {
            int result;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(11, 5);
                Console.Write("Enter the number of mines you want in your minesweeper: ");
            } while (!int.TryParse(Console.ReadLine(), out result));
            return result;
        }

        private int GetValidLengthByUser(int limit, string scope)
        {
            int result;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(12, 4);
                Console.Write("The number you've entered isn't valid.");
                Console.SetCursorPosition(12, 6);
                Console.Write($"Please enter a different number, which is {limit} or {scope}: ");
            } while (!int.TryParse(Console.ReadLine(), out result));

            return result;
        }

        private int GetUserColumn()
        {
            int colNum;
            do
            {
                LevelDifficultyHandler.PrintUserDifficulty(UserInput);
                Console.SetCursorPosition(29, 6);
                Console.Write("Enter the board height: ");
            } while (!int.TryParse(Console.ReadLine(), out colNum));
            return colNum;
        }

        private int GetUserRow()
        {
            int rowNum;
            do
            {
                LevelDifficultyHandler.PrintUserDifficulty(UserInput);
                Console.SetCursorPosition(28, 6);
                Console.Write("Enter the board width: ");

            } while (!int.TryParse(Console.ReadLine(), out rowNum));
            return rowNum;
        }
    }
}
