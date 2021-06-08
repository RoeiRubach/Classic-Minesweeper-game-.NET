using System;
using System.Threading;

namespace ConsoleAppMinesweeper
{
    public class GameManager
    {
        private TextManager _textManager;
        private BoardManager _board;

        public GameManager()
        {
            _textManager = new TextManager();
            _board = new BoardManager();
        }

        public static void SetThreadSleep(int amountInMS) => Thread.Sleep(amountInMS);

        public void InitializeGame()
        {
            InitializeInstructions();
            DifficultyHandler difficulty = new DifficultyHandler();
            TextManager.PrintUserDifficultyTitle();
            InitializeBoard();
            LoadingScreen.Load();
            InitializeCursor();

            Console.Clear();
            _board.Print();
        }

        public void Play()
        {
            UserActionsController userActionsController = new UserActionsController();
            userActionsController.MovingAlongTheArray(_board.Game2DArray, _board.MinesCount);
        }

        private void InitializeInstructions()
        {
            bool isApproved = false;

            while (!isApproved)
            {
                _textManager.PrintGameInstructions();

                switch (Console.ReadLine().ToLower())
                {
                    case "ok":
                    case "k":
                        isApproved = true;
                        break;

                    default:
                        _textManager.ClearUserInputGameInstructions();
                        break;
                }
            }
        }

        private void InitializeBoard()
        {
            _board = new BoardManager();
            _board.SetBoard();
        }

        private void InitializeCursor()
        {
            CursorHandler cursorHandler = new CursorHandler();
            cursorHandler.GetCursorOffSet(_board.Game2DArray, DifficultyHandler.UserDifficulty);
        }

        public bool IsAnotherGame()
        {
            bool isUserAnswer = false;
            bool result = false;
            _textManager.PrintWhetherRestartGame();

            while (!isUserAnswer)
            {
                Console.SetCursorPosition(35, 9);
                Console.WriteLine("\t");
                Console.SetCursorPosition(35, 9);

                switch (Console.ReadLine().ToLower())
                {
                    case "yes":
                    case "y":
                        isUserAnswer = true;
                        Console.Clear();
                        break;

                    case "no":
                    case "n":
                        isUserAnswer = true;
                        result = true;
                        Console.SetCursorPosition(23, 10);
                        Console.WriteLine("Thanks for playing. Goodbye.");
                        Thread.Sleep(2000);
                        break;

                    default:
                         _textManager.ClearUserInputRestartGame();
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            return result;
        }
    }
}
