using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    public class Destroyer : ShipBase
    {
        public Destroyer()
             : base(ShipType.Destroyer, 2)
        {
        }
    }
}
