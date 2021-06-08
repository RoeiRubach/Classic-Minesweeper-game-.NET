using System;

namespace ConsoleAppMinesweeper
{
    public class BoardManager
    {
        public Cell[,] Game2DArray { get; private set; }
        public int MinesCount { get; private set; }

        private UserActionsController _userActionsController;
        private int _rowLength;
        private int _colLength;

        public BoardManager() => _userActionsController = new UserActionsController();

        public void SetBoard()
        {
            GetMeasures();
            Game2DArray = new Cell[_rowLength, _colLength];
            SetFrame();
            SetMines();
        }

        public void Print()
        {
            int iTop = 5, temp;

            for (int i = 0; i < Game2DArray.GetLength(0); i++)
            {
                temp = CursorHandler.CursorOffSet;

                for (int j = 0; j < Game2DArray.GetLength(1); j++)
                {
                    if (!Game2DArray[i, j].IsHidden)
                    {
                        Console.SetCursorPosition(temp, iTop);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(Game2DArray[i, j].CellValue + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    temp += 2;
                }
                iTop++;
                Console.WriteLine();
            }
        }

        private void GetMeasures()
        {
            if (DifficultyHandler.UserDifficulty == StringUtilities.BEGINNER)
            {
                _rowLength = 11;
                _colLength = 11;
                MinesCount = 10;
            }
            else if (DifficultyHandler.UserDifficulty == StringUtilities.AMATEUR)
            {
                _rowLength = 18;
                _colLength = 18;
                MinesCount = 40;
            }
            else if (DifficultyHandler.UserDifficulty == StringUtilities.EXPERT)
            {
                _rowLength = 18;
                _colLength = 32;
                MinesCount = 99;
            }

            else
            {
                _rowLength = _userActionsController.GetRow();

                const int MAX_WIDTH = 40;
                const int MIN_WIDTH = 6;
                string lower = "lower";
                string higher = "higher";

                while (_rowLength > MAX_WIDTH || _rowLength < MIN_WIDTH)
                {
                    if (_rowLength > MAX_WIDTH)
                        _rowLength = _userActionsController.GetValidLength(MAX_WIDTH, lower);

                    else if (_rowLength < MIN_WIDTH)
                        _rowLength = _userActionsController.GetValidLength(MIN_WIDTH, higher);
                }

                _colLength = _userActionsController.GetColumn();

                while (_colLength > MAX_WIDTH || _colLength < MIN_WIDTH)
                {
                    if (_colLength > MAX_WIDTH)
                        _colLength = _userActionsController.GetValidLength(MAX_WIDTH, lower);

                    else if (_colLength < MIN_WIDTH)
                        _colLength = _userActionsController.GetValidLength(MIN_WIDTH, higher);
                }

                MinesCount = _userActionsController.GetMinesNum();

                while (MinesCount >= (_rowLength * _colLength) / MIN_WIDTH)
                {
                    int tempInput;

                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(9, 4);
                        Console.Write("The number of mines you've entered is greater than the field.");
                        Console.SetCursorPosition(9, 6);
                        Console.Write("Please enter a smaller mine number: ");
                    } while (!int.TryParse(Console.ReadLine(), out tempInput));

                    MinesCount = tempInput;
                }
            }
        }

        private void SetFrame()
        {
            for (int topAndBottomRow = 0; topAndBottomRow < Game2DArray.GetLength(1); topAndBottomRow++)
            {
                Game2DArray[0, topAndBottomRow].CellValue = StringUtilities.FRAME_SYMBOL;
                Game2DArray[0, topAndBottomRow].IsHidden = false;
                Game2DArray[Game2DArray.GetLength(0) - 1, topAndBottomRow].CellValue = StringUtilities.FRAME_SYMBOL;
                Game2DArray[Game2DArray.GetLength(0) - 1, topAndBottomRow].IsHidden = false;
            }
            for (int leftAndRightColumn = 0; leftAndRightColumn < Game2DArray.GetLength(0); leftAndRightColumn++)
            {
                Game2DArray[leftAndRightColumn, 0].CellValue = StringUtilities.FRAME_SYMBOL;
                Game2DArray[leftAndRightColumn, 0].IsHidden = false;
                Game2DArray[leftAndRightColumn, Game2DArray.GetLength(1) - 1].CellValue = StringUtilities.FRAME_SYMBOL;
                Game2DArray[leftAndRightColumn, Game2DArray.GetLength(1) - 1].IsHidden = false;
            }
        }

        private void SetMines()
        {
            int mineIndex = 0;
            Random rnd = new Random();

            while (mineIndex < MinesCount)
            {
                int firstRndNum = rnd.Next(1, Game2DArray.GetLength(0) - 1);
                int secondRndNum = rnd.Next(1, Game2DArray.GetLength(1) - 1);

                if (Game2DArray[firstRndNum, secondRndNum].CellValue != StringUtilities.MINE_SYMBOL)
                {
                    Game2DArray[firstRndNum, secondRndNum].CellValue = StringUtilities.MINE_SYMBOL;

                    //Increases the cells value around the current mine.
                    for (int aroundMineI = firstRndNum - 1; aroundMineI <= firstRndNum + 1; aroundMineI++)
                    {
                        for (int aroundMineJ = secondRndNum - 1; aroundMineJ <= secondRndNum + 1; aroundMineJ++)
                        {
                            Game2DArray[aroundMineI, aroundMineJ].MinesAround++;
                        }
                    }
                    mineIndex++;
                }
            }

            //Gives a value for each cell on the 2D array - excluding the mines!
            //Also set all cell's isHidden as true.
            for (int i = 1; i < Game2DArray.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < Game2DArray.GetLength(1) - 1; j++)
                {
                    //Checks if the current cell isn't a mine.
                    if (Game2DArray[i, j].CellValue != StringUtilities.MINE_SYMBOL)
                    {
                        //?: Operator - condition ? first_expression : second_expression;
                        Game2DArray[i, j].CellValue = Game2DArray[i, j].MinesAround == 0 ? StringUtilities.EMPTY_CELL_SYMBOL : (char)Game2DArray[i, j].MinesAround;
                    }
                    Game2DArray[i, j].IsHidden = true;
                    Game2DArray[i, j].IsMarked = false;
                }
            }
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
        public static void UnlockSlotsIfEmpty(Cell[,] playableArray, int upAndDown, int tempSides, int tempUpAndDown, int sidesCount, int upAndDownCount)
        {
            int tempForLeft = CursorHandler.CursorOffSet;
            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);

            //Checks if the current location is equal to 'empty'.
            if (playableArray[upAndDownCount, sidesCount].MinesAround == 0)
            {
                playableArray[upAndDownCount, sidesCount].IsHidden = false;
                Console.Write(playableArray[upAndDownCount, sidesCount].CellValue);
                upAndDown = tempUpAndDown - 1;

                //Passes around the current location.
                for (int aroundCharI = upAndDownCount - 1; aroundCharI <= upAndDownCount + 1; aroundCharI++)
                {
                    CursorHandler.CursorOffSet = tempSides - 2;

                    for (int aroundCharJ = sidesCount - 1; aroundCharJ <= sidesCount + 1; aroundCharJ++)
                    {
                        //Checks if the current location isn't a frame, a mine and marked by exclamation mark.
                        if (playableArray[aroundCharI, aroundCharJ].CellValue != StringUtilities.FRAME_SYMBOL && playableArray[aroundCharI, aroundCharJ].CellValue != StringUtilities.MINE_SYMBOL && !playableArray[aroundCharI, aroundCharJ].IsMarked)
                        {
                            //Checks if the current location is hidden and is it an empty cell.
                            //If it does, starts all over again from the current location.
                            if (playableArray[aroundCharI, aroundCharJ].IsHidden && playableArray[aroundCharI, aroundCharJ].CellValue == StringUtilities.EMPTY_CELL_SYMBOL)
                            {
                                UnlockSlotsIfEmpty(playableArray, upAndDown, tempSides = CursorHandler.CursorOffSet, tempUpAndDown = upAndDown, sidesCount = aroundCharJ, upAndDownCount = aroundCharI);
                                break;
                            }

                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
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
                        CursorHandler.CursorOffSet += 2;
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
            CursorHandler.CursorOffSet = tempForLeft;
        }
    }
}
