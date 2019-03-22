using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Players;
using Battleship.Ships;
using Battleship.Game;

namespace Battleship
{
    public static class UI
    {
        public static void InvalidCoordinateMessage()
        {
            Console.WriteLine("Please enter a valid coordinate.");
            Console.WriteLine($"     Acceptable horizontal positions include: A, B, C, D, E, F, G, H, I, J");
            Console.WriteLine($"     Acceptable vertical positions include: 1, 2 , 3, 4, 5, 6, 7, 8, 9, 10");
            Console.WriteLine("");
        }

        public static void InvalidVerticalPositionMessage()
        {
            Console.WriteLine("You must select a valid vertical position! Please try again.");
            Console.WriteLine("");
        }

        public static void NeedValidVerticalandHorizontalMessage()
        {
            Console.WriteLine("You need both a valid vertical and horizontal start position! Please try again.");
            Console.WriteLine($"     Acceptable horizontal positions include: A, B, C, D, E, F, G, H, I, J");
            Console.WriteLine($"     Acceptable vertical positions include: 1, 2 , 3, 4, 5, 6, 7, 8, 9, 10");
            Console.WriteLine("");
        }

        public static void EmptySelectionMessage()
        {
            Console.WriteLine("Your selection cannont be empty! Please try again.");
            Console.WriteLine($"     Acceptable horizontal positions include: A, B, C, D, E, F, G, H, I, J");
            Console.WriteLine($"     Acceptable vertical positions include: 1, 2 , 3, 4, 5, 6, 7, 8, 9, 10");
            Console.WriteLine("");
        }

        public static void AskForShotCoordinate()
        {
            Console.WriteLine("What coordinate would you like to fire at? (Choose from: Horizontal A - J and Vertical 1 - 10. For Example: A10.)");
            Console.WriteLine("");
        }

        public static void DisplayShipsAndShipLocations(List<ShipBase> shipList)
        {
            Console.WriteLine("Your ships and their locations are:");
            foreach (ShipBase ship in shipList)
            {
                Console.WriteLine($"   {ship.Type}: Located at coordinates: {ReturnShipCoordinates(ship)}");
            }
            Console.WriteLine("");
        }

        public static void DisplayAllMissedCoordinates(List<string> missFireCoordinatesList)
        {            
            var missedCoordinates = "";
            foreach (string coordinate in missFireCoordinatesList)
            {
                missedCoordinates += FormatCoordinateForUI(coordinate) + " ";                
            }

            if (string.IsNullOrWhiteSpace(missedCoordinates))
            {
                return;
            }
            else
            {
                Console.WriteLine("Coordinates you've fired at and MISSED:");
                Console.WriteLine($"{missedCoordinates}");
                Console.WriteLine("");
            }            
        }

        public static void DisplayDirectHits(PlayerBase opponent)
        {
            var hitCoordinates = "";
            foreach (ShipBase ship in opponent.FloatingShipList)
            {
                foreach (string coordinate in ship.HitCoordinates)
                {
                    hitCoordinates += FormatCoordinateForUI(coordinate) + " ";
                }
            }
            if (string.IsNullOrWhiteSpace(hitCoordinates))
            {
                return;
            }
            else
            {

                Console.WriteLine($"Coordinates you've fired at and HIT:");
                Console.WriteLine($"{hitCoordinates}");
                Console.WriteLine("");
            }            
        }

        public static string FormatCoordinateForUI(string coordinate)
        {
            var formattedCoordinate = "";

            var horizontalString = coordinate.Substring(0, 1).ToUpper();
            var verticalString = coordinate.Substring(1).ToLower();
            Enum.TryParse(verticalString, out Vertical verticalEnum);
            var verticalStartInt = Convert.ToInt32(verticalEnum);
            formattedCoordinate = horizontalString + verticalStartInt.ToString();

            return formattedCoordinate;
        }

        public static void DisplayOpponentsShipLocations(PlayerBase opponent)
        {
            Console.WriteLine($"You've sunk ({opponent.SunkShipList.Count}) of your opponents ships:");
            foreach (ShipBase ship in opponent.SunkShipList)
            {
                Console.WriteLine($"     {ship.Type}");
            }
            Console.WriteLine();
        }

        public static void DisplayOpponentsSunkShipNames(PlayerBase opponent)
        {
            if (opponent.SunkShipList.Count == 0)
            {
                return;
            }
            else
            {
                Console.WriteLine($"You've sunk ({opponent.SunkShipList.Count}) of your opponents ships:");            
                Console.WriteLine();
            }
            
        }

        

        public static void GetHumanPlayerName(PlayerBase player)
        {
            bool isAcceptable = false;
            
            while (!isAcceptable)
            {

                Console.WriteLine("What's your name?");

                string responseString = Console.ReadLine();
                
                if (responseString.Length > 15)
                {
                    Console.WriteLine("This is America dude. There is no way your name is that long.");
                    continue;
                }
                if (responseString.All(char.IsDigit))
                {
                    Console.WriteLine("Enough with the games. Your name is not a number.");
                    continue;
                }
                if (responseString.Any(char.IsDigit))
                {
                    Console.WriteLine("Bro, your name does not have a digit in it.");
                    continue;
                }
                if (String.IsNullOrWhiteSpace(responseString))
                {
                    Console.WriteLine("Really??");
                    continue;
                }
                else
                {
                    player.Name = responseString;
                    isAcceptable = true;
                }

            }
            
            
        }

        public static int SelectHowtoChoosePieceLocations()
        {
            while (true)
            {
                Console.WriteLine("Do you want to Randomize your ship locations or choose them yourself? (Randomize = 1 and Choose = 2)");
                var selction = Console.ReadLine().ToUpper();
                if (string.IsNullOrEmpty(selction) || string.IsNullOrWhiteSpace(selction))
                {
                    Console.WriteLine("Please enter a valid option");
                    Console.WriteLine("");
                    continue;
                }
                if (!int.TryParse(selction, out int selectionInt))
                {
                    Console.WriteLine("Please enter a number.");
                    Console.WriteLine("");
                    continue;
                }
                if (selectionInt != 1 && selectionInt != 2)
                {
                    Console.WriteLine("Only enter a 1 or 2. ");
                    Console.WriteLine("");
                    continue;
                }
                else return selectionInt; 

            }
        }

        public static void GameCountdown()
        {
            
            Console.WriteLine("Your game will start in ");
            for (int a = 10; a >= 0; a--)
            {
                
                Console.WriteLine($"{a}");   
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Let's play Battleship!");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }

        public static string ReturnShipCoordinates(ShipBase ship)
        {
            var coordinates = "";
            for (var i = 0; i < ship.Coordinates.Count; i++)
            {
                coordinates += FormatCoordinateForUI(ship.Coordinates[i]) + " ";
            }
            return coordinates;
        }
        public static void ChooseShipLocations(PlayerBase player)
        {
            while (player.FloatingShipList.Count() < 5)
            {

                ShipBase ship = null;

                ship = ChooseShipAndValidate(player, ship);// Asks which ship the user would like to place and determines whether their input is valid.
                ship = ChooseShipStartPositionAndValidate(player, ship);// Asks what starting position to place the ship and determines whether their input is valid.
                ship = ChooseShipDirectionAndValidate(player, ship);// Asks which direction they'd like to orient their ship and determines whether their input is valid. 

                Console.Clear();
                player.FloatingShipList.Add(ship);

            }
        }

        private static ShipBase ChooseShipDirectionAndValidate(PlayerBase player, ShipBase ship)
        {
            while (true)
            {
                string[] acceptableDirectionOptions = new string[] { "up", "down", "left", "right" };

                Console.WriteLine("Which direction would you like to place your ship? (up, down, left, right)");

                var shipDirectionString = Console.ReadLine().ToLower();

                if (string.IsNullOrEmpty(shipDirectionString))
                {
                    Console.WriteLine("Please enter a valid response. Valid responses include: up, down, left, right");
                    Console.WriteLine("");
                    continue;
                }

                if (!Array.Exists(acceptableDirectionOptions, element => element == shipDirectionString))
                {
                    Console.WriteLine("Please enter a valid response. Valid responses include: up, down, left, right");
                    Console.WriteLine("");
                    continue;
                }

                ship.ShipDirection = Enum.GetValues(typeof(Direction)).Cast<Direction>().First(e => e.ToString() == shipDirectionString);
                ship.SetHorizontalEnd(ship.HorizontalStart, ship.ShipDirection);
                ship.SetVerticalEnd(ship.VerticalStart, ship.ShipDirection);

                if (!Gameplay.IsSelectionOnGameboard(ship))
                {
                    ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                    Console.WriteLine("You've placed your ship partially off the board. Please try again.");
                    Console.WriteLine("");
                    continue;
                }

                ship.SetRemainingShipCoordinates(ship.ShipDirection);

                if (Gameplay.DoShipCoordinatesOverlap(ship, player))
                {
                    ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                    Console.WriteLine("You've placed your ship on top of another ship. Please try again.");
                    Console.WriteLine($"");
                    continue;
                }
                break;
            }
            return ship;
        }

        private static ShipBase ChooseShipStartPositionAndValidate(PlayerBase player, ShipBase ship)
        {
            while (true)
            {
                string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

                Console.WriteLine($"Where would you like to place your {ship.Type}? (Choose from: Horizontal A - J and Vertical 1 - 10. For Example: A10.)");
                string locationString = Console.ReadLine();

                if (string.IsNullOrEmpty(locationString))
                {
                    Console.WriteLine("You must select a valid coordinate! Please try again.");
                    Console.WriteLine("");
                    continue;
                }

                var horizontalStartString = locationString.Substring(0, 1).ToUpper();
                var verticalStartString = locationString.Substring(1);

                if (string.IsNullOrEmpty(horizontalStartString) || string.IsNullOrEmpty(verticalStartString))
                {
                    Console.WriteLine("You must select a valid coordinate! Please try again.");
                    Console.WriteLine("");
                    continue;
                }

                if (!Int32.TryParse(verticalStartString, out int verticalStartInt))
                {
                    Console.WriteLine("You must select a valid vertical position! Please try again.");
                    Console.WriteLine("");
                    continue;
                }

                if (!Array.Exists(acceptableHorizontalOptions, element => element == horizontalStartString) ||
                        !Array.Exists(acceptableVerticalOptions, element => element == verticalStartString) ||
                        verticalStartInt >= 11 ||
                        verticalStartInt <= 0)
                {
                    Console.WriteLine("Please enter a valid coordinate.");
                    Console.WriteLine($"     Acceptable horizontal positions include: A, B, C, D, E, F, G, H, I, J");
                    Console.WriteLine($"     Acceptable vertical positions include: 1, 2 , 3, 4, 5, 6, 7, 8, 9, 10");
                    Console.WriteLine("");
                    continue;
                }

                ship.HorizontalStart = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalStartString);
                ship.VerticalStart = (Vertical)verticalStartInt;
                ship.SetInitialShipCoordinate(ship.HorizontalStart, ship.VerticalStart);

                if (Gameplay.DoShipCoordinatesOverlap(ship, player))
                {
                    ship.Coordinates.Clear();
                    Console.WriteLine("You've placed your ship on top of another ship. Please try again.");
                    Console.WriteLine("");
                    continue;
                }
                break;
            }
            return ship;
        }

        private static ShipBase ChooseShipAndValidate(PlayerBase player, ShipBase ship)
        {
            int shipTypeInt = 0;
            while (true)
            {
                Console.WriteLine("Which ship would you like to place? Carrier = 1, Cruiser = 2, Destroyer = 3, Battleship = 4, Submarine = 5");
                var shipChoice = Console.ReadLine();

                if (string.IsNullOrEmpty(shipChoice))
                {
                    Console.WriteLine("You must select a ship! Please try again.");
                    Console.WriteLine("");
                    continue;
                }
                if (!int.TryParse(shipChoice, out shipTypeInt))
                {
                    Console.WriteLine("That's not a valid choice. Please enter a value between 1 - 5!");
                    Console.WriteLine("");
                    continue;
                }

                if (Gameplay.DoShipTypesOverlap(player, shipTypeInt))
                {
                    Console.WriteLine($"You've already placed a {(ShipType)shipTypeInt}. Please try again.");
                    Console.WriteLine("");
                    continue;
                }

                if (shipTypeInt < 0 || shipTypeInt > 5)
                {
                    Console.WriteLine("That's not a valid choice. Please enter a value between 1 - 5!");
                    Console.WriteLine("");
                    continue;
                }

                var shipType = (ShipType)shipTypeInt;
                switch (shipType)
                {
                    case ShipType.Carrier:
                        ship = new Carrier();
                        break;
                    case ShipType.Cruiser:
                        ship = new Cruiser();
                        break;
                    case ShipType.Destroyer:
                        ship = new Destroyer();
                        break;
                    case ShipType.Battleship:
                        ship = new Ships.Battleship();
                        break;
                    case ShipType.Submarine:
                        ship = new Submarine();
                        break;
                    default:
                        break;
                }
                break;
            }
            return ship;
        }
    }

}
