using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MemorizeCLI
{
    internal class Program
    {

        public static void Main(string[] i_Args)
        {
            GameInterface g1 = new GameInterface();
            g1.startGame();
        

        }

    }
}


/*TODO
 * Change game mode to running after menu - suppose to be at the constructor of the GameLogicManager
 * drawData() = displayGameInterface()
 * take player input - function
 * validate human input
 *
 *
 */