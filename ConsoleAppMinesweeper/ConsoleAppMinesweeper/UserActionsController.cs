using System;

namespace ConsoleAppMinesweeper
{
    public class UserActionsController
    {
        public int IsHidden { get; private set; }

        public int GetMinesNum()
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

        public int GetValidLength(int limit, string scope)
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

        public int GetColumn()
        {
            int colNum;
            do
            {
                TextManager.PrintUserDifficultyTitle();
                Console.SetCursorPosition(29, 6);
                Console.Write("Enter the board height: ");
            } while (!int.TryParse(Console.ReadLine(), out colNum));
            return colNum;
        }

        public int GetRow()
        {
            int rowNum;
            do
            {
                TextManager.PrintUserDifficultyTitle();
                Console.SetCursorPosition(28, 6);
                Console.Write("Enter the board width: ");

            } while (!int.TryParse(Console.ReadLine(), out rowNum));
            return rowNum;
        }

        /// <summary>
        /// The logic of the movement around the field and pressed playable buttons.
        /// </summary>
        /// <param name="gameBoard">The 2D Array the user is playing on.</param>
        /// <localVariable name="upAndDown">The number for the SetCourserPosition. - Top </localVariable>
        /// <localVariable name="sidesCount">Tracking on which cell the user is currently on. - Rows</localVariable>
        /// <localVariable name="upAndDownCount">Tracking on which cell the user is currently on. - Columns</localVariable>
        /// <localVariable name="tempSides">Saved the last number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempUpAndDown">Saved the last number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="tempForSides">Saved the first number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempForUpAndDown">Saved the first number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="redo">True / false if the player - won / lost - the game.</localVariable>
        public void MovingAlongTheArray(Cell[,] gameBoard, int minesCounter)
        {
            int upAndDown = 5, sidesCount = 0, upAndDownCount = 0, tempSides = CursorHandler.CursorOffSet + 2, tempUpAndDown = upAndDown + 1,
                tempForSides = CursorHandler.CursorOffSet + 2, tempForUpAndDown = upAndDown + 1;
            bool redo = false;

            ConsoleKeyInfo keyInfo;
            do
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    //Moving the cursor 1 time to the left.
                    case ConsoleKey.LeftArrow:
                        CursorHandler.CursorOffSet -= 2;
                        sidesCount--;
                        //Checks if the user is staying on the field.
                        if (CursorHandler.CursorOffSet < 0)
                        {
                            CursorHandler.CursorOffSet += 2;
                            sidesCount++;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                        break;

                    //Moving the cursor 1 time to the right.
                    case ConsoleKey.RightArrow:
                        CursorHandler.CursorOffSet += 2;
                        sidesCount++;
                        //Checks if the user is staying on the field.
                        if (CursorHandler.CursorOffSet > 79)
                        {
                            CursorHandler.CursorOffSet -= 2;
                            sidesCount--;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
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
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
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
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                        break;

                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                        Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);

                        //Checks if the Enter was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < gameBoard.GetLength(0) && sidesCount < gameBoard.GetLength(1))
                        {

                            //Checks if the current location is hidden.
                            if (gameBoard[upAndDownCount, sidesCount].IsHidden)
                            {

                                //Checks if the current location contains exclamation mark.
                                if (gameBoard[upAndDownCount, sidesCount].IsMarked)
                                {
                                    Console.SetCursorPosition(0, 0);
                                    Console.Write("To clear this exclamation mark press - Delete");
                                    GameManager.SetThreadSleep(2500);
                                    Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                                }

                                //Checks if the current location contains a mine.
                                else if (gameBoard[upAndDownCount, sidesCount].CellValue == StringUtilities.MINE_SYMBOL)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    gameBoard[upAndDownCount, sidesCount].IsHidden = false;
                                    Console.Write(gameBoard[upAndDownCount, sidesCount].CellValue);
                                    gameBoard[upAndDownCount, sidesCount].CellValue = '0';
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    upAndDown = tempForUpAndDown;

                                    //Prints all the mines on the field. Then waits 2.5 seconds, clears everything and print "Game over".
                                    for (int i = 1; i < gameBoard.GetLength(0) - 1; i++)
                                    {
                                        CursorHandler.CursorOffSet = tempForSides;
                                        for (int j = 1; j < gameBoard.GetLength(1) - 1; j++)
                                        {
                                            if (gameBoard[i, j].CellValue == StringUtilities.MINE_SYMBOL)
                                            {
                                                Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                                                (gameBoard[i, j].IsHidden) = false;
                                                Console.Write(gameBoard[i, j].CellValue);
                                            }
                                            CursorHandler.CursorOffSet += 2;
                                        }
                                        upAndDown++;
                                    }
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    GameManager.SetThreadSleep(2500);
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
                                    BoardManager.UnlockSlotsIfEmpty(gameBoard, upAndDown, tempSides = CursorHandler.CursorOffSet, tempUpAndDown = upAndDown, sidesCount, upAndDownCount);
                                    IsHidden = 0;

                                    //Counts the number of hidden values.
                                    for (int i = 1; i < gameBoard.GetLength(0) - 1; i++)
                                    {
                                        for (int j = 1; j < gameBoard.GetLength(1) - 1; j++)
                                        {
                                            if (gameBoard[i, j].IsHidden)
                                            {
                                                IsHidden++;
                                            }
                                        }
                                    }
                                    //Checks if the amount of hidden values is equal to the number of mines.
                                    //If it does, uses a nested loop to print all the mines locations as exclamation marks.
                                    if (IsHidden == minesCounter)
                                    {
                                        upAndDown = tempForUpAndDown;
                                        for (int i = 1; i < gameBoard.GetLength(0) - 1; i++)
                                        {
                                            CursorHandler.CursorOffSet = tempForSides;
                                            for (int j = 1; j < gameBoard.GetLength(1) - 1; j++)
                                            {
                                                if (gameBoard[i, j].CellValue == StringUtilities.MINE_SYMBOL)
                                                {
                                                    Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                                                    (gameBoard[i, j].IsMarked) = true;
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.Write(gameBoard[i, j].CellValue = StringUtilities.MARK_SYMBOL);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                CursorHandler.CursorOffSet += 2;
                                            }
                                            upAndDown++;
                                        }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        GameManager.SetThreadSleep(2500);
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
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                        }
                        break;

                    case ConsoleKey.Insert:

                        //Checks if the Insert was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < gameBoard.GetLength(0) && sidesCount < gameBoard.GetLength(1))
                        {
                            //Checks if the current location isn't hidden or marked by exclamation mark.
                            if (!gameBoard[upAndDownCount, sidesCount].IsHidden || gameBoard[upAndDownCount, sidesCount].IsMarked)
                            {
                                break;
                            }
                            gameBoard[upAndDownCount, sidesCount].IsMarked = true;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(gameBoard[upAndDownCount, sidesCount].MarkValue = StringUtilities.MARK_SYMBOL);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to redecorate my Minesweeper I see..\nMaybe it's better to focus on finishing it.");
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                        }
                        break;

                    case ConsoleKey.Delete:

                        //Checks if the Delete was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < gameBoard.GetLength(0) && sidesCount < gameBoard.GetLength(1))
                        {
                            //Checks if the current location is marked by exclamation mark.
                            if (gameBoard[upAndDownCount, sidesCount].IsMarked)
                            {
                                gameBoard[upAndDownCount, sidesCount].IsMarked = false;
                                Console.Write(gameBoard[upAndDownCount, sidesCount].MarkValue = '\0');
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("There is nothing to delete out there..\nMaybe it's better to focus on the Minesweeper.");
                            GameManager.SetThreadSleep(2500);
                            Console.SetCursorPosition(CursorHandler.CursorOffSet, upAndDown);
                        }
                        break;
                }
            } while (!redo);
        }
    }
}
