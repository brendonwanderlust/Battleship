using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    public class Carrier : ShipBase
    {
        public Carrier()
             : base(ShipType.Carrier, 4)
        {
        }
    }
}
