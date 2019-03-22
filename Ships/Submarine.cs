using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    public class Submarine : ShipBase
    {
        public Submarine()
            : base(ShipType.Submarine, 3)
        {
        }
    }
}
