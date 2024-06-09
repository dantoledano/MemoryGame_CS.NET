using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameMenu
    {
        public eGameType RunMenuScreen(out string o_FirstPlayerName, out string o_SecondPlayerName,out int o_NumOfRows, out int o_NumOfColumns)
        {
            o_NumOfColumns = 4;
            o_NumOfRows = 4;
            Console.WriteLine("Welcome To The Memory Game!");
            Console.WriteLine("Please Enter First Player Name: ");
            o_FirstPlayerName = Console.ReadLine();
            Console.WriteLine($"Hello {o_FirstPlayerName}");
            Console.WriteLine("Choose Your Preferred Game Type:");
            eGameType gameType = getAndValidateGameType(out o_SecondPlayerName);
            GetAndValidateMatrixDimensions(out o_NumOfRows, out o_NumOfColumns);
            Ex02.ConsoleUtils.Screen.Clear();
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

        public bool GetAndValidateMatrixDimensions(out int o_NumOfRows, out int o_NumOfColumns)
        {
            bool isValidMatrixDimensions = false;
            o_NumOfColumns = 4;
            o_NumOfRows = 4;

            while (!isValidMatrixDimensions)
            {
                Console.WriteLine("Please Enter Number Of Rows:");
                o_NumOfRows = GetSizeWithinRange(GameLogicManager.MinMatrixRows, GameLogicManager.MaxMatrixRows);
                Console.WriteLine("Please Enter Number Of Columns:");
                o_NumOfColumns = GetSizeWithinRange(GameLogicManager.MinMatrixColumns, GameLogicManager.MaxMatrixColumns);

                if ((o_NumOfColumns * o_NumOfRows) % 2 == 0)
                {
                    isValidMatrixDimensions = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, enter even sized matrix.\n");
                }
            }

            return isValidMatrixDimensions;
        }

        private eGameType getAndValidateGameType(out string o_SecondPlayerName)
        {
            eGameType gameType = eGameType.HumanVComputer;
            Console.WriteLine("1) Human Vs Human");
            Console.WriteLine("2) Human Vs Computer");
            string userChoiceForGameType = validateGameType();
            if (userChoiceForGameType == "1")
            {
                Console.WriteLine("Please Enter Second Player Name:");
                o_SecondPlayerName = Console.ReadLine();
                gameType = eGameType.HumanVHuman;
            }
            else
            {
                o_SecondPlayerName = "Computer";
                gameType = eGameType.HumanVComputer;
            }

            return gameType;
        }



        private string validateGameType()
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
