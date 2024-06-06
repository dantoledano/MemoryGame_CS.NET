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
        private readonly GameDataManager r_GameDataManager;
        private readonly eGameType r_GameType;
        private readonly Dictionary<BoardTile, char> r_ComputerMemory;
        private BoardTile m_FirstSelection;
        private BoardTile m_CurrentSelection;
        private bool m_ComputerFoundMatch;
        private bool m_AreOptionalMatches;
        private BoardTile m_AiComputerSelection;
        private bool m_IsFirstSelection;
        private bool m_IsMatch;
        private bool m_ComputerHasMatch;
        private static readonly Random sr_Random = new Random();
        
        public GameLogicManager(Player i_Player1, Player i_Player2, int i_NumOfRows, int i_NumOfColumns, eGameType i_GameType)
        {
            r_GameDataManager = new GameDataManager(i_NumOfRows, i_NumOfColumns, i_GameType, i_Player1, i_Player2);
            r_GameType = i_GameType;
            m_IsFirstSelection = true;
            m_IsMatch = false;

            if(r_GameType == eGameType.HumanVComputer)
            {
                r_ComputerMemory = new Dictionary<BoardTile, char>();
            }
        }
        public bool ComputerHasMatch
        {
            get
            {
                return m_ComputerHasMatch;
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
                return r_GameDataManager.NumOfRows;
            }
        }

        public int BoardWidth
        {
            get
            {
                return r_GameDataManager.NumOfColumns;
            }
        }

        public int FirstPlayerScore
        {
            get
            {
                return r_GameDataManager.FirstPlayer.PlayerPoints;
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

        public int SecondPlayerScore
        {
            get
            {
                return r_GameDataManager.SecondPlayer.PlayerPoints;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return r_GameDataManager.CurrentPlayer;
            }
            set
            {
                this.r_GameDataManager.CurrentPlayer = value;
            }
        }

        public GameDataManager GameDataManager
        {
            get
            {
                return r_GameDataManager;
            }
        }

        public void UpdateTurn(ref BoardTile i_UserSelection)
        {

            if (!m_IsMatch)
            {
                updateNextTurn(ref i_UserSelection);
            }

            if ((FirstPlayerScore + SecondPlayerScore) == ((BoardHeight * BoardWidth) / 2))
            {
                r_GameDataManager.GameStatus = eGameStatus.Over;
            }

        }
        private void updateNextTurn(ref BoardTile i_UserSelection)
        {
            this.m_CurrentSelection = i_UserSelection;

            if (this.r_GameType == eGameType.HumanVComputer)
            {
                addToComputerMemory(i_UserSelection);
            }

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

                if(m_IsMatch)
                {
                    CurrentPlayer.PlayerPoints++;
                    if(this.r_GameType == eGameType.HumanVComputer)
                    {
                        r_ComputerMemory.Remove(m_FirstSelection);
                        r_ComputerMemory.Remove(m_CurrentSelection);
                    }
                }

                m_IsFirstSelection = true;
            }
        }

        public string GetAiNextMove()
        {
            string aiComputerSelection;
            
            if(r_ComputerMemory.Count == 0)
            {
                m_AreOptionalMatches = false;
                aiComputerSelection = getRandomBoardTile();
            }
            else
            {
                aiComputerSelection = m_IsFirstSelection ? getFirstAiSelection() : getSecondAiSelection();
            }

            return aiComputerSelection;
        }
        private bool findBoardTileMatch(ref string i_MemorizedMatchingLetter)
        {
            bool foundMatch = false;

            foreach (var firstMemorizedLetter in r_ComputerMemory)
            {
                foreach (var secondMemorizedLetter in r_ComputerMemory)
                {
                    if (!firstMemorizedLetter.Key.Equals(secondMemorizedLetter.Key))
                    {
                        if (firstMemorizedLetter.Value == secondMemorizedLetter.Value)
                        {
                            i_MemorizedMatchingLetter = GameDataManager.GameBoard.ConvertTileToString(firstMemorizedLetter.Key.RowIndexInBoard, firstMemorizedLetter.Key.ColumnIndexInBoard);
                            m_AiComputerSelection = secondMemorizedLetter.Key;
                            foundMatch = true;
                            break;
                        }
                    }
                }
            }

            return foundMatch;
        }

        private void addToComputerMemory(BoardTile i_BoardTileToBeAdded)
        {
            if (!r_ComputerMemory.ContainsKey(i_BoardTileToBeAdded))
            {
                r_ComputerMemory.Add(i_BoardTileToBeAdded, i_BoardTileToBeAdded.Value);
            }
        }

        private string getFirstAiSelection()
        {
            string firstSelection = null;

            m_ComputerFoundMatch = findBoardTileMatch(ref firstSelection);

            if (m_ComputerFoundMatch)
            {
                m_ComputerHasMatch = true;
            }
            else
            {
                m_ComputerHasMatch = false;
                firstSelection = getRandomBoardTile();
            }

            return firstSelection;
        }

        private string getRandomBoardTile()
        {
            List<string> unrevealedBoardTiles = new List<string>(); 
            int boardTileIndexInList = 0 ;

            for (int i = 0; i < r_GameDataManager.NumOfRows; i++)
            {
                for (int j = 0; j < r_GameDataManager.NumOfColumns; j++)
                {
                    if (!GameDataManager.GameBoard.GetBoardTile(i,j).IsRevealed)
                    {
                      unrevealedBoardTiles.Add(GameDataManager.GameBoard.ConvertTileToString(i, j));
                    }
                }
            }

            boardTileIndexInList = RandomizeAnIndex(0, unrevealedBoardTiles.Count);
            m_AiComputerSelection = GameDataManager.GameBoard.GetTile(unrevealedBoardTiles[boardTileIndexInList]);

            return unrevealedBoardTiles[boardTileIndexInList];
        }


        private string getSecondAiSelection()
        {
            string secondSelection;

            if (m_ComputerHasMatch)
            {
                secondSelection = GameDataManager.GameBoard.ConvertTileToString(
                    m_AiComputerSelection.RowIndexInBoard,
                    m_AiComputerSelection.ColumnIndexInBoard);
            }
            else
            {
                secondSelection = findLetterInMemory(m_AiComputerSelection);

                if (secondSelection != null)
                {
                    m_ComputerHasMatch = true;
                }
                else
                {
                    m_ComputerHasMatch = false;
                    secondSelection = getRandomBoardTile();
                }
            }

            return secondSelection;
        }

        private string findLetterInMemory(BoardTile i_FirstSelectionBoardTile)
        {
            string foundLetter = null;

            foreach (var boardTileInMemory in r_ComputerMemory)
            {
                BoardTile currentKey = boardTileInMemory.Key;
                char firstSelectionValue = i_FirstSelectionBoardTile.Value;

                if (!currentKey.Equals(i_FirstSelectionBoardTile) && boardTileInMemory.Key.Value == firstSelectionValue)
                {
                    foundLetter = GameDataManager.GameBoard.ConvertTileToString(boardTileInMemory.Key.RowIndexInBoard, boardTileInMemory.Key.ColumnIndexInBoard);
                }
            }

            return foundLetter;
        }
        public void TogglePlayer()
        {
            CurrentPlayer = CurrentPlayer == this.r_GameDataManager.FirstPlayer ?
                                this.r_GameDataManager.SecondPlayer :
                                this.r_GameDataManager.FirstPlayer;
            this.m_CurrentSelection.IsRevealed = false;
            this.m_FirstSelection.IsRevealed = false;
            this.m_IsMatch = false;
        }

        public void ResetGameLogic(int i_NumOfRows, int i_NumOfColumns)
        {
            CurrentPlayer = this.r_GameDataManager.FirstPlayer;
            this.r_GameDataManager.FirstPlayer.PlayerPoints = 0;
            this.r_GameDataManager.SecondPlayer.PlayerPoints = 0;
            this.r_GameDataManager.NumOfRows = i_NumOfRows;
            this.r_GameDataManager.NumOfColumns = i_NumOfColumns;
            r_GameDataManager.GameBoard = new GameBoard(i_NumOfRows, i_NumOfColumns);
            m_IsFirstSelection = true;
            m_IsMatch = false;
            m_AreOptionalMatches = false;
            m_ComputerHasMatch = false;
        }

        public static int RandomizeAnIndex(int i_Start, int i_End)
        {

            return sr_Random.Next(i_Start, i_End);
        }
    }
}
