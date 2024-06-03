using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizeCLI
{
    internal class Player
    {
        private int m_PlayerPoints; 
        private ePlayerType m_PlayerType;
        private string m_PlayerName;


        public Player(int i_PlayerPoints)
        {
            m_PlayerPoints = i_PlayerPoints;
        }

        public Player(string i_playerName, ePlayerType i_playerType)
        {
            m_PlayerName = i_playerName;
            m_PlayerType = i_playerType;
            m_PlayerPoints = 0;
        }

        public ePlayerType PlayerType
        {
            get
            {
                return m_PlayerType; 
            }

            set
            {
                m_PlayerType = value;
            }

        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
            set
            {
                m_PlayerName = value;
            }
        }

        public int PlayerPoints
        {
            get
            {
                return m_PlayerPoints;
            }
            set
            {
                m_PlayerPoints = value;
            }
        }

        
    }
}
