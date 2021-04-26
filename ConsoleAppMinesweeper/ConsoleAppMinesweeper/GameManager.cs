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
        public static string UserDifficultyInput { get; private set; }

        private LevelDifficultyHandler difficultyHandler;
        private BoardManager board;

        public void InitializeGame()
        {
            GetInstructions();
            GetDifficulty();
            PrintUserDifficultyTitle();
            InitializeBoard();
            LoadingScreen.Load();
            InitializeCursor();

            Console.Clear();
            board.Print();
        }

        public void Play()
        {
            UserActionsController userActionsController = new UserActionsController();
            userActionsController.MovingAlongTheArray(board.Game2DArray, board.MinesCount);
        }

        private void InitializeCursor()
        {
            CursorHandler cursorHandler = new CursorHandler();
            cursorHandler.CordsForILeft(board.Game2DArray, UserDifficultyInput);
        }

        private void InitializeBoard()
        {
            board = new BoardManager();
            board.SetBoard();
        }

        public static void PrintUserDifficultyTitle()
        {
            Console.Clear();
            Console.SetCursorPosition(31, 3);
            Console.WriteLine(UserDifficultyInput + " it is!");
            Console.SetCursorPosition(31, 4);

            if (UserDifficultyInput == StringUtilities.CUSTOMIZED)
                Console.Write("----------------");
            else
                Console.Write("--------------");
        }

        private void GetInstructions()
        {
            GameInstructions gameInstructions = new GameInstructions();
            gameInstructions.InitializeInstructions();
        }

        private void GetDifficulty()
        {
            difficultyHandler = new LevelDifficultyHandler();
            UserDifficultyInput = difficultyHandler.GetUserDifficulty();
        }

        /// <summary>
        /// The "restart game" logic. Asks the user if he would like to play again. - Yes or No question.
        /// </summary>
        /// <param name="condition">A returned boolean to set if the user want to restart the game.</param>
        /// <returns></returns>
        public bool IsAnotherGame()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            bool loop = false, condition = false;

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
            for (int i = 6; i <= 8; i += 2)
            {
                Console.SetCursorPosition(30, i);
                Console.Write("------------");
            }
            while (!loop)
            {
                Console.SetCursorPosition(35, 9);
                Console.WriteLine("\t");
                Console.SetCursorPosition(35, 9);

                switch (Console.ReadLine().ToLower())
                {
                    case "yes":
                    case "y":
                        loop = true;
                        Console.Clear();
                        break;

                    case "no":
                    case "n":
                        loop = true;
                        condition = true;
                        Console.SetCursorPosition(23, 10);
                        Console.WriteLine("Thanks for playing. Goodbye.");
                        Thread.Sleep(2000);
                        break;

                    default:
                        Console.SetCursorPosition(35, 9);
                        Console.WriteLine("\t\t\t\t\t\t\t");
                        Console.SetCursorPosition(20, 10);
                        Console.WriteLine("Invalid content, please try again.");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(20, 10);
                        Console.WriteLine("\t\t\t\t\t");
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            return condition;
        }

        public static void SetThread(int amountInMS) => Thread.Sleep(amountInMS);
    }
}
