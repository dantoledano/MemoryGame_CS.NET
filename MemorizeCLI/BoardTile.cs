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
        int m_LineIndexInBoard { get; set; }
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

        public int  LineIndexInBoard
        {
            get { return m_LineIndexInBoard; }
            set { m_LineIndexInBoard = value; }
        }

        public int RowIndexInBoard
        {
            get { return m_RowIndexInBoard; }
            set { m_RowIndexInBoard = value; }
        }


        public BoardTile(char i_Value, int i_Line, int i_Row)
        {
            m_Value = i_Value;
            m_IsRevealed = false;
            m_LineIndexInBoard = i_Line;
            m_RowIndexInBoard = i_Row;
        }
    }
}
