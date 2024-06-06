using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameInterface
    {
        private const string k_QuitGame = "Q";
        private const string k_RestartGame = "R";
        private const int k_DefaultNumOfRows = 4;
        private const int k_DefaultNumOfColumns = 4;
        private readonly GameMenu r_GameMenu;
        private GameLogicManager m_GameLogicManager;


        public GameInterface()
        {
            r_GameMenu = new GameMenu();
        }
        /* ----------------------------------------------- */

        public void StartGame()
        {
            runMenu();
            runGame();
        }
        /* ----------------------------------------------- */
        public void RestartGame()
        {
            int newNumOfRows = k_DefaultNumOfRows;
            int newNumOfColumns = k_DefaultNumOfColumns;
            m_GameLogicManager.GameDataManager.GameStatus = eGameStatus.CurrentlyRunning;
            r_GameMenu.GetAndValidateMatrixDimensions(out newNumOfRows, out newNumOfColumns);
            m_GameLogicManager.ResetGameLogic(newNumOfRows, newNumOfColumns);
            runGame();
        }
        /* ----------------------------------------------- */

        private bool restartGameIfNeeded()
        {
            bool restartRequested = false;
            Console.WriteLine("\n \n");
            string userInputForRestartRequest = Console.ReadLine();
            if (userInputForRestartRequest == k_RestartGame)
            {
                ClearScreen();
                restartRequested = true;
            }
            else
            {
                exitGame();
            }

            return restartRequested;
        }
        /* ----------------------------------------------- */

        private void displayWinnerMessage()
        {
            if (m_GameLogicManager.FirstPlayerScore == m_GameLogicManager.SecondPlayerScore)
            {
                Console.WriteLine("\n No Win Today. It's A Tie...\n");
            }
            else
            {
                string name = m_GameLogicManager.FirstPlayerScore > m_GameLogicManager.SecondPlayerScore
                                  ? m_GameLogicManager.GameDataManager.FirstPlayer.PlayerName
                                  : m_GameLogicManager.GameDataManager.SecondPlayer.PlayerName;
                Console.WriteLine($"\n Congratulations {name} You Are The Winner ! \n");
            }
        }
        /* ----------------------------------------------- */

        private void runGame()
        {
            while (m_GameLogicManager.GameDataManager.GameStatus == eGameStatus.CurrentlyRunning)
            {
                displayGameInterface();
                string playerInput = getPlayerNextMove();
                updateTurnAndView(playerInput.ToUpper());
            }
            displayWinnerMessage();
            if (restartGameIfNeeded())
            {
                RestartGame();
            }

        }
        /* ----------------------------------------------- */

        private void updateTurnAndView(string i_PlayerInput)
        {
            if (i_PlayerInput != k_QuitGame)
            {
            BoardTile selectedTile = m_GameLogicManager.GameDataManager.GameBoard.GetTile(i_PlayerInput);
            m_GameLogicManager.UpdateTurn(ref selectedTile);
            displayGameInterface();
            if (m_GameLogicManager.IsFirstSelection && !m_GameLogicManager.AreMatchingTiles)
            {
                Console.ForegroundColor = ConsoleColor.Red; // Change text color to green
                Console.WriteLine("No Match This Time. Don't give up !");
                Console.ResetColor();
                System.Threading.Thread.Sleep(2000);
                m_GameLogicManager.TogglePlayer();
            }
            m_GameLogicManager.AreMatchingTiles = false;
            }
            else
            {
                m_GameLogicManager.GameDataManager.GameStatus = eGameStatus.Over;
                exitGame();
            }
        }

        /* ----------------------------------------------- */
        private void exitGame()
        {
            Console.WriteLine("Thanks For Playing. Have A Nice Day!");
        }
        /* ----------------------------------------------- */

        private void runMenu()
        {
            string firstPlayerName, secondPlayerName;
            int columns, rows;

            eGameType gameType =
                r_GameMenu.RunMenuScreen(out firstPlayerName, out secondPlayerName, out rows, out columns);
            Player firstPlayer = new Player(firstPlayerName, ePlayerType.Human);

            ePlayerType secondPlayerType = gameType == eGameType.HumanVComputer ? ePlayerType.Computer :
                 ePlayerType.Human;

            Player secondPlayer = new Player(secondPlayerName, secondPlayerType);
            m_GameLogicManager = new GameLogicManager(firstPlayer, secondPlayer, rows, columns, gameType);
        }

        /* ----------------------------------------------- */

        //        private void displayGameInterface()
        //        {
        //            Ex02.ConsoleUtils.Screen.Clear();
        //            string scoreBoard = string.Format(@"
        //            SCORE BOARD
        //  ||============================||
        //  ||  {0,-8} | {1,-5}          ||
        //  ||----------------------------||
        //  ||  {2,-7} | {3,-5}          ||
        //  ||============================||
        //",
        //                m_GameLogicManager.GameDataManager.FirstPlayer.PlayerName,
        //                m_GameLogicManager.GameDataManager.FirstPlayer.PlayerPoints,
        //                m_GameLogicManager.GameDataManager.SecondPlayer.PlayerName,
        //                m_GameLogicManager.GameDataManager.SecondPlayer.PlayerPoints);
        //            Console.WriteLine(scoreBoard);
        //            Console.WriteLine("\n{0}'s Turn\n", m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerName);
        //            m_GameLogicManager.GameDataManager.GameBoard.DisplayBoard();
        //        }

        private void displayGameInterface()
        {
            Ex02.ConsoleUtils.Screen.Clear();

            string firstPlayerName = m_GameLogicManager.GameDataManager.FirstPlayer.PlayerName;
            string firstPlayerPoints = m_GameLogicManager.GameDataManager.FirstPlayer.PlayerPoints.ToString();
            string secondPlayerName = m_GameLogicManager.GameDataManager.SecondPlayer.PlayerName;
            string secondPlayerPoints = m_GameLogicManager.GameDataManager.SecondPlayer.PlayerPoints.ToString();

            int nameColumnWidth = Math.Max(firstPlayerName.Length, secondPlayerName.Length) + 2;
            int pointsColumnWidth = Math.Max(firstPlayerPoints.Length, secondPlayerPoints.Length) + 2;
            int totalWidth = nameColumnWidth + pointsColumnWidth + 7;

            string borderLine = new string('=', totalWidth);
            string separatorLine = new string('-', totalWidth);

            string scoreBoard = string.Format(@"
   SCORE BOARD:
  ||{0}||
  ||  {1} | {2}  ||
  ||{3}||
  ||  {4} | {5}  ||
  ||{6}||
",
                borderLine,
                firstPlayerName.PadRight(nameColumnWidth),
                firstPlayerPoints.PadRight(pointsColumnWidth),
                separatorLine,
                secondPlayerName.PadRight(nameColumnWidth),
                secondPlayerPoints.PadRight(pointsColumnWidth),
                borderLine
            );

            Console.WriteLine(scoreBoard);
            Console.WriteLine("\n{0}'s Turn\n", m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerName);
            m_GameLogicManager.GameDataManager.GameBoard.DisplayBoard();
        }


        /* ----------------------------------------------- */

        private string getPlayerNextMove()
        {
            string playerNextMove = "";

            if (m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerType == ePlayerType.Human)
            {
                playerNextMove = getInputFromHumanPlayer();
            }
            else
            {
                playerNextMove = m_GameLogicManager.GetAiNextMove();
                displayComputerMessage();
            }

            return playerNextMove;
        }
        /* ----------------------------------------------- */

        private void displayComputerMessage()
        {
            const string thinkingMessage = "The computer is deep in thought";
            const string matchMessage = "Eureka! The computer has found a match!";

            if (!m_GameLogicManager.ComputerHasMatch)
            {
                Console.Write(thinkingMessage);
                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write('.');
                }
                Console.WriteLine(); // Move to the next line after dots
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green; // Change text color to green
                Console.Write(matchMessage);
                System.Threading.Thread.Sleep(2000);
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        /* ----------------------------------------------- */

        private string getInputFromHumanPlayer()
        {
            string playerNextMove = "";

            bool isNextMoveIsValid = false;

            while (!isNextMoveIsValid)
            {
                Console.WriteLine("Now {0} has to choose next Move", m_GameLogicManager.GameDataManager.CurrentPlayer.PlayerName);
                playerNextMove = Console.ReadLine();
                isNextMoveIsValid = validateHumanPlayerNextMove(playerNextMove);
            }

            return playerNextMove;
        }
        /* ----------------------------------------------- */

        private bool validateHumanPlayerNextMove(string i_playerNextMove)
        {
            bool isNextMoveIsValid = true;

            if (i_playerNextMove != null)
            {
                i_playerNextMove = i_playerNextMove.ToUpper();

                if (i_playerNextMove != k_QuitGame)
                {

                    isNextMoveIsValid = validateTileHumanPlayerPicked(i_playerNextMove);

                    if (isNextMoveIsValid)
                    {
                        isNextMoveIsValid = validateTileIsNotHidden(i_playerNextMove);
                    }
                }

            }

            return isNextMoveIsValid;
        }
        /* ----------------------------------------------- */

        private bool validateTileIsNotHidden(string i_TileHumanPlayerPicked)
        {
            bool isHiddenTile = true;

            if (m_GameLogicManager.GameDataManager.GameBoard.GetTile(i_TileHumanPlayerPicked).IsRevealed)
            {
                Console.WriteLine("Wrong Input. You Picked A Tile That Is Already Revealed!\n");
                isHiddenTile = false;
            }
            return isHiddenTile;
        }
        /* ----------------------------------------------- */

        private bool validateTileHumanPlayerPicked(string i_TileHumanPlayerPicked)
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
                isValidTileChoice = validateColumnLetter(LetterColumns) && validateRowDigit(DigitRow);
            }

            return isValidTileChoice;
        }
        /* ----------------------------------------------- */

        private bool validateRowDigit(char i_ChosenRow)
        {
            bool isValidRowDigit = true;
            char largerstValidDigit = (char)('0' + m_GameLogicManager.GameDataManager.NumOfRows);

            if (i_ChosenRow < '1' || i_ChosenRow > largerstValidDigit)
            {
                Console.WriteLine("Wrong Input. Enter Row Between {0}-{1}", 1, m_GameLogicManager.GameDataManager.NumOfRows);
                isValidRowDigit = false;
            }

            return isValidRowDigit;
        }
        /* ----------------------------------------------- */

        private bool validateColumnLetter(char i_ChosenColumn)
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
        /* ----------------------------------------------- */

        public void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
