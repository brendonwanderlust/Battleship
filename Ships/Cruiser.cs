using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    public class Cruiser : ShipBase
    {
        public Cruiser()
            : base(ShipType.Cruiser, 3)
        {
        }
    }
}
