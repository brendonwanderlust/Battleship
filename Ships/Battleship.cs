using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    public class Battleship : ShipBase
    {
        public Battleship()
            : base(ShipType.Battleship, 4)
        {
        }
    }
}
