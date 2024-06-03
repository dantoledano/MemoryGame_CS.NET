using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameLogicManager
    {
        private const int k_MaxMatrixRows = 6;
        private const int k_MaxMatrixColumns = 6;
        private const int k_MinMatrixRows = 4;
        private const int k_MinMatrixColumns = 4;
        private static eGameStatus s_currentGameStatus = eGameStatus.MainMenu;
        private GameDataManager m_GameDataManager;



        public static eGameStatus GameStatus
        {
            get
            {
                return s_currentGameStatus;
            }
        }

        public static int MaxMatrixRows
        {
            get
            {
                return k_MaxMatrixRows;
            }
        }

        public static int MaxMatrixColumns
        {
            get
            {
                return k_MaxMatrixColumns;
            }
        }

        public static int MinMatrixRows
        {
            get
            {
                return k_MinMatrixRows;
            }
        }

        public static int MinMatrixColumns
        {
            get
            {
                return k_MinMatrixColumns;
            }
        }
    }
}
