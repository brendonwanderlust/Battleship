using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public abstract class ShipBase
    {
        protected ShipBase(ShipType shipType, int size)
        {
            Type = shipType;
            Size = size;
            IsSunk = false;
        }

        public bool IsSunk { get; set; }
        public ShipType Type { get; private set; }
        public int Size { get; private set; }
        public Horizontal HorizontalStart { get; set; }
        public Vertical VerticalStart { get; set; }
        public Horizontal HorizontalEnd { get; set; }
        public Vertical VerticalEnd { get; set; }
        public Ships.Direction ShipDirection { get; set; }
        public List<string> Coordinates = new List<string>();
        public List<string> HitCoordinates = new List<string>();

        public bool IsSelectionOnGameboard()
        {           
            if (!Enum.IsDefined(typeof(Horizontal), HorizontalEnd) || !Enum.IsDefined(typeof(Vertical), VerticalEnd))
            {                       
                return false;
            }
            return true;
        }
     
        public void SetHorizontalEnd(Horizontal horizontalStart, Ships.Direction shipDirection)
        {           
            if (shipDirection == Ships.Direction.up)
            {
                HorizontalEnd = horizontalStart;
                //Console.WriteLine($"Horizontal Start Position: {horizontalStart}. Horizontal End Position: {HorizontalEnd}.");
            }
            else if (shipDirection == Ships.Direction.down)
            {
                HorizontalEnd = horizontalStart;
                //Console.WriteLine($"Horizontal Start Position: {horizontalStart}. Horizontal End Position: {HorizontalEnd}.");
            }
            else if (shipDirection == Ships.Direction.left)
            {                
                HorizontalEnd = horizontalStart - (Size - 1);
                //Console.WriteLine($"Horizontal Start Position: {horizontalStart}. Horizontal End Position: {HorizontalEnd}.");
            }
            else if (shipDirection == Ships.Direction.right)
            {                
                HorizontalEnd = horizontalStart + (Size - 1);
                //Console.WriteLine($"Horizontal Start Position: {horizontalStart}. Horizontal End Position: {HorizontalEnd}.");
            }           
        }

        public void SetVerticalEnd(Vertical verticalStart, Ships.Direction shipDirection)
        {
            if (shipDirection == Ships.Direction.up)
            {               
                VerticalEnd = verticalStart - (Size - 1);
                //Console.WriteLine($"Vertical Start Position: {verticalStart}. Vertical End Position: {VerticalEnd}.");
            }
            else if (shipDirection == Ships.Direction.down)
            {               
                VerticalEnd = verticalStart + (Size - 1);
                //Console.WriteLine($"Vertical Start Position: {verticalStart}. Vertical End Position: {VerticalEnd}.");
            }
            else if (shipDirection == Ships.Direction.left)
            {
                VerticalEnd = verticalStart;
                //Console.WriteLine($"Vertical Start Position: {verticalStart}. Vertical End Position: {VerticalEnd}.");
            }
            else if (shipDirection == Ships.Direction.right)
            {
                VerticalEnd = verticalStart;
                //Console.WriteLine($"Vertical Start Position: {verticalStart}. Vertical End Position: {VerticalEnd}.");
            }            
        }

        public void SetInitialShipCoordinate(Horizontal horizontalStart,Vertical verticalStart)
        {
            string bowCoordinates = HorizontalStart.ToString().ToUpper() + VerticalStart.ToString().ToUpper();
            Coordinates.Add(bowCoordinates);
        }

        public void SetRemainingShipCoordinates(Ships.Direction shipDirection)
        {
            List<string> shipCoordinatesToSet = new List<string>();            
            string sternCoordinates = HorizontalEnd.ToString().ToUpper() + VerticalEnd.ToString().ToUpper();            
            shipCoordinatesToSet.Add(sternCoordinates);

            if (Type == ShipType.Destroyer)
            {
                Coordinates.AddRange(shipCoordinatesToSet);
                return;
            }

            if (shipDirection == Ships.Direction.up)
            {
                string horizontal = HorizontalStart.ToString().ToUpper();                
                for (var i = 1; i < (Size - 1); i++) //Size - 1 because we already have the vertical points at the bow and stern but we start at 1
                {                  
                    var verticalPosition = VerticalStart - i;
                    string verticalPositionString = verticalPosition.ToString().ToUpper();
                    shipCoordinatesToSet.Add(horizontal + verticalPositionString);                    
                }                               
            }
            else if (shipDirection == Ships.Direction.down)
            {
                string horizontal = HorizontalStart.ToString().ToUpper();
                for (var i = 1; i < (Size - 1); i++) 
                {
                    var verticalPosition = VerticalStart + i;
                    string verticalPositionString = verticalPosition.ToString().ToUpper();
                    shipCoordinatesToSet.Add(horizontal + verticalPositionString);
                }
            }
            else if (shipDirection == Ships.Direction.left)
            {
                string vertical = VerticalStart.ToString().ToUpper();
                for (var i = 1; i < (Size - 1); i++) 
                {
                    var horizontalPosition = HorizontalStart - i;
                    string hortizontalPositionString = horizontalPosition.ToString().ToUpper();
                    shipCoordinatesToSet.Add(hortizontalPositionString + vertical);
                }
            }
            else if (shipDirection == Ships.Direction.right)
            {
                string vertical = VerticalStart.ToString().ToUpper();
                for (var i = 1; i < (Size - 1); i++) 
                {
                    var horizontalPosition = HorizontalStart + i;
                    string hortizontalPositionString = horizontalPosition.ToString().ToUpper();
                    shipCoordinatesToSet.Add(hortizontalPositionString + vertical);
                }
            }

            Coordinates.AddRange(shipCoordinatesToSet);
                      
        }

        public void SetShipStatus()
        {
            if (HitCoordinates.Count == Coordinates.Count)
            {
                IsSunk = true;
                //Console.WriteLine($"Your {Type} is sunk.");
            }

        }

        public string ReturnShipCoordinates()
        {
            var coordinates = "";
            for (var i = 0; i < Coordinates.Count; i++)
            {
                coordinates += UI.FormatCoordinateForUI(Coordinates[i]) +  " ";
            }
            return coordinates;
        }
    }

}
