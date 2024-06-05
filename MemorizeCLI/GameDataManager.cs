using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameDataManager
    {

        private int m_NumOfColumns;
        private int m_NumOfRows;
        private Player m_CurrentPlayer;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private GameBoard m_Board;
        private eGameStatus m_GameStatus;
        private eGameType m_GameType;
        private eComputerLevel m_ComputerLevel;
        private int m_NumOfComputerMovesInGame;

        public GameDataManager(int i_NumOfRows, int i_NumOfColumns, eGameType i_GameType,
            Player i_FirstPlayer, Player i_SecondPlayer)
        {
            m_NumOfColumns = i_NumOfColumns;
            m_NumOfRows = i_NumOfRows;
            m_FirstPlayer = i_FirstPlayer;
            m_SecondPlayer = i_SecondPlayer;
            m_GameStatus = eGameStatus.CurrentlyRunning;
            m_CurrentPlayer = i_FirstPlayer;
            m_Board = new GameBoard(i_NumOfRows,i_NumOfColumns );
            m_NumOfComputerMovesInGame = 0;
        }

        public int NumOfComputerMovesInGame
        {
            get
            {
                return m_NumOfComputerMovesInGame;
            }
            set
            {
                m_NumOfComputerMovesInGame = value;
            }
        }

        public eGameType GameType
        {
            get
            {
                return m_GameType;
            }
            set
            {
                m_GameType = value;
            }
        }

        public eGameStatus GameStatus
        {
            get
            {
                return m_GameStatus;
            }
            set
            {
                m_GameStatus = value;
            }
        }

        public eComputerLevel ComputerLevel
        {
            get
            {
                return m_ComputerLevel;
            }
            set
            {
                m_ComputerLevel = value;
            }
        }



        public int NumOfColumns
        {
            get
            {
                return m_NumOfColumns;
            }
            set
            {
                m_NumOfColumns = value;
            }
        }

        public int NumOfRows
        {
            get
            {
                return m_NumOfRows;
            }
            set
            {
                m_NumOfRows = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
            set
            {
                m_CurrentPlayer = value;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return m_FirstPlayer;
            }
            set
            {
                m_FirstPlayer = value;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return m_SecondPlayer;
            }
            set
            {
                m_SecondPlayer = value;
            }
        }

        public GameBoard GameBoard
        {
            get
            {
                return m_Board;
            }
        }
    }
}
