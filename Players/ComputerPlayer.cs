using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Players;


namespace Battleship
{
    public class ComputerPlayer : PlayerBase
    {
        public ComputerPlayer()
        {
            Name = "The Juggernaut";
            Type = PlayerType.Computer;
        }
    }
}
