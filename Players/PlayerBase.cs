using Battleship.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Players;

namespace Battleship
{
    public abstract class PlayerBase
    {
        public string Name { get; set; }

        public PlayerType Type { get; set; }

        public bool IsTurn { get; set; }

        public List<ShipBase> FloatingShipList = new List<ShipBase>();

        public List<ShipBase> SunkShipList = new List<ShipBase>();

        public List<string> MissedShotCoordinates = new List<string>();      
       
    }

}
