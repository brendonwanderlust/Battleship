using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class UI
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
                Console.WriteLine($"   {ship.Type}: Located at coordinates: {ship.ReturnShipCoordinates()}");
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
                //Console.WriteLine($"You haven't sunk any of {opponent.Name}'s ships yet.");
                return;
            }
            else
            {
                Console.WriteLine($"You've sunk ({opponent.SunkShipList.Count}) of your opponents ships:");
                //foreach (ShipBase ship in opponent.SunkShipList)
                //{
                //    Console.WriteLine($"     {ship.Type}");
                //}
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
                if (!Int32.TryParse(selction, out int selectionInt))
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
                
                Console.WriteLine($"{a}");    // Add space to make sure to override previous contents
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Let's play Battleship!");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }


    }

}
