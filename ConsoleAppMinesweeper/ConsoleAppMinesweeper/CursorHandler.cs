namespace ConsoleAppMinesweeper
{
    public class CursorHandler
    {
        public static int CursorOffSet;

        public void GetCursorOffSet(Cell[,] game2DArray, string difficulty)
        {
            switch (difficulty)
            {
                case StringUtilities.BEGINNER:
                    CursorOffSet = 29;
                    break;

                case StringUtilities.AMATEUR:
                    CursorOffSet = 22;
                    break;

                case StringUtilities.EXPERT:
                    CursorOffSet = 8;
                    break;

                default:
                    if (game2DArray.GetLength(0) >= 34 || game2DArray.GetLength(1) >= 34)
                        CursorOffSet = 0;

                    else if (game2DArray.GetLength(0) >= 27 || game2DArray.GetLength(1) >= 27)
                        CursorOffSet = 7;

                    else if (game2DArray.GetLength(0) >= 20 || game2DArray.GetLength(1) >= 20)
                        CursorOffSet = 15;

                    else if (game2DArray.GetLength(0) >= 13 || game2DArray.GetLength(1) >= 13)
                        CursorOffSet = 21;

                    else if (game2DArray.GetLength(0) >= 6 || game2DArray.GetLength(1) >= 6)
                        CursorOffSet = 31;

                    break;
            }
        }
    }
}
