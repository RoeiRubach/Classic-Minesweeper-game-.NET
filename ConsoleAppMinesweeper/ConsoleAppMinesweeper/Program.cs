namespace ConsoleAppMinesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager;
            bool isGameOver = false;

            while (!isGameOver)
            {
                gameManager = new GameManager();
                gameManager.InitializeGame();
                gameManager.Play();

                isGameOver = gameManager.IsAnotherGame();
            }
        }
    }
}