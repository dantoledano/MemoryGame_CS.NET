using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameInterface
    {
        private int k_NumOfColumns;
        private int k_NumOfRows;
        private eGameType e_GameType;
        private GameDataManager m_GameDataManager;
        private readonly GameMenu r_GameMenu;


        public GameInterface()
        {
            m_GameDataManager = new GameDataManager(k_NumOfColumns, k_NumOfRows);
            
            r_GameMenu = new GameMenu();
        }

        public void startGame()
        {
            if (GameLogicManager.GameStatus == eGameStatus.MainMenu)
            {
                runMenu();
            }
            ClearScreen();
            
            DisplayGameInterface();
            //startGame
        }

        private void runMenu()
        {
            string firstPlayerName, secondPlayerName;
            int columns, rows;

            eGameType gameType =
                r_GameMenu.RunMenuScreen(out firstPlayerName, out secondPlayerName, out columns, out rows);
            Player firstPlayer = new Player(firstPlayerName, ePlayerType.Human);
            Player? secondPlayer;
            if (gameType == eGameType.HumanVHuman)
            {
                secondPlayer = new Player(secondPlayerName, ePlayerType.Human);
            }
            else
            {
                secondPlayer = new Player(secondPlayerName, ePlayerType.Computer);
            }

            m_GameDataManager = new GameDataManager(columns, rows, gameType, firstPlayer,
                secondPlayer);
            


        }


        private void DisplayGameInterface()
        {
            Console.WriteLine($"{m_GameDataManager.CurrentPlayer.PlayerName}'s Turn\n");

            string scoreBoard = string.Format("Score Board: {0}:{1} | {2}:{3}",
                m_GameDataManager.FirstPlayer.PlayerName,
                m_GameDataManager.FirstPlayer.PlayerPoints,
                m_GameDataManager.SecondPlayer.PlayerName,
                m_GameDataManager.SecondPlayer.PlayerPoints);

            Console.WriteLine(scoreBoard);
            //m_GameDataManager.GameBoard.initializeBoard();
            m_GameDataManager.GameBoard.DisplayBoard();
        }

        public void DisplayGameSettings()
        {
            Console.WriteLine($"Game Type: {e_GameType}");
            Console.WriteLine($"Number of Columns: {k_NumOfColumns}");
            Console.WriteLine($"Number of Rows: {k_NumOfRows}");
            Console.WriteLine($"First Player Type: {m_GameDataManager.FirstPlayer.PlayerType}");
            Console.WriteLine($"Second Player Type: {m_GameDataManager.SecondPlayer.PlayerType}");
        }

        public void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
