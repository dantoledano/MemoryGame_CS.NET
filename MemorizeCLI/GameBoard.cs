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
        readonly BoardTile[,] m_Board;

        public BoardTile GetBoardTile(int row, int column)
        {
            return m_Board[row, column];
        }

        public GameBoard(int i_NumOfRows, int i_NumOfColumns)
        {
            m_NumOfColumns = i_NumOfColumns;
            m_NumOfRows = i_NumOfRows;
            InitializeBoard(m_NumOfRows,m_NumOfColumns ,ref m_Board);
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

        public void InitializeBoard(int i_NumOfRows, int i_NumOfColumns, ref BoardTile[,] m_Board)
        {
            m_Board = new BoardTile[i_NumOfRows, i_NumOfColumns];
            int shuffeledArrayIndexCounter = 0;
            char[] letters = new char[i_NumOfColumns * i_NumOfRows / 2];
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

            for (int i = 0; i < i_NumOfRows; ++i)
            {
                for(int j = 0; j < i_NumOfColumns; ++j)
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
            {
                Console.Write($"  {(char)('A' + j)} ");
            }
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

        public void PrintSeparator(int i_NumOfColumns)
        {
            Console.WriteLine($"  {new string('=', i_NumOfColumns * 4 + 1)}");
        }

        public bool RevealTile(int i_Row, int i_Column)
        {
            bool isSuccededToReveal = true;

            if (i_Row < 0 || i_Row >= m_NumOfColumns || i_Column < 0 || i_Column >= m_NumOfRows || m_Board[i_Row, i_Column].IsRevealed)
            {
                isSuccededToReveal = false;
            }

            m_Board[i_Row, i_Column].IsRevealed = true;
            return isSuccededToReveal;
        }

        public void HideTile(int i_Column, int i_Row)
        {
            if (i_Row >= 0 && i_Row < m_NumOfColumns && i_Column >= 0 && i_Column < m_NumOfRows)
            {
                m_Board[i_Row, i_Column].IsRevealed = false;
            }
        }

        public BoardTile GetTile(string i_TileToIndexInMatrix)
        {
            int row = i_TileToIndexInMatrix[1] - '1';
            int column = i_TileToIndexInMatrix[0] - 'A';

            return m_Board[row, column];
        }

        public string ConvertToStringCell(int i_Row, int i_Column)
        {
            char columnLetter = (char)('A' + i_Column);
            int rowNumber = i_Row + 1;
            return columnLetter.ToString() + rowNumber.ToString();
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
