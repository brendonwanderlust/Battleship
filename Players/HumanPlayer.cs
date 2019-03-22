using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Ships;
using Battleship.Players;

namespace Battleship
{
    public class HumanPlayer : PlayerBase 
    {
        public HumanPlayer()
        {
            Type = PlayerType.Human;
        } 
    }
}
