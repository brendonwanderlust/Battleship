using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***Welcome to Battleship!***");

            GameLogic newGame = new GameLogic();
            newGame.SinglePlayerGame();


        }

        
    }
}
