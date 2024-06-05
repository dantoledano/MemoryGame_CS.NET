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
        private readonly eGameType r_GameType;
        private BoardTile m_FirstSelection;
        private BoardTile m_SecondSelection;
        private BoardTile m_CurrentSelection;
        private bool m_IsFirstSelection;
        private bool m_IsMatch;
        

        public GameLogicManager(Player i_Player1, Player i_Player2, int i_NumOfRows, int i_NumOfColumns, eGameType i_GameType)
        {
            m_GameDataManager = new GameDataManager(i_NumOfRows, i_NumOfColumns, i_GameType , i_Player1, i_Player2);
            r_GameType = i_GameType;
            //m_GameDataManager.GameStatus = eGameStatus.CurrentlyRunning;
            m_IsFirstSelection = true;
            m_IsMatch = false;


            //if(this.r_GameType == eGameType.HumanVComputer)
            //{
            //    r_AiMemory = new Dictionary<BoardTile, char>;
            //}
        }

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

        public int BoardHeight
        {
            get
            {
                return m_GameDataManager.NumOfRows;
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_GameDataManager.NumOfColumns;
            }
        }

        public int firstPlayerScore
        {
            get
            {
                return m_GameDataManager.FirstPlayer.PlayerPoints;
            }
        }

        public bool AreMatchingTiles
        {
            get
            {
                return m_IsMatch;
            }
            set
            {
                m_IsMatch = value;
            }
        }

        public bool IsFirstSelection
        {
            get
            {
                return m_IsFirstSelection;
            }
        }

        public int secondPlayerScore
        {
            get
            {
                return m_GameDataManager.SecondPlayer.PlayerPoints;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return this.m_GameDataManager.CurrentPlayer;
            }
            set
            {
                this.m_GameDataManager.CurrentPlayer = value;
            }
        }

        public GameDataManager GameDataManager
        {
            get
            {
                return m_GameDataManager;
            }
        }

        public void updateTurn(ref BoardTile i_UserSelection)
        {

            if (!m_IsMatch)
            {
                updateNextTurn(ref i_UserSelection);
            }
            if ((firstPlayerScore + secondPlayerScore) == ((BoardHeight * BoardWidth) / 2))
            {
                s_currentGameStatus = eGameStatus.Over;
            }

        }


        // private void AddToAiMemory(BoardTile i_UserSelection);//לשנות שם פרמטר

        private void updateNextTurn(ref BoardTile i_UserSelection)
        {
            this.m_CurrentSelection = i_UserSelection;
            //if(this.r_GameType == eGameType.HumanVComputer)
            //{

            //}

            if (this.m_IsFirstSelection)
            {
                this.m_FirstSelection = this.m_CurrentSelection;
                m_CurrentSelection.IsRevealed = true;
                this.m_IsFirstSelection = false;
            }
            else
            {
                BoardTile firstBoardTileSelected = m_FirstSelection;
                BoardTile secondBoardTileSelected = m_CurrentSelection;
                m_IsMatch = false;
                secondBoardTileSelected.IsRevealed = true;
                m_IsMatch = firstBoardTileSelected.Value == secondBoardTileSelected.Value;

                if (m_IsMatch)
                {
                    //if(this.r_GameType == eGameType.HumanVComputer)
                    //{

                    //}

                    CurrentPlayer.PlayerPoints++;
                }
                //m_IsMatch = false;
                m_IsFirstSelection = true;
            }
        }

        public void TogglePlayer()
        {
            CurrentPlayer = CurrentPlayer == this.m_GameDataManager.FirstPlayer ?
                                this.m_GameDataManager.SecondPlayer :
                                this.m_GameDataManager.FirstPlayer;
            this.m_CurrentSelection.IsRevealed = false;
            this.m_FirstSelection.IsRevealed = false;
            this.m_IsMatch = false;
        }

        //public void RestartGame(int i_NumOfRows, int i_NumOfColumns)// לחשוב על אופציה של ריסטארט
        //{
        //    CurrentPlayer = this.m_GameDataManager.FirstPlayer;
        //    this.m_GameDataManager.FirstPlayer.PlayerPoints = 0;
        //    this.m_GameDataManager.SecondPlayer.PlayerPoints = 0;

        //    this.m_GameDataManager.NumOfRows = i_NumOfRows;
        //    this.m_GameDataManager.NumOfColumns = i_NumOfColumns;
        //}



    }
}
