using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameMenu
    {

        public eGameType RunMenuScreen(out string o_FirstPlayerName, out string o_secondPlayerName,out int o_numOfRows, out int o_numOfColumns)
        {
            o_numOfColumns = 4;
            o_numOfRows = 4;
            Console.WriteLine("Welcome To The Memory Game!");
            Console.WriteLine("Please Enter First Player Name: ");
            o_FirstPlayerName = Console.ReadLine();
            Console.WriteLine($"Hello {o_FirstPlayerName}");
            Console.WriteLine("Choose Your Prefered Game Type:");
            eGameType gameType = GetAndValidateGameType(out o_secondPlayerName);
            GetAndValidateMatrixDimenssions(out o_numOfRows, out o_numOfColumns);
            return gameType;
        }

        public int GetSizeWithinRange(int i_MinSize, int i_MaxSize)
        {
            int userSizeChoice = i_MinSize;
            bool isInputANumber = false;
            bool isInputWithinRange = false;

            while (!isInputWithinRange || !isInputANumber)
            {
                Console.WriteLine($"Enter A Value Between {i_MinSize} - {i_MaxSize}: ");
                isInputANumber = int.TryParse(Console.ReadLine(), out userSizeChoice);

                if (isInputANumber)
                {
                    if (userSizeChoice >= i_MinSize && userSizeChoice <= i_MaxSize)
                    {
                        isInputWithinRange = true;
                    }
                }

                if (!isInputWithinRange || !isInputANumber)
                {
                    Console.WriteLine("Try Again.\n");
                }

            }

            return userSizeChoice;

        }

        private bool GetAndValidateMatrixDimenssions(out int o_numOfRows, out int o_numOfColumns )
        {
            bool isValidMatrixDimenssions = false;
            o_numOfColumns = 4;
            o_numOfRows = 4;

            while (!isValidMatrixDimenssions)
            {
                Console.WriteLine("Please Enter Number Of Rows:");
                o_numOfRows = GetSizeWithinRange(GameLogicManager.MinMatrixRows, GameLogicManager.MaxMatrixRows);
                Console.WriteLine("Please Enter Number Of Columns:");
                o_numOfColumns =
                    GetSizeWithinRange(GameLogicManager.MinMatrixColumns, GameLogicManager.MaxMatrixColumns);


                if ((o_numOfColumns * o_numOfRows) % 2 == 0)
                {
                    isValidMatrixDimenssions = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, enter even sized matrix.\n");
                }

            }
            return isValidMatrixDimenssions;
        }

        private eGameType GetAndValidateGameType(out string o_secondPlayerName)
        {
            eGameType gameType = eGameType.HumanVComputer;
            o_secondPlayerName = "0";
            Console.WriteLine("1) Human Vs Human");
            Console.WriteLine("2) Human Vs Computer");
            string userChoiceForGameType = ValidateGameType();
            if (userChoiceForGameType == "1")
            {
                Console.WriteLine("Please Enter Second Player Name:");
                o_secondPlayerName = Console.ReadLine();
                gameType = eGameType.HumanVHuman;
            }

            return gameType;
        }



        private string ValidateGameType()
        {
            string selectedGameType = Console.ReadLine();

            while (selectedGameType != "1" && selectedGameType != "2")
            {
                Console.WriteLine("Invalid Input, Choose 1 OR 2\n");
                selectedGameType = Console.ReadLine();
            }

            return selectedGameType;
        }
    }
}
