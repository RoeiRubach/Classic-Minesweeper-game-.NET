using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppMinesweeper
{
    public class CursorHandler
    {
        public static int ILeft;

        /// <summary>
        /// Sets the iLeft according the user's difficulty. For later used by the SetCursorPosition(Left).
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="difficulty">The copy of the user's difficulty input.</param>
        public void CordsForILeft(Cell[,] game2DArray, string difficulty)
        {
            switch (difficulty)
            {
                case StringUtilities.BEGINNER:
                    ILeft = 29;
                    break;

                case StringUtilities.AMATEUR:
                    ILeft = 22;
                    break;

                case StringUtilities.EXPERT:
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
    }
}
