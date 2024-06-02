using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class BoardTile
    {
        public char m_Value { get; set; }
        public bool m_IsRevealed { get; set; }
        public int m_LineIndexInBoard { get; set; }
        public int m_RowIndexInBoard { get; set; }

        public BoardTile(char value, int line, int row)
        {
            m_Value = value;
            m_IsRevealed = false;
            m_LineIndexInBoard = line;
            m_RowIndexInBoard = row;
        }
    }
}
