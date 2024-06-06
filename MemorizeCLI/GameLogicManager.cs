using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameLogicManager
    {
        private static readonly Random sr_Random = new Random();
        private const int k_MaxMatrixRows = 6;
        private const int k_MaxMatrixColumns = 6;
        private const int k_MinMatrixRows = 4;
        private const int k_MinMatrixColumns = 4;
        //private static eGameStatus s_currentGameStatus = eGameStatus.MainMenu;
        private GameDataManager m_GameDataManager;
        private readonly eGameType r_GameType;
        private BoardTile m_FirstSelection;
        private BoardTile m_SecondSelection;
        private BoardTile m_CurrentSelection;
        private bool m_IsFirstSelection;
        private readonly Dictionary<BoardTile, char> r_ComputerMemory;
        private bool m_IsMatch;
        private bool m_DoesComputerFoundMatch;
        private bool m_DoesComputerHasMatches;
        private BoardTile m_ComputerSelectedTile;


        public GameLogicManager(Player i_Player1, Player i_Player2, int i_NumOfRows, int i_NumOfColumns, eGameType i_GameType)
        {
            m_GameDataManager = new GameDataManager(i_NumOfRows, i_NumOfColumns, i_GameType, i_Player1, i_Player2);
            r_GameType = i_GameType;
            m_IsFirstSelection = true;

            m_IsMatch = false;
            m_DoesComputerFoundMatch = false;
            m_DoesComputerHasMatches = false;
            if (this.r_GameType == eGameType.HumanVComputer)
            {
                r_ComputerMemory = new Dictionary<BoardTile, char>();
            }
        }

        public bool ComputerHasAMatch
        {
            get
            {
                return m_DoesComputerHasMatches;
            }
            set { m_DoesComputerHasMatches = value; }

        }

        public Dictionary<BoardTile, char> ComputerMemory
        {
            get
            {
                return r_ComputerMemory;
            }
        }

        public BoardTile SelectedBoardTile
        {
            get
            {
                return m_ComputerSelectedTile;
            }
            set
            {
                m_ComputerSelectedTile = value;
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
                 GameDataManager.GameStatus = eGameStatus.Over;
            }

        }


        private void UpdateComputerMemory(BoardTile i_UserSelection)
        {

            if (!r_ComputerMemory.ContainsKey(i_UserSelection))
            {
                r_ComputerMemory.Add(i_UserSelection, i_UserSelection.Value);
            }
        }
        private void updateNextTurn(ref BoardTile i_UserSelection)
        {
            m_CurrentSelection = i_UserSelection;

            if (r_GameType == eGameType.HumanVComputer)
            {
                if (GameLogicManager.RandomizeANumber(0, 100) < 70)
                {
                    UpdateComputerMemory(m_CurrentSelection);
                }
            }

            if (m_IsFirstSelection)
            {
                m_FirstSelection = m_CurrentSelection;
                m_CurrentSelection.IsRevealed = true;
                m_IsFirstSelection = false;
            }
            else
            {
                BoardTile firstBoardTileSelected = m_FirstSelection;
                BoardTile secondBoardTileSelected = m_CurrentSelection;

                secondBoardTileSelected.IsRevealed = true;

                m_IsMatch = firstBoardTileSelected.Value == secondBoardTileSelected.Value;

                if (m_IsMatch)
                {
                    if (r_GameType == eGameType.HumanVComputer)
                    {
                        r_ComputerMemory.Remove(m_CurrentSelection);
                        r_ComputerMemory.Remove(m_FirstSelection);
                    }

                    CurrentPlayer.PlayerPoints++;
                }

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

        public string CalculateComputerInput()
        {
            string computerSelection;

            if (r_ComputerMemory.Count == 0)
            {
                m_DoesComputerHasMatches = false;
                computerSelection = getRandomUnmemorizedCell();
            }
            else
            {

                computerSelection = m_IsFirstSelection ?
                    calculateFirstSelection() :
                    calculateSecondSelection();
            }

            return computerSelection;
        }

        private string calculateFirstSelection()
        {
            string firstSelection = null;

            m_DoesComputerFoundMatch = findLetterMatch(ref firstSelection);

            if (m_DoesComputerFoundMatch)
            {
                m_DoesComputerHasMatches = true;
            }
            else
            {
                m_DoesComputerHasMatches = false;
                firstSelection = getRandomUnmemorizedCell();
            }

            return firstSelection;
        }

        private string calculateSecondSelection()
        {
            string secondSelection;

            if (m_DoesComputerFoundMatch)
            {
                secondSelection = m_ComputerSelectedTile.ParseToString();
            }
            else
            {
                secondSelection = findLetterInMemory(m_ComputerSelectedTile);

                if (secondSelection != null)
                {
                    m_DoesComputerHasMatches = true;
                }
                else
                {
                    m_DoesComputerHasMatches = false;
                    secondSelection = getRandomUnmemorizedCell();
                }
            }
            return secondSelection;
        }

        private string findLetterInMemory(BoardTile i_FirstSelectionCell)
        {
            string foundLetter = null;

            foreach (var memorizedLetter in r_ComputerMemory)
            {
                BoardTile currentKey = memorizedLetter.Key;
                char firstSelectionLetter = GameDataManager.GameBoard.BoardTile[i_FirstSelectionCell.RowIndexInBoard, i_FirstSelectionCell.ColumnIndexInBoard].Value;

                if (!currentKey.Equals(i_FirstSelectionCell) && memorizedLetter.Value == firstSelectionLetter)
                {
                    foundLetter = memorizedLetter.Key.ParseToString();
                }
            }

            return foundLetter;
        }

        private string getRandomUnmemorizedCell() //to change name
        {
            int unsavedTileCoordinatesIndex = 0;
            int row = GameDataManager.GameBoard.BoardTile.GetLength(0);
            int column = GameDataManager.GameBoard.BoardTile.GetLength(1);
            int sizeOfUnsavedTilesArray = (BoardHeight * BoardWidth) - r_ComputerMemory.Count;
            BoardTile[] unsavedTiles = new BoardTile[sizeOfUnsavedTilesArray];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (!GameDataManager.GameBoard.BoardTile[i, j].IsRevealed)
                    {
                        if (!r_ComputerMemory.ContainsKey(new BoardTile(i, j)))
                        {
                            if(unsavedTileCoordinatesIndex < sizeOfUnsavedTilesArray)
                                unsavedTiles[unsavedTileCoordinatesIndex++] = new BoardTile(i, j);
                        }
                    }
                }
            }

            unsavedTileCoordinatesIndex = RandomizeANumber(0, unsavedTileCoordinatesIndex);
            m_ComputerSelectedTile = unsavedTiles[unsavedTileCoordinatesIndex];

            return m_ComputerSelectedTile.ParseToString();
        }

        private bool findLetterMatch(ref string i_MemorizedMatchingLetter)
        {
            bool foundMatch = false;

            foreach (var firstCharInComputerMemory in r_ComputerMemory)
            {
                foreach (var secondCharInComputerMemory in r_ComputerMemory)
                {
                    if (!firstCharInComputerMemory.Key.Equals(secondCharInComputerMemory.Key))
                    {
                        if (firstCharInComputerMemory.Value == secondCharInComputerMemory.Value)
                        {
                            i_MemorizedMatchingLetter = firstCharInComputerMemory.Key.ParseToString();
                            m_ComputerSelectedTile = secondCharInComputerMemory.Key;
                            foundMatch = true;
                        }
                    }
                }
            }
            
            return foundMatch;
        }

        public static int RandomizeANIndex(int i_Start, int i_End)
        {
            return sr_Random.Next(i_Start, i_End);
        }
    }
}
