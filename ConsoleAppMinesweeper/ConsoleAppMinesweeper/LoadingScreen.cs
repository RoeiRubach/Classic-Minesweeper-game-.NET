using System;
using System.Threading;

namespace ConsoleAppMinesweeper
{
    public static class LoadingScreen
    {
        public static void Load()
        {
            int dotMovementNum = 30;
            const int REPETITIONS_NUM = 12;

            for (int i = 0; i <= REPETITIONS_NUM; i++)
            {
                Console.SetCursorPosition(dotMovementNum, 7);
                Console.Write("  .");
                Thread.Sleep(135);
                dotMovementNum++;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
