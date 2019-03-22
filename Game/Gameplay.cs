using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Game;
using Battleship.Ships;
using Battleship.Players;

namespace Battleship
{
    public class Gameplay
    {
        public bool GameOn = true;
        public GameType Type { get; private set; }
        public PlayerBase Player1 { get; set; }
        public PlayerBase Player2 { get; set; }

        public void SinglePlayerGame ()
        {
            Type = GameType.SinglePlayer;

            SinglePlayerSetupSequence();
                   
            while (GameOn)
            {
                Fire(Player1, Player2);
                Fire(Player2, Player1);
                CheckforWinner(Player1, Player2);               
            }

            PlayerBase winner = DetermineWinner(Player1, Player2); 
            
            Console.WriteLine($"The Winner is {winner.Name}");
        }

        public PlayerBase DetermineWinner(PlayerBase player1, PlayerBase player2)
        {           
            if (player1.FloatingShipList.Count > player2.FloatingShipList.Count)
            {
                var winner = player1;                
                return winner;
            } 
            else
            {
                var winner = player2;
                return winner;
            }            
        }

        public void CheckforWinner(PlayerBase player1, PlayerBase player2)
        {
            if (player1.FloatingShipList.Count == 0 || player2.FloatingShipList.Count == 0)
            {
                GameOn = false;
            }
        }

        private void SinglePlayerSetupSequence()
        {
                    
            Player2 = new ComputerPlayer();
            RandomizeShipSelections(Player2);
            Player1 = new HumanPlayer();
            UI.GetHumanPlayerName(Player1);
            if (UI.SelectHowtoChoosePieceLocations() == 1)
            {
                RandomizeShipSelections(Player1);
                UI.DisplayShipsAndShipLocations(Player1.FloatingShipList);
                //UI.GameCountdown();
            }
            else
            {
                ChooseShipLocations(Player1);
                UI.DisplayShipsAndShipLocations(Player1.FloatingShipList);
                //UI.GameCountdown();
            }

        }

        public void ChooseShipLocations(PlayerBase player)
        {
            while (player.FloatingShipList.Count() < 5)
            {
                int shipTypeInt = 0;
                ShipBase ship = null;

                // Asks which ship the user would like to place and determines whether their input is valid.
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

                    if (DoShipTypesOverlap(player, shipTypeInt))
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

                // Asks what starting position to place the ship and determines whether their input is valid.
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

                    if (DoShipCoordinatesOverlap( ship, player ))
                    {
                        ship.Coordinates.Clear();
                        Console.WriteLine("You've placed your ship on top of another ship. Please try again.");
                        Console.WriteLine("");
                        continue;
                    }
                    break;
                }

                // Asks which direction they'd like to orient their ship and determines whether their input is valid. 
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

                    if (!IsSelectionOnGameboard(ship))
                    {
                        ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                        Console.WriteLine("You've placed your ship partially off the board. Please try again.");
                        Console.WriteLine("");
                        continue;
                    }

                    ship.SetRemainingShipCoordinates(ship.ShipDirection);

                    if (DoShipCoordinatesOverlap(ship, player))
                    {
                        ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                        Console.WriteLine("You've placed your ship on top of another ship. Please try again.");
                        Console.WriteLine($"");
                        continue;
                    }
                    break;
                }
                Console.Clear();
                player.FloatingShipList.Add(ship);
                UI.DisplayShipsAndShipLocations(player.FloatingShipList);
            }
        }

        public bool DoShipTypesOverlap(PlayerBase player, ShipBase ship)
        {
            List<ShipType> allShipTypes = new List<ShipType>();
            foreach (ShipBase shipItem in player.FloatingShipList)
            {
                allShipTypes.Add(shipItem.Type);
            }
            if (allShipTypes.Contains(ship.Type))
            {
                Console.WriteLine($"You've already added a {ship.Type} to the gameboard.");
                return true;
            }
            return false;
        }

        public bool DoShipTypesOverlap(PlayerBase player, int shipType)
        {
            var correctShipType = (ShipType)shipType;
            List<ShipType> allShipTypes = new List<ShipType>();
            foreach (ShipBase shipItem in player.FloatingShipList)
            {
                allShipTypes.Add(shipItem.Type);
            }
            if (allShipTypes.Contains(correctShipType))
            {
                return true;
            }
            return false;
        }

        public void TransferSunkShipsToAppropriateShipList(PlayerBase opponent)
        {
            for (var i = 0; i < opponent.FloatingShipList.Count; i++)
            {
                if (opponent.FloatingShipList[i].IsSunk == true)
                {
                    opponent.SunkShipList.Add(opponent.FloatingShipList[i]);
                    opponent.FloatingShipList.RemoveAt(i);
                }
            }
        }

        protected bool DidShotHitOpponent(string shotCoordinate, PlayerBase opponent)
        {
            foreach (ShipBase ship in opponent.FloatingShipList)
            {
                if (ship.Coordinates.Contains(shotCoordinate))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        protected bool HasShotAlreadyBeenSelected(string shotCoordinate, PlayerBase player)
        {
            if (player.MissedShotCoordinates.Contains(shotCoordinate))
            {
                return true;
            }
            return false;
        }


        public void RandomizeShipSelections(PlayerBase player)
        {
            int[] shipTypeIntArray = new int[] { 1, 2, 3, 4, 5 };
            ShipBase ship = null;
            for (var i = 0; i < 5; i++)
            {
                var shipTypeInt = shipTypeIntArray[i];
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

                while (true)
                {

                    string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                    string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

                    Random rnd = new Random();
                    var randomHorizontalIndex = rnd.Next(0, acceptableHorizontalOptions.Length);
                    var randomVerticalIndex = rnd.Next(0, acceptableVerticalOptions.Length);
                    var horizontalStartString = acceptableHorizontalOptions[randomHorizontalIndex];
                    var verticalStartString = acceptableVerticalOptions[randomVerticalIndex];
                    Int32.TryParse(verticalStartString, out int verticalStartInt);
                    ship.HorizontalStart = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalStartString);
                    ship.VerticalStart = (Vertical)verticalStartInt;
                    if (horizontalStartString.Length > 1)
                    {
                        ship.Coordinates.Clear();
                        continue;
                    }
                    ship.SetInitialShipCoordinate(ship.HorizontalStart, ship.VerticalStart);

                    if (DoShipCoordinatesOverlap(ship, player))
                    {
                        ship.Coordinates.Clear();
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    string[] acceptableDirectionOptions = new string[] { "up", "down", "left", "right" };

                    Random rnd = new Random();
                    var randomDirectionIndex = rnd.Next(0, acceptableDirectionOptions.Length);
                    var shipDirectionString = acceptableDirectionOptions[randomDirectionIndex];
                    ship.ShipDirection = Enum.GetValues(typeof(Direction)).Cast<Direction>().First(e => e.ToString() == shipDirectionString);
                    ship.SetHorizontalEnd(ship.HorizontalStart, ship.ShipDirection);
                    ship.SetVerticalEnd(ship.VerticalStart, ship.ShipDirection);

                    if (!IsSelectionOnGameboard(ship))
                    {
                        ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                        continue;
                    }

                    ship.SetRemainingShipCoordinates(ship.ShipDirection);

                    if (DoShipCoordinatesOverlap(ship, player))
                    {
                        ship.Coordinates.RemoveRange(1, (ship.Coordinates.Count() - 1));
                        continue;
                    }
                    break;
                }
                player.FloatingShipList.Add(ship);
            }            
        }

        protected bool DoShipCoordinatesOverlap(ShipBase ship, PlayerBase player)
        {
            List<string> allShipCoordinates = new List<string>();
            foreach (ShipBase shipp in player.FloatingShipList)
            {
                for (var i = 0; i < shipp.Coordinates.Count; i++)
                {
                    allShipCoordinates.Add(shipp.Coordinates[i]);
                }
            }
            if (allShipCoordinates.Intersect(ship.Coordinates).Any())
            {
                return true;
            }
            return false;
        }

        public void CheckIfShotSunkAnOpponentShip(string shotCoordinate, PlayerBase opponent)
        {
            foreach (ShipBase ship in opponent.FloatingShipList)
            {
                if (ship.IsSunk == true)
                {
                    Console.WriteLine($"You've sunk {opponent.Name}'s {ship.Type}!");
                    Console.WriteLine();
                    return;
                }
                else if (ship.Coordinates.Contains(shotCoordinate))
                {
                    Console.WriteLine($"You hit {opponent.Name}'s {ship.Type}! Nice work.");
                    Console.WriteLine();
                }
            }
        }

        public bool IsSelectionOnGameboard(ShipBase ship)
        {
            if (!Enum.IsDefined(typeof(Horizontal), ship.HorizontalEnd) || !Enum.IsDefined(typeof(Vertical), ship.VerticalEnd))
            {
                return false;
            }
            return true;
        }

        public string CoordinateReformatter(string shotCoordinateString)
        {
            var horizontalString = shotCoordinateString.Substring(0, 1).ToUpper();
            var verticalString = shotCoordinateString.Substring(1);
            var horizontalStartEnum = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalString);
            var verticalStartInt = Int32.Parse(verticalString);
            var verticalStartEnum = (Vertical)verticalStartInt;
            string reformattedCoodinate = horizontalString.ToString().ToUpper() + verticalStartEnum.ToString().ToUpper();
            return reformattedCoodinate;
        }

        private void GoodHitHandler(string shotCoordinate, PlayerBase opponent)
        {
            AddCoordinateToShipHitList(shotCoordinate, opponent);
            CheckIfShotSunkAnOpponentShip(shotCoordinate, opponent);
            TransferSunkShipsToAppropriateShipList(opponent);
        }

        public void AddCoordinateToShipHitList(string coordinate, PlayerBase opponent)
        {
            foreach (ShipBase opponentShip in opponent.FloatingShipList)
            {
                if (opponentShip.IsSunk == true)
                {
                    continue;
                }
                else if (opponentShip.Coordinates.Contains(coordinate))
                {
                    opponentShip.HitCoordinates.Add(coordinate);
                    opponentShip.SetShipStatus();
                    break;
                }
            }
        }

        public bool IsCoordinateInAcceptableFormat(string coordinate)
        {
            string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            if (string.IsNullOrEmpty(coordinate))
            {
                UI.EmptySelectionMessage();
                return false;
            }

            var horizontalString = coordinate.Substring(0, 1).ToUpper();
            var verticalString = coordinate.Substring(1);

            if (string.IsNullOrEmpty(horizontalString) ||
                string.IsNullOrEmpty(verticalString) ||
                string.IsNullOrWhiteSpace(horizontalString) ||
                string.IsNullOrWhiteSpace(verticalString))
            {
                UI.NeedValidVerticalandHorizontalMessage();
                return false;

            }
            return true;
        }

        public bool IsCoordinateOnGameboard(string coordinate)
        {
            string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            var horizontalString = coordinate.Substring(0, 1).ToUpper();
            var verticalString = coordinate.Substring(1);

            if (!Int32.TryParse(verticalString, out int verticalStartInt))
            {
                UI.InvalidVerticalPositionMessage();
                return false;
            }

            if (!Array.Exists(acceptableHorizontalOptions, element => element == horizontalString) ||
                    !Array.Exists(acceptableVerticalOptions, element => element == verticalString) ||
                    verticalStartInt >= 11 ||
                    verticalStartInt <= 0)
            {
                UI.InvalidCoordinateMessage();
                return false;
            }

            var horizontalEnum = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalString);
            var verticalEnum = (Vertical)verticalStartInt;

            if (!Enum.IsDefined(typeof(Horizontal), horizontalEnum) || !Enum.IsDefined(typeof(Vertical), verticalEnum))
            {
                UI.InvalidCoordinateMessage();
                return false;
            }

            return true;
        }

        public void Fire(PlayerBase player, PlayerBase opponent)
        {
            if (player.Type == Players.PlayerType.Human)
            {
                while (true)
                {
                    //UI.DisplayOpponentsShipLocations(opponent);
                    UI.DisplayAllMissedCoordinates(player.MissedShotCoordinates);
                    UI.DisplayDirectHits(opponent);
                    UI.DisplayOpponentsSunkShipNames(opponent);
                    Console.WriteLine("**************************************************************************************************************");
                    UI.AskForShotCoordinate();
                    string shotCoordinateString = Console.ReadLine().ToUpper();
                    if (!IsCoordinateInAcceptableFormat(shotCoordinateString) || !IsCoordinateOnGameboard(shotCoordinateString))
                    {
                        Console.WriteLine();
                        continue;
                    }
                    var shotCoordinate = CoordinateReformatter(shotCoordinateString);
                    if (HasShotAlreadyBeenSelected(shotCoordinate, player))
                    {
                        Console.WriteLine($"You've already fired at that coordinate.");
                        Console.WriteLine();
                        continue;
                    }
                    if (!DidShotHitOpponent(shotCoordinate, opponent))
                    {
                        player.MissedShotCoordinates.Add(shotCoordinate);
                        Console.WriteLine($"You missed! Better luck next time.");
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        GoodHitHandler(shotCoordinate, opponent);
                        break;
                    }
                }
            }
            else if (player.Type == Players.PlayerType.Computer)
            {
                string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                while (true)
                {
                    Random rnd1 = new Random();
                    Random rnd2 = new Random();

                    var randomHorizontalIndex = rnd1.Next(0, acceptableHorizontalOptions.Length);
                    var randomVerticalIndex = rnd2.Next(0, acceptableVerticalOptions.Length);
                    var horizontalStartString = acceptableHorizontalOptions[randomHorizontalIndex];
                    var verticalStartString = acceptableVerticalOptions[randomVerticalIndex];

                    string shotCoordinateString = horizontalStartString + verticalStartString;
                    if (!IsCoordinateInAcceptableFormat(shotCoordinateString) || !IsCoordinateOnGameboard(shotCoordinateString))
                    {
                        continue;
                    }
                    var shotCoordinate = CoordinateReformatter(shotCoordinateString);
                    if (!DidShotHitOpponent(shotCoordinate, opponent))
                    {
                        Console.WriteLine($"Phew. {player.Name} missed!");
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        AddCoordinateToShipHitList(shotCoordinate, opponent);
                        TransferSunkShipsToAppropriateShipList(opponent);
                        Console.WriteLine($"Damn, {player.Name} hit you!");
                        Console.WriteLine();
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Oh. shit. Something unexpected happened.");
            }


        }

        //public void AddCoordinateToShipHitList(string coordinate, PlayerBase opponent)
        //{
        //    foreach (ShipBase opponentShip in opponent.FloatingShipList)
        //    {
        //        if (opponentShip.IsSunk == true)
        //        {
        //            continue;
        //        }
        //        else if (opponentShip.Coordinates.Contains(coordinate))
        //        {
        //            opponentShip.HitCoordinates.Add(coordinate);
        //            opponentShip.CheckShipStatus();
        //            break;
        //        }
        //    }
        //}

        //public bool IsCoordinateInAcceptableFormat(string coordinate)
        //{
        //    string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        //    string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        //    if (string.IsNullOrEmpty(coordinate))
        //    {
        //        return false;
        //    }

        //    var horizontalString = coordinate.Substring(0, 1).ToUpper();
        //    var verticalString = coordinate.Substring(1);

        //    if (string.IsNullOrEmpty(horizontalString) ||
        //        string.IsNullOrEmpty(verticalString) ||
        //        string.IsNullOrWhiteSpace(horizontalString) ||
        //        string.IsNullOrWhiteSpace(verticalString))
        //    {
        //        return false;

        //    }
        //    return true;
        //}

        //public bool IsCoordinateOnGameboard(string coordinate)
        //{
        //    string[] acceptableHorizontalOptions = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        //    string[] acceptableVerticalOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        //    var horizontalString = coordinate.Substring(0, 1).ToUpper();
        //    var verticalString = coordinate.Substring(1);

        //    if (!Int32.TryParse(verticalString, out int verticalStartInt))
        //    {
        //        return false;
        //    }

        //    if (!Array.Exists(acceptableHorizontalOptions, element => element == horizontalString) ||
        //            !Array.Exists(acceptableVerticalOptions, element => element == verticalString) ||
        //            verticalStartInt >= 11 ||
        //            verticalStartInt <= 0)
        //    {
        //        return false;
        //    }

        //    var horizontalEnum = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalString);
        //    var verticalEnum = (Vertical)verticalStartInt;

        //    if (!Enum.IsDefined(typeof(Horizontal), horizontalEnum) || !Enum.IsDefined(typeof(Vertical), verticalEnum))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public string CoordinateReformatter(string shotCoordinateString)
        //{
        //    var horizontalString = shotCoordinateString.Substring(0, 1).ToUpper();
        //    var verticalString = shotCoordinateString.Substring(1);
        //    var horizontalStartEnum = Enum.GetValues(typeof(Horizontal)).Cast<Horizontal>().First(e => e.ToString() == horizontalString);
        //    var verticalStartInt = Int32.Parse(verticalString);
        //    var verticalStartEnum = (Vertical)verticalStartInt;
        //    string reformattedCoodinate = horizontalString.ToString().ToUpper() + verticalStartEnum.ToString().ToUpper();
        //    return reformattedCoodinate;
        //}
    }
}
