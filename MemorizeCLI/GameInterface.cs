﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameInterface
    {
        //private int k_NumOfColumns;
        //private int k_NumOfRows;
        //private eGameType e_GameType;
        //private GameDataManager m_GameDataManager;
        private const string QuitGame = "Q";
        private readonly GameMenu r_GameMenu;
        private  GameLogicManager m_GameLogicManager;


        public GameInterface()
        {
            r_GameMenu = new GameMenu();
        }

        public void StartGame()
        {
            //if (m_GameLogicManager.GameStatus == eGameStatus.MainMenu)
            //{
            //    runMenu();
            //}
            runMenu();
            RunGame();
            finishGame();
        }

        private void finishGame()
        {

                //DisplayGameInterface();
                printWinnner();
                Console.WriteLine("BYE\n");
                exitGame();
                //Console.WriteLine(m_GameLogicManager.GetGameOverStatus());

                //bool restartNeeded = CheckRestart();

                //if (restartNeeded)
                //{
                //    ClearWindow();
                //    restartGame();
                //}
                //else
                //{
                //    stopGame();
                //}
        }

        private void printWinnner()
        {
            if(m_GameLogicManager.firstPlayerScore == m_GameLogicManager.secondPlayerScore)
            {
                Console.WriteLine("It's A Tie !!!\n");
            }
            else
            {
                string name = m_GameLogicManager.firstPlayerScore > m_GameLogicManager.secondPlayerScore
                                  ? m_GameLogicManager.GameDataManager.FirstPlayer.PlayerName
                                  : m_GameLogicManager.GameDataManager.SecondPlayer.PlayerName;
                Console.WriteLine($"{name} Is The Winner !!!\n");
            }
        }

        private void RunGame()
        {
            while (m_GameLogicManager.GameDataManager.GameStatus == eGameStatus.CurrentlyRunning)
            {
                DisplayGameInterface();
                string playerInput = getPlayerNextMove();
                updateTurnAndView(playerInput);
            }
        }

        private void updateTurnAndView(string i_PlayerInput)
        {
            BoardTile selectedTile = m_GameLogicManager.GameDataManager.GameBoard.GetTile(i_PlayerInput);
            if (i_PlayerInput == "Q")
            {
                exitGame();
            }
            else
            {
                m_GameLogicManager.updateTurn(ref selectedTile);
                DisplayGameInterface();
                if (m_GameLogicManager.IsFirstSelection && !m_GameLogicManager.AreMatchingTiles)
                {
                    Console.WriteLine("Not Matching! Try again next time.");
                    System.Threading.Thread.Sleep(2000);
                    m_GameLogicManager.TogglePlayer();
                }
                m_GameLogicManager.AreMatchingTiles = false;
            }
        }

        private void exitGame()
        {
            Console.WriteLine("Thanks For Playing. Have A Nice Day!");
            Environment.Exit(0);
        }


        private void runMenu()
        {
            string firstPlayerName, secondPlayerName;
            int columns, rows;
            eComputerLevel computerLevel;
            eGameType gameType =
                r_GameMenu.RunMenuScreen(out firstPlayerName, out secondPlayerName, out rows, out columns, out computerLevel);
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

            m_GameLogicManager = new GameLogicManager(firstPlayer, secondPlayer, rows, columns,gameType);
        }


        private void DisplayGameInterface()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("{0}'s Turn\n", m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerName);

            string scoreBoard = string.Format("Score Board: {0}:{1} | {2}:{3}\n",
                m_GameLogicManager.GameDataManager.FirstPlayer.PlayerName,
                m_GameLogicManager.GameDataManager.FirstPlayer.PlayerPoints,
                m_GameLogicManager.GameDataManager.SecondPlayer.PlayerName,
                m_GameLogicManager.GameDataManager.SecondPlayer.PlayerPoints);

            Console.WriteLine(scoreBoard);
            //m_GameDataManager.GameBoard.InitializeBoard();
           m_GameLogicManager.GameDataManager.GameBoard.DisplayBoard();
        }

        public void DisplayGameSettings()
        {
            Console.WriteLine($"Game Type: {m_GameLogicManager.GameDataManager.GameType}");
            Console.WriteLine($"Number of Columns: {m_GameLogicManager.GameDataManager.NumOfColumns}");
            Console.WriteLine($"Number of Rows: {m_GameLogicManager.GameDataManager.NumOfRows}");
            Console.WriteLine($"First Player Type: {m_GameLogicManager.GameDataManager.FirstPlayer.PlayerType}");
            Console.WriteLine($"Second Player Type: {m_GameLogicManager.GameDataManager.SecondPlayer.PlayerType}");
        }

        private string getPlayerNextMove()
        {
            string playerNextMove = "";

            if (m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerType == ePlayerType.Human)
            {
                playerNextMove = GetInputFromHumanPlayer();
            }
            else
            {
                playerNextMove = m_GameLogicManager.CalculateComputerInput();
                displayComputerProccess();
            }

            return playerNextMove;
        }

        private string GetInputFromHumanPlayer()
        {
            string playerNextMove = "";

            bool isNextMoveIsValid = false;

            while (!isNextMoveIsValid)
            {
                Console.WriteLine("Now {0} has to choose next Move", m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerName);
                playerNextMove = Console.ReadLine();
                isNextMoveIsValid = ValidateHumanPlayerNextMove(playerNextMove);
            }

            return playerNextMove;
        }

        private bool ValidateHumanPlayerNextMove(string i_playerNextMove)
        {
            bool isNextMoveIsValid = true;

            if (i_playerNextMove != null)
            {
               i_playerNextMove = i_playerNextMove.ToUpper();

               if (i_playerNextMove != QuitGame)
               {

                   isNextMoveIsValid = ValidateTileHumanPlayerPicked(i_playerNextMove);

                   if (isNextMoveIsValid)
                   {
                       isNextMoveIsValid = ValidateTileIsNotHidden(i_playerNextMove);
                   }
               }

            }

            return isNextMoveIsValid;
        }

        private bool ValidateTileIsNotHidden(string i_TileHumanPlayerPicked)
        {
            bool isHiddenTile = true;

            if (m_GameLogicManager.GameDataManager.GameBoard.GetTile(i_TileHumanPlayerPicked).IsRevealed)
            {
                Console.WriteLine("Wrong Input. You Picked A Tile That Is Already Revealed!\n");
                isHiddenTile = false;
            }
            return isHiddenTile;
        }

        private bool ValidateTileHumanPlayerPicked(string i_TileHumanPlayerPicked)
        {
            bool isValidTileChoice;

            if (i_TileHumanPlayerPicked.Length != 2)
            {
                Console.WriteLine("Wrong Input. Input Should Look Like: A2\n");
                isValidTileChoice = false;
            }
            else
            {
                char LetterColumns = i_TileHumanPlayerPicked[0];
                char DigitRow = i_TileHumanPlayerPicked[1];
                isValidTileChoice = ValidateColumnLetter(LetterColumns) && ValidateRowDigit(DigitRow);
            }

            return isValidTileChoice;
        }

        private bool ValidateRowDigit(char i_ChosenRow)
        {
            bool isValidRowDigit = true;
            char largerstValidDigit = (char)('0' + m_GameLogicManager.GameDataManager.NumOfRows);

            if (i_ChosenRow < '1' || i_ChosenRow > largerstValidDigit)
            {
                Console.WriteLine("Wrong Input. Enter Row Between {0}-{1}",1,m_GameLogicManager.GameDataManager.NumOfRows);
                isValidRowDigit = false;
            }

            return isValidRowDigit;
        }

        private bool ValidateColumnLetter(char i_ChosenColumn)
        {
            bool isValidLetterColumn = true;
            char maxValidLetter = (char)('A' + m_GameLogicManager.GameDataManager.NumOfColumns);

            if (i_ChosenColumn < 'A' || i_ChosenColumn > maxValidLetter)
            {
                Console.WriteLine("Wrong Input. Enter Column Between {0}-{1}", 'A', maxValidLetter);
                isValidLetterColumn = false;
            }

            return isValidLetterColumn;
        }

        private void displayComputerProccess()
        {
            string computerMessage = "Computer Recalls A Match.";

            if (!m_GameLogicManager.ComputerHasAMatch)
            {
                computerMessage = "Proceccing.";
                Console.Write(computerMessage);
                System.Threading.Thread.Sleep(1000);
                Console.Write('.');
                System.Threading.Thread.Sleep(1000);
                Console.Write('.');
                System.Threading.Thread.Sleep(1000);
            }

            else
            {
                Console.Write(computerMessage);
                System.Threading.Thread.Sleep(2000);
            }
        }

        public void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
