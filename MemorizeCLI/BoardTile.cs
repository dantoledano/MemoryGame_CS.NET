using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class BoardTile
    {
        private char m_Value;
        private bool m_IsRevealed;
        private int m_ColumnIndexInBoard;
        private int m_RowIndexInBoard;

        public char Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public bool IsRevealed
        {
            get { return m_IsRevealed; }
            set { m_IsRevealed = value; }
        }

        public int ColumnIndexInBoard
        {
            get { return m_ColumnIndexInBoard; }
            set { m_ColumnIndexInBoard = value; }
        }

        public int RowIndexInBoard
        {
            get { return m_RowIndexInBoard; }
            set { m_RowIndexInBoard = value; }
        }


        public BoardTile(char i_Value, int i_Row, int i_Column)
        {
            m_Value = i_Value;
            m_IsRevealed = false;
            m_ColumnIndexInBoard = i_Column;
            m_RowIndexInBoard = i_Row;
        }

        public BoardTile(int i_Row, int i_Column)
        {
            m_RowIndexInBoard = i_Row;
            m_ColumnIndexInBoard = i_Column;
        }

        public string ParseToString()
        {
            return string.Format("{0}{1}", (char)(m_ColumnIndexInBoard + 'A'), (char)(m_RowIndexInBoard + '1'));
        }
    }
}
