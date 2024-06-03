using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class BoardTile
    {
        char m_Value { get; set; } 
        bool m_IsRevealed { get; set; }
        int m_ColumnIndexInBoard { get; set; }
        int m_RowIndexInBoard { get; set; }

        public char Value
        {
            get { return m_Value; }
            set{ m_Value = value; }
        }

        public bool IsRevealed
        {
            get{ return m_IsRevealed; }
            set{ m_IsRevealed = value; }
        }

        public int  ColumnIndexInBoard
        {
            get { return m_ColumnIndexInBoard; }
            set { m_ColumnIndexInBoard = value; }
        }

        public int RowIndexInBoard
        {
            get { return m_RowIndexInBoard; }
            set { m_RowIndexInBoard = value; }
        }


        public BoardTile(char i_Value, int i_Column, int i_Row)
        {
            m_Value = i_Value;
            m_IsRevealed = true;
            m_ColumnIndexInBoard = i_Column;
            m_RowIndexInBoard = i_Row;
        }
    }
}
