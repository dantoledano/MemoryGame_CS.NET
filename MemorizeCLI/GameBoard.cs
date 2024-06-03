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
            (m_NumOfLines, m_NumOfRows) = GetValidBoardSize();
            m_Board = new BoardTile[m_NumOfLines, m_NumOfRows];
            InitializeBoard();
        }
        
        private (int, int) GetValidBoardSize()
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

        private void InitializeBoard()
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
                    Console.Write(m_Board[i, j].m_IsRevealed ? $" {m_Board[i, j].m_Value} |" : "   |");
                }
                Console.WriteLine();
                PrintSeparator(m_NumOfRows);
            }
        }

        static public void PrintSeparator(int numOfRows)
        {
            Console.WriteLine("  " + new string('=', numOfRows * 4 + 1));
        }

        public bool RevealTile(int line, int row)
        {
            bool isSuccededToReveal = true;

            if (line < 0 || line >= m_NumOfLines || row < 0 || row >= m_NumOfRows || m_Board[line, row].m_IsRevealed)
            {
                isSuccededToReveal = false;
            }

            m_Board[line, row].m_IsRevealed = true;
            return isSuccededToReveal;
        }

        public void HideTile(int line, int row)
        {
            if (line >= 0 && line < m_NumOfLines && row >= 0 && row < m_NumOfRows)
            {
                m_Board[line, row].m_IsRevealed = false;
            }
        }

        public BoardTile GetTile(int line, int row)
        {
            return m_Board[line, row];
        }

        public bool AllTilesRevealed()
        {
            foreach (var tile in m_Board)
            {
                if (!tile.m_IsRevealed)
                    return false;
            }
            return true;
        }
    }
}
