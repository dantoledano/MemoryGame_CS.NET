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
        readonly int m_NumOfColumns;
        readonly int m_NumOfRows;
        static BoardTile[,] m_Board;

        public GameBoard(int i_NumOfColumns,int i_NumOfRows)
        {
            m_NumOfColumns = i_NumOfColumns;
            m_NumOfRows = i_NumOfRows;
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

        public void initializeBoard()
        {
            int shuffeledArrayIndexCounter = 0;
            char[] letters = new char[m_NumOfColumns * m_NumOfRows / 2];
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = (char)('A' + i);
            }

            // Concatenate the array with itself
            char[] shuffeledBoard = new char[letters.Length * 2];
            Array.Copy(letters, 0, shuffeledBoard, 0, letters.Length);
            Array.Copy(letters, 0, shuffeledBoard, letters.Length, letters.Length);

            // Shuffle the board
            Random rand = new Random();
            for (int i = shuffeledBoard.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                char temp = shuffeledBoard[i];
                shuffeledBoard[i] = shuffeledBoard[j];
                shuffeledBoard[j] = temp;
            }

            for (int i = 0; i < m_NumOfColumns; ++i)
            {
                for (int j = 0; j < m_NumOfRows; ++j)
                {
                    m_Board[i, j] = new BoardTile(shuffeledBoard[shuffeledArrayIndexCounter], i, j);
                    shuffeledArrayIndexCounter++;
                }
            }
        }


        public void DisplayBoard()
        {
            Console.Write("  ");
            for (int j = 0; j < m_NumOfColumns; j++)
                Console.Write($"  {(char)('A' + j)} ");
            Console.WriteLine();
            PrintSeparator(m_NumOfColumns);

            for (int i = 0; i < m_NumOfRows; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < m_NumOfColumns; j++)
                {
                    // Printing empty spaces in the board cells
                    Console.Write(m_Board[i, j].IsRevealed ? $" {m_Board[i, j].Value} |" : "   |");
                }
                Console.WriteLine();
                PrintSeparator(m_NumOfColumns);
            }
        }

        public void PrintSeparator(int m_NumOfRows)
        {
            Console.WriteLine($"  {new string('=', m_NumOfRows * 4 + 1)}");
        }

        public bool RevealTile(int i_Line, int i_Row)
        {
            bool isSuccededToReveal = true;

            if (i_Line < 0 || i_Line >= m_NumOfColumns || i_Row < 0 || i_Row >= m_NumOfRows || m_Board[i_Line, i_Row].IsRevealed)
            {
                isSuccededToReveal = false;
            }

            m_Board[i_Line, i_Row].IsRevealed = true;
            return isSuccededToReveal;
        }

        public void HideTile(int i_Line, int i_Row)
        {
            if (i_Line >= 0 && i_Line < m_NumOfColumns && i_Row >= 0 && i_Row < m_NumOfRows)
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
