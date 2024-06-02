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
            g1.GetUserInput();
            g1.DisplayGameSettings();
        }

    }
}
