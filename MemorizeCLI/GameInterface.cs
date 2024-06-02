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


        public GameInterface()
        {
            m_GameDataManager = new GameDataManager(k_NumOfColumns, k_NumOfRows);
        }
        public bool GetUserInput()
        {
            Console.WriteLine("Please Enter Your First Player Name: ");
            //string userName = Console.ReadLine();
            m_GameDataManager.FirstPlayer.PlayerName = Console.ReadLine();
                
            Console.WriteLine($"Hello {m_GameDataManager.FirstPlayer.PlayerName}!\n");

            // Get game type
            int gameType=0;
            Console.WriteLine("Please Enter The Game Type: ");
            Console.WriteLine("1) Human Vs Human");
            Console.WriteLine("2) Human Vs Computer");
            
            bool isValidInput = false;

            while (!isValidInput)
            {
                if (int.TryParse(Console.ReadLine(), out gameType) && (gameType == 1 || gameType == 2))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input, Choose 1 OR 2\n");
                    Console.WriteLine("Please Enter The Game Type: ");
                    Console.WriteLine("1) Human Vs Human");
                    Console.WriteLine("2) Human Vs Computer");
                }
            }

            e_GameType = gameType == 1 ? eGameType.HumanVHuman : eGameType.HumanVComputer;
            if (e_GameType == eGameType.HumanVHuman)
            {
                Console.WriteLine("Please Enter Your Second Player Name: ");
                m_GameDataManager.SecondPlayer.PlayerName = Console.ReadLine();
                m_GameDataManager.SecondPlayer.PlayerType = ePlayerType.Human;
            }
            else
            {
                m_GameDataManager.SecondPlayer.PlayerType = ePlayerType.Computer;
            }

            // Get number of columns
            Console.WriteLine("Please Enter The Number Of Columns: ");
            while (!int.TryParse(Console.ReadLine(), out k_NumOfColumns) || k_NumOfColumns <= 0)
            {
                Console.WriteLine("Invalid Input, Please Enter A Positive Integer\n");
            }

            // Get number of rows
            Console.WriteLine("Please Enter The Number Of Rows: ");
            while (!int.TryParse(Console.ReadLine(), out k_NumOfRows) || k_NumOfRows <= 0)
            {
                Console.WriteLine("Invalid Input, Please Enter A Positive Integer\n");
            }

            return isValidInput;
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
