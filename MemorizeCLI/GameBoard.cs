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
        readonly int m_NumOfLines;
        readonly int m_NumOfRows;
        static BoardTile[,] m_Board;

        public GameBoard()
        {
            (m_NumOfLines, m_NumOfRows) = getValidBoardSize();
            m_Board = new BoardTile[m_NumOfLines, m_NumOfRows];
            initializeBoard();
        }
        
        private (int, int) getValidBoardSize()
        {
            int numberOfLinesInBoard, numberOfRowsInBoard;
            do
            {
                Console.WriteLine("Enter number of lines");
                numberOfLinesInBoard = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter number of rows");
                numberOfRowsInBoard = int.Parse(Console.ReadLine());

                if (numberOfLinesInBoard % 2 == 0 && numberOfRowsInBoard % 2 == 0)
                {
                    break;
                }

                Console.WriteLine("Invalid board size. The board size must be even and between 4x4 and 6x6.");
            } while (true);

            return (numberOfLinesInBoard, numberOfRowsInBoard);
        }

        private void initializeBoard()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new Random();

            // Place pairs of letters on the board
            for (int letterIndex = 0; letterIndex < letters.Length; letterIndex++)
            {
                char currentLetter = letters[letterIndex];
                for (int pair = 0; pair < 2; pair++)
                {
                    // Find a random empty cell for the current letter
                    int row, col;
                    do
                    {
                        row = rand.Next(m_NumOfLines);
                        col = rand.Next(m_NumOfRows);
                    } while (m_Board[row, col] != null);

                    // Place the letter at the selected cell
                    m_Board[row, col] = new BoardTile(currentLetter, row, col);
                }
            }
        }

        public void DisplayBoard()
        {
            Console.Write("  ");
            for (int j = 0; j < m_NumOfRows; j++)
                Console.Write($"  {(char)('A' + j)}");
            Console.WriteLine();
            PrintSeparator(m_NumOfRows);

            for (int i = 0; i < m_NumOfLines; i++)
            {
                Console.Write($"  {i + 1} |");
                for (int j = 0; j < m_NumOfRows; j++)
                {
                    // Printing empty spaces in the board cells that unrecovered
                    Console.Write(m_Board[i, j].IsRevealed ? $" {m_Board[i, j].Value} |" : "   |");
                }
                Console.WriteLine();
                PrintSeparator(m_NumOfRows);
            }
        }

        static public void PrintSeparator(int i_NumOfRows)
        {
            Console.WriteLine("  " + new string('=', i_NumOfRows * 4 + 1));
        }

        public bool RevealTile(int i_Line, int i_Row)
        {
            bool isSuccededToReveal = true;

            if (i_Line < 0 || i_Line >= m_NumOfLines || i_Row < 0 || i_Row >= m_NumOfRows || m_Board[i_Line, i_Row].IsRevealed)
            {
                isSuccededToReveal = false;
            }

            m_Board[i_Line, i_Row].IsRevealed = true;
            return isSuccededToReveal;
        }

        public void HideTile(int i_Line, int i_Row)
        {
            if (i_Line >= 0 && i_Line < m_NumOfLines && i_Row >= 0 && i_Row < m_NumOfRows)
            {
                m_Board[i_Line, i_Row].IsRevealed = false;
            }
        }

        public BoardTile GetTile(int i_Line, int i_Row)
        {
            return m_Board[i_Line, i_Row];
        }

        //public bool AllTilesRevealed()
        //{
        //    foreach (var tile in m_Board)
        //    {
        //        if (!tile.m_IsRevealed)
        //            return false;
        //    }
        //    return true;
        //}
    }
}
