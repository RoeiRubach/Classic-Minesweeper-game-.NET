namespace ConsoleAppMinesweeper
{
    public struct Cell
    {
        public bool IsHidden;
        public bool IsMarked;
        public char CellValue;
        public char MarkValue;
        public int MinesAround;
    }
}