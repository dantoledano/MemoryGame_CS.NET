using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class GameBoard
    {
        private readonly int r_NumOfColumns;
        private readonly int r_NumOfRows;
        private readonly BoardTile[,] r_Board;

        public BoardTile GetBoardTile(int i_Row, int i_Column)
        {
            return r_Board[i_Row, i_Column];
        }
        /* ----------------------------------------------- */

        public GameBoard(int i_NumOfRows, int i_NumOfColumns)
        {
            r_NumOfColumns = i_NumOfColumns;
            r_NumOfRows = i_NumOfRows;
            InitializeBoard(r_NumOfRows, r_NumOfColumns, ref r_Board);
        }
        /* ----------------------------------------------- */

        public void InitializeBoard(int i_NumOfRows, int i_NumOfColumns, ref BoardTile[,] o_Board)
        {
            o_Board = new BoardTile[i_NumOfRows, i_NumOfColumns];
            int shuffledArrayIndexCounter = 0;
            char[] letters = new char[i_NumOfColumns * i_NumOfRows / 2];
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = (char)('A' + i);
            }

            char[] shuffledBoard = new char[letters.Length * 2];
            Array.Copy(letters, 0, shuffledBoard, 0, letters.Length);
            Array.Copy(letters, 0, shuffledBoard, letters.Length, letters.Length);

            // Shuffle the board
            Random rand = new Random();
            for (int i = shuffledBoard.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                char temp = shuffledBoard[i];
                shuffledBoard[i] = shuffledBoard[j];
                shuffledBoard[j] = temp;
            }

            for (int i = 0; i < i_NumOfRows; ++i)
            {
                for (int j = 0; j < i_NumOfColumns; ++j)
                {
                    o_Board[i, j] = new BoardTile(shuffledBoard[shuffledArrayIndexCounter], i, j);
                    shuffledArrayIndexCounter++;
                }
            }

        }

        /* ----------------------------------------------- */

        public void DisplayBoard()
        {
            Console.Write("  ");
            for (int j = 0; j < r_NumOfColumns; j++)
            {
                Console.Write($"  {(char)('A' + j)} ");
            }

            Console.WriteLine();
            PrintSeparator(r_NumOfColumns);
            for (int i = 0; i < r_NumOfRows; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < r_NumOfColumns; j++)
                {
                    if(!r_Board[i, j].IsRevealed)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" " + r_Board[i, j].Value); 
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }

                Console.WriteLine();
                PrintSeparator(r_NumOfColumns);
            }
        }
        /* ----------------------------------------------- */

        public void PrintSeparator(int i_NumOfColumns)
        {
            Console.WriteLine($"  {new string('=', i_NumOfColumns * 4 + 1)}");
        }
        /* ----------------------------------------------- */

        public BoardTile GetTile(string i_TileToIndexInMatrix)
        {
            int row = i_TileToIndexInMatrix[1] - '1';
            int column = i_TileToIndexInMatrix[0] - 'A';

            return r_Board[row, column];
        }
        /* ----------------------------------------------- */

        public string ConvertTileToString(int i_Row, int i_Column)
        {
            char columnLetter = (char)('A' + i_Column);
            int rowNumber = i_Row + 1;
            return columnLetter.ToString() + rowNumber.ToString();
        }
    }
}

