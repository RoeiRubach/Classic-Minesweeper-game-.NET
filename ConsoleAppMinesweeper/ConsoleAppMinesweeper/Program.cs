using System;
using System.Threading;

namespace ConsoleAppMinesweeper
{
    class Program
    {
        public const char FRAME_SYMBOL = '^';
        public const char EMPTY_CELL_SYMBOL = '~';
        public const char MINE_SYMBOL = '*';
        public const char MARK_SYMBOL = '!';
        private const string BEGINNER = "Beginner";
        private const string AMATEUR = "Amateur";
        private const string EXPERT = "Expert";
        private const string CUSTOMIZED = "Customized";
        public static int ILeft;
        public static int IHidden;

        static void Main(string[] args)
        {
            string userInput;
            int rows, columns, mines;
            bool isGameOver = false;

            while (!isGameOver)
            {
                Console.ForegroundColor = ConsoleColor.Gray;

                GameInstructions gi = new GameInstructions();
                gi.InitializeInstructions();

                DifficultyLevel(out userInput);
                PrintUserSelection(userInput);

                GetMeasures(out rows, out columns, out mines, userInput);
                Cell[,] game2DArray = new Cell[rows, columns];

                InitializeMineSweeper(game2DArray, mines);
                LoadingScreen.Load();
                Console.ForegroundColor = ConsoleColor.White;
                CordsForILeft(game2DArray, userInput);
                Console.Clear();

                Print(game2DArray);
                MovingAlongTheArray(game2DArray, mines);
                isGameOver = IsAnotherGame();
            }
        }

        private static void PrintUserSelection(string userInput)
        {
            Console.Clear();
            Console.SetCursorPosition(31, 3);
            Console.WriteLine(userInput + " it is!");
            Console.SetCursorPosition(31, 4);

            if(userInput == CUSTOMIZED)
                Console.Write("----------------");
            else
                Console.Write("--------------");
        }

        /// <summary>
        /// Prints all the different levels and asks the user to write the difficulty that he wants to play.
        /// </summary>
        /// <param name="userInput">The variable that gets the user's input.</param>
        static void DifficultyLevel(out string userInput)
        {
            bool passCheck = false;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(15, 5);
                Console.Write("Choose difficulty by writing it and press - Enter.\n\t\t  Beginner, Amateur, Expert or Customized: ");

                userInput = Console.ReadLine();
                switch (userInput.ToLower())
                {
                    case "beginner":
                    case "b":
                        passCheck = true;
                        userInput = BEGINNER;
                        break;
                    case "amateur":
                    case "a":
                        passCheck = true;
                        userInput = AMATEUR;
                        break;
                    case "expert":
                    case "e":
                        passCheck = true;
                        userInput = EXPERT;
                        break;
                    case "customized":
                    case "c":
                        passCheck = true;
                        userInput = CUSTOMIZED;
                        break;
                    default:
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("Invalid content, please try again.");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("\t\t\t\t\t");
                        Console.Clear();
                        break;
                }
            } while (!passCheck);
        }

        /// <summary>
        /// Gets the user's difficulty input and converting it to the game's measures - borders and the number of mines.
        /// </summary>
        /// <param name="rowLength">Keeps the row number that was set.</param>
        /// <param name="colLength">Keeps the column number that was set.</param>
        /// <param name="mineCount">Keeps the mines number that was set.</param>
        /// <param name="userInput">Uses the user's input.</param>
        static void GetMeasures(out int rowLength, out int colLength, out int mineCount, string userInput)
        {
            if (userInput == BEGINNER)
            {
                rowLength = 11;
                colLength = 11;
                mineCount = 10;
            }
            else if (userInput == AMATEUR)
            {
                rowLength = 18;
                colLength = 18;
                mineCount = 40;
            }
            else if (userInput == EXPERT)
            {
                rowLength = 18;
                colLength = 32;
                mineCount = 99;
            }

            else
            {
                rowLength = GetUserRow(userInput);

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

                colLength = GetUserColumn(userInput);

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

        private static int GetMinesNumByUser()
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

        private static int GetValidLengthByUser(int limit, string scope)
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

        private static int GetUserColumn(string userInput)
        {
            int colNum;
            do
            {
                PrintUserSelection(userInput);
                Console.SetCursorPosition(29, 6);
                Console.Write("Enter the board height: ");
            } while (!int.TryParse(Console.ReadLine(), out colNum));
            return colNum;
        }

        private static int GetUserRow(string userInput)
        {
            int rowNum;
            do
            {
                PrintUserSelection(userInput);
                Console.SetCursorPosition(28, 6);
                Console.Write("Enter the board width: ");

            } while (!int.TryParse(Console.ReadLine(), out rowNum));
            return rowNum;
        }

        /// <summary>
        /// Initializes the values of the cells, first sets random mines then sets numbers around the mines
        /// and then sets default values to all empty cells.
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="mineCount">The number of mines to create.</param>
        static void InitializeMineSweeper(Cell[,] game2DArray, int mineCount)
        {
            //Initializes 2D array border as frames.
            for (int topAndBottomRow = 0; topAndBottomRow < game2DArray.GetLength(1); topAndBottomRow++)
            {
                game2DArray[0, topAndBottomRow].CellValue = FRAME_SYMBOL;
                game2DArray[0, topAndBottomRow].IsHidden = false;
                game2DArray[game2DArray.GetLength(0) - 1, topAndBottomRow].CellValue = FRAME_SYMBOL;
                game2DArray[game2DArray.GetLength(0) - 1, topAndBottomRow].IsHidden = false;
            }
            for (int leftAndRightColumn = 0; leftAndRightColumn < game2DArray.GetLength(0); leftAndRightColumn++)
            {
                game2DArray[leftAndRightColumn, 0].CellValue = FRAME_SYMBOL;
                game2DArray[leftAndRightColumn, 0].IsHidden = false;
                game2DArray[leftAndRightColumn, game2DArray.GetLength(1) - 1].CellValue = FRAME_SYMBOL;
                game2DArray[leftAndRightColumn, game2DArray.GetLength(1) - 1].IsHidden = false;
            }

            int mineIndex = 0;
            Random rnd = new Random();

            while (mineIndex < mineCount)
            {
                //Generates new coordinates for the mines on the field.
                int firstRndNum = rnd.Next(1, game2DArray.GetLength(0) - 1);
                int secondRndNum = rnd.Next(1, game2DArray.GetLength(1) - 1);
                if (game2DArray[firstRndNum, secondRndNum].CellValue != MINE_SYMBOL)
                {
                    game2DArray[firstRndNum, secondRndNum].CellValue = MINE_SYMBOL;

                    //Increases the cells value around the current mine.
                    for (int aroundMineI = firstRndNum - 1; aroundMineI <= firstRndNum + 1; aroundMineI++)
                    {
                        for (int aroundMineJ = secondRndNum - 1; aroundMineJ <= secondRndNum + 1; aroundMineJ++)
                        {
                            game2DArray[aroundMineI, aroundMineJ].MinesAround++;
                        }
                    }
                    mineIndex++;
                }
            }

            //Gives a value for each cell on the 2D array - excluding the mines!
            //Also set all cell's isHidden as true.
            for (int i = 1; i < game2DArray.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < game2DArray.GetLength(1) - 1; j++)
                {
                    //Checks if the current cell isn't a mine.
                    if (game2DArray[i, j].CellValue != MINE_SYMBOL)
                    {
                        //?: Operator - condition ? first_expression : second_expression;
                        game2DArray[i, j].CellValue = game2DArray[i, j].MinesAround == 0 ? EMPTY_CELL_SYMBOL : (char)game2DArray[i, j].MinesAround;
                    }
                    game2DArray[i, j].IsHidden = true;
                    game2DArray[i, j].IsMarked = false;
                }
            }
        }

        /// <summary>
        /// Sets the iLeft according the user's difficulty. For later used by the SetCursorPosition(Left).
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="copyOfInput">The copy of the user's difficulty input.</param>
        static void CordsForILeft(Cell[,] game2DArray, string copyOfInput)
        {
            switch (copyOfInput)
            {
                case BEGINNER:
                    ILeft = 29;
                    break;

                case AMATEUR:
                    ILeft = 22;
                    break;

                case EXPERT:
                    ILeft = 8;
                    break;

                default:
                    if (game2DArray.GetLength(0) >= 34 || game2DArray.GetLength(1) >= 34)
                    {
                        ILeft = 0;
                    }
                    else if (game2DArray.GetLength(0) >= 27 || game2DArray.GetLength(1) >= 27)
                    {
                        ILeft = 7;
                    }
                    else if (game2DArray.GetLength(0) >= 20 || game2DArray.GetLength(1) >= 20)
                    {
                        ILeft = 15;
                    }
                    else if (game2DArray.GetLength(0) >= 13 || game2DArray.GetLength(1) >= 13)
                    {
                        ILeft = 21;
                    }
                    else if (game2DArray.GetLength(0) >= 6 || game2DArray.GetLength(1) >= 6)
                    {
                        ILeft = 31;
                    }
                    break;
            }
        }

        /// <summary>
        /// Uses a nested loop to prints all the unhidden values (The frame).
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        static void Print(Cell[,] game2DArray)
        {
            int iTop = 5, temp;
            for (int i = 0; i < game2DArray.GetLength(0); i++)
            {
                temp = ILeft;
                for (int j = 0; j < game2DArray.GetLength(1); j++)
                {
                    if (!game2DArray[i, j].IsHidden)
                    {
                        Console.SetCursorPosition(temp, iTop);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(game2DArray[i, j].CellValue + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    temp += 2;
                }
                iTop++;
                Console.WriteLine();
            }
        }

        /// <summary>
        /// The logic of the movement around the field and pressed playable buttons.
        /// </summary>
        /// <param name="playableArray">The 2D Array the user is playing on.</param>
        /// <localVariable name="upAndDown">The number for the SetCourserPosition. - Top </localVariable>
        /// <localVariable name="sidesCount">Tracking on which cell the user is currently on. - Rows</localVariable>
        /// <localVariable name="upAndDownCount">Tracking on which cell the user is currently on. - Columns</localVariable>
        /// <localVariable name="tempSides">Saved the last number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempUpAndDown">Saved the last number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="tempForSides">Saved the first number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempForUpAndDown">Saved the first number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="redo">True / false if the player - won / lost - the game.</localVariable>
        static void MovingAlongTheArray(Cell[,] playableArray, int minesCounter)
        {
            int upAndDown = 5, sidesCount = 0, upAndDownCount = 0, tempSides = ILeft + 2, tempUpAndDown = upAndDown + 1,
                tempForSides = ILeft + 2, tempForUpAndDown = upAndDown + 1;
            bool redo = false;

            ConsoleKeyInfo keyInfo;
            do
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                Console.SetCursorPosition(ILeft, upAndDown);
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    //Moving the cursor 1 time to the left.
                    case ConsoleKey.LeftArrow:
                        ILeft -= 2;
                        sidesCount--;
                        //Checks if the user is staying on the field.
                        if (ILeft < 0)
                        {
                            ILeft += 2;
                            sidesCount++;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(ILeft, upAndDown);
                        break;

                    //Moving the cursor 1 time to the right.
                    case ConsoleKey.RightArrow:
                        ILeft += 2;
                        sidesCount++;
                        //Checks if the user is staying on the field.
                        if (ILeft > 79)
                        {
                            ILeft -= 2;
                            sidesCount--;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(ILeft, upAndDown);
                        break;

                    //Moving the cursor 1 time up.
                    case ConsoleKey.UpArrow:
                        upAndDown--;
                        upAndDownCount--;
                        //Checks if the user is staying on the field.
                        if (upAndDown < 0)
                        {
                            upAndDown++;
                            upAndDownCount++;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(ILeft, upAndDown);
                        break;

                    //Moving the cursor 1 time down.
                    case ConsoleKey.DownArrow:
                        upAndDown++;
                        upAndDownCount++;
                        //Checks if the user is staying on the field.
                        if (upAndDown > 50)
                        {
                            upAndDown--;
                            upAndDownCount--;
                            Console.SetCursorPosition(0, 47);
                            Console.Write("There's sharks out there.. Trust me, I'm doing you a favor.\nNow go up there and finish my Minesweeper!");
                            Console.SetCursorPosition(ILeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(ILeft, upAndDown);
                        break;

                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                        Console.SetCursorPosition(ILeft, upAndDown);

                        //Checks if the Enter was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {

                            //Checks if the current location is hidden.
                            if (playableArray[upAndDownCount, sidesCount].IsHidden)
                            {

                                //Checks if the current location contains exclamation mark.
                                if (playableArray[upAndDownCount, sidesCount].IsMarked)
                                {
                                    Console.SetCursorPosition(0, 0);
                                    Console.Write("To clear this exclamation mark press - Delete");
                                    Thread.Sleep(2500);
                                    Console.SetCursorPosition(ILeft, upAndDown);
                                }

                                //Checks if the current location contains a mine.
                                else if (playableArray[upAndDownCount, sidesCount].CellValue == MINE_SYMBOL)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    playableArray[upAndDownCount, sidesCount].IsHidden = false;
                                    Console.Write(playableArray[upAndDownCount, sidesCount].CellValue);
                                    playableArray[upAndDownCount, sidesCount].CellValue = '0';
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    upAndDown = tempForUpAndDown;

                                    //Prints all the mines on the field. Then waits 2.5 seconds, clears everything and print "Game over".
                                    for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                    {
                                        ILeft = tempForSides;
                                        for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                        {
                                            if (playableArray[i, j].CellValue == MINE_SYMBOL)
                                            {
                                                Console.SetCursorPosition(ILeft, upAndDown);
                                                (playableArray[i, j].IsHidden) = false;
                                                Console.Write(playableArray[i, j].CellValue);
                                            }
                                            ILeft += 2;
                                        }
                                        upAndDown++;
                                    }
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Thread.Sleep(2500);
                                    Console.Clear();
                                    Console.SetCursorPosition(31, 1);
                                    Console.WriteLine("Game over!");
                                    Console.SetCursorPosition(30, 2);
                                    Console.WriteLine("Tough luck..");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    redo = true;
                                }
                                else
                                {
                                    UnlockSlotsIfEmpty(playableArray, upAndDown, tempSides = ILeft, tempUpAndDown = upAndDown, sidesCount, upAndDownCount);
                                    IHidden = 0;

                                    //Counts the number of hidden values.
                                    for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                    {
                                        for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                        {
                                            if (playableArray[i, j].IsHidden)
                                            {
                                                IHidden++;
                                            }
                                        }
                                    }
                                    //Checks if the amount of hidden values is equal to the number of mines.
                                    //If it does, uses a nested loop to print all the mines locations as exclamation marks.
                                    if (IHidden == minesCounter)
                                    {
                                        upAndDown = tempForUpAndDown;
                                        for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                        {
                                            ILeft = tempForSides;
                                            for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                            {
                                                if (playableArray[i, j].CellValue == MINE_SYMBOL)
                                                {
                                                    Console.SetCursorPosition(ILeft, upAndDown);
                                                    (playableArray[i, j].IsMarked) = true;
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.Write(playableArray[i, j].CellValue = MARK_SYMBOL);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                ILeft += 2;
                                            }
                                            upAndDown++;
                                        }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Thread.Sleep(2500);
                                        Console.Clear();
                                        Console.SetCursorPosition(28, 1);
                                        Console.WriteLine("Congratulations!");
                                        Console.SetCursorPosition(31, 2);
                                        Console.WriteLine("You've won!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        redo = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("What are you trying to click on?\nYou totally missed the board..");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                        }
                        break;

                    case ConsoleKey.Insert:

                        //Checks if the Insert was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {
                            //Checks if the current location isn't hidden or marked by exclamation mark.
                            if (!playableArray[upAndDownCount, sidesCount].IsHidden || playableArray[upAndDownCount, sidesCount].IsMarked)
                            {
                                break;
                            }
                            playableArray[upAndDownCount, sidesCount].IsMarked = true;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(playableArray[upAndDownCount, sidesCount].MarkValue = MARK_SYMBOL);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to redecorate my Minesweeper I see..\nMaybe it's better to focus on finishing it.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                        }
                        break;

                    case ConsoleKey.Delete:

                        //Checks if the Delete was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {
                            //Checks if the current location is marked by exclamation mark.
                            if (playableArray[upAndDownCount, sidesCount].IsMarked)
                            {
                                playableArray[upAndDownCount, sidesCount].IsMarked = false;
                                Console.Write(playableArray[upAndDownCount, sidesCount].MarkValue = '\0');
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("There is nothing to delete out there..\nMaybe it's better to focus on the Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(ILeft, upAndDown);
                        }
                        break;
                }
            } while (!redo);
        }

        /// <summary>
        /// Reveals the cell/s if the specific location is in the field and it isn't a frame, a mine, hidden or exclamation mark.
        /// First, checks if current location is empty then passes around it to seek for another empty cell.
        /// If found - Starts all over again. If not - Prints the number.
        /// </summary>
        /// <param name="playableArray">The 2D Array the user is playing on.</param>
        /// <param name="upAndDown">The number for the SetCourserPosition. - Top</param>
        /// <param name="tempSides">Saved the last number 'sides' used for the SetCourserPosition. - Left</param>
        /// <param name="tempUpAndDown">Saved the last number 'upAndDown' used for the SetCourserPosition. - Top</param>
        /// <param name="sidesCount">Tracking on which cell the user is currently on. - Rows</param>
        /// <param name="upAndDownCount">Tracking on which cell the user is currently on. - Columns</param>
        static void UnlockSlotsIfEmpty(Cell[,] playableArray, int upAndDown, int tempSides, int tempUpAndDown, int sidesCount, int upAndDownCount)
        {
            int tempForLeft = ILeft;
            Console.SetCursorPosition(ILeft, upAndDown);

            //Checks if the current location is equal to 'empty'.
            if (playableArray[upAndDownCount, sidesCount].MinesAround == 0)
            {
                playableArray[upAndDownCount, sidesCount].IsHidden = false;
                Console.Write(playableArray[upAndDownCount, sidesCount].CellValue);
                upAndDown = tempUpAndDown - 1;

                //Passes around the current location.
                for (int aroundCharI = upAndDownCount - 1; aroundCharI <= upAndDownCount + 1; aroundCharI++)
                {
                    ILeft = tempSides - 2;

                    for (int aroundCharJ = sidesCount - 1; aroundCharJ <= sidesCount + 1; aroundCharJ++)
                    {
                        //Checks if the current location isn't a frame, a mine and marked by exclamation mark.
                        if (playableArray[aroundCharI, aroundCharJ].CellValue != FRAME_SYMBOL && playableArray[aroundCharI, aroundCharJ].CellValue != MINE_SYMBOL && !playableArray[aroundCharI, aroundCharJ].IsMarked)
                        {
                            //Checks if the current location is hidden and is it an empty cell.
                            //If it does, starts all over again from the current location.
                            if (playableArray[aroundCharI, aroundCharJ].IsHidden && playableArray[aroundCharI, aroundCharJ].CellValue == EMPTY_CELL_SYMBOL)
                            {
                                UnlockSlotsIfEmpty(playableArray, upAndDown, tempSides = ILeft, tempUpAndDown = upAndDown, sidesCount = aroundCharJ, upAndDownCount = aroundCharI);
                                break;
                            }

                            Console.SetCursorPosition(ILeft, upAndDown);
                            //Checks if the current location isn't equal to 'empty' and it's hidden.
                            if (playableArray[aroundCharI, aroundCharJ].MinesAround != 0 && playableArray[aroundCharI, aroundCharJ].IsHidden)
                            {
                                NumColorsGuarantor.DyeNumber(playableArray, aroundCharI, aroundCharJ);
                                playableArray[aroundCharI, aroundCharJ].IsHidden = false;
                                Console.Write(playableArray[aroundCharI, aroundCharJ].MinesAround);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            //Checks if the current location isn't hidden.
                            if (playableArray[aroundCharI, aroundCharJ].IsHidden != false)
                            {
                                playableArray[aroundCharI, aroundCharJ].IsHidden = false;
                                Console.Write(playableArray[aroundCharI, aroundCharJ].CellValue);
                            }
                        }
                        ILeft += 2;
                    }
                    upAndDown++;
                }
            }
            //Prints a number.
            else
            {
                NumColorsGuarantor.DyeNumber(playableArray, upAndDownCount, sidesCount);
                playableArray[upAndDownCount, sidesCount].IsHidden = false;
                Console.Write(playableArray[upAndDownCount, sidesCount].MinesAround);
                Console.ForegroundColor = ConsoleColor.White;
            }
            ILeft = tempForLeft;
        }

        /// <summary>
        /// The "restart game" logic. Asks the user if he would like to play again. - Yes or No question.
        /// </summary>
        /// <param name="condition">A returned boolean to set if the user want to restart the game.</param>
        /// <returns></returns>
        static bool IsAnotherGame()
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
    }
}