using GameComponents.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    public class GameController
    {
        private GameState GameState { get; set; } = new GameState();
        
        public bool StartGame()
        {
            Console.WriteLine("Game starting");
            if (GameState.MisterX == null)
            {
                Console.WriteLine("MisterX still missing");
                Console.WriteLine("Game not started");
                return false;
            }
            if (GameState.Detectives.Count < 1) //todo: How many detectives are needed?
            {
                Console.WriteLine("Not enough detectives");
                Console.WriteLine("Game not started");
                return false;
            }
            InitPois();
            InitPlayers(GameState.Detectives.Count + 1); // + MisterX

            GameState.ActivePlayer = GameState.MisterX;

            GameState.Round = 1;
            Console.WriteLine("Game started");
            return true;
        }
        public void ResetGame() 
        { 
            Console.WriteLine("Game reset");
            GameState = new GameState();
        }

        public bool AddPlayer(Player player)
        {
            if (GameState.GameStarted)
            {
                Console.WriteLine("Player not added");
                return false;
            }
            else if (GameState.MisterX == null)
            {
                GameState.MisterX = player;
                GameState.MisterX.BlackTicket = 5;
                GameState.MisterX.DoubleTicket = 2;
                //free tickets for X!!!!!!!!!!
                GameState.MisterX.BusTicket = int.MaxValue;
                GameState.MisterX.BikeTicket = int.MaxValue;
                GameState.MisterX.ScooterTicket = int.MaxValue;
                Console.WriteLine("MisterX added");
                return true;
            }
            else if (GameState.Detectives.Count < 5)
            {
                GameState.Detectives.Add(player);
                Console.WriteLine("Detective added");
                return true;
            }
            else
            {
                Console.WriteLine("Player not added");
                return false;
            }
        }
        public bool MovePlayer(Player player, PointOfInterest poi, TicketTypeEnum ticketType)
        { 
            //exchange player with player from GameState. This is necessary because the poi attribute is not the same as in the player object in the GameState
            //same for poi
            //todo: force poi numbers to be unique
            if (GameState.GameStarted && Validator.ValidateMove(GameState.AllPlayers.Where(p => p.UserName == player.UserName).First(), 
                GameState.PointsOfInterest.Where(p => p.Number == poi.Number).First(), 
                ticketType, 
                GameState.Detectives)) //todo: clean this up
            {
                player.Position = poi;
                Console.WriteLine("Player moved");
                NextRound();
                return true;
            }

            return false;
        }


        /// <summary>
        /// Initializes points of interest from game config
        /// </summary>
        private void InitPois()
        {
            //read POIs from JSON file Testmap in project folder 
            string jsonContent = new StreamReader(File.OpenRead("Testmap.json")).ReadToEnd();
            dynamic json = JsonConvert.DeserializeObject(jsonContent);  //read JSON to dynamic variable

            // generate POIs 
            int numPoi = 0;     //number of POIs in JSON-File
            foreach (var Nodes in json.Nodes)
            {
                GameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name, new Vector2((float)Nodes.Location_x, (float)Nodes.Location_y)));
                ++numPoi;
            }

            // generate Connections 
            int numConnect = 0;     //total number of Connections 
            var noSuccess = 0;      //  =1 in default case
            foreach (var Connections in json.Connections)
            {
                switch ((int)Connections.type) //defines number and type of connections between POIs
                {
                    // !!! Diese Logik stimmt bisher nur, wenn die POIs in der Reihenfolge ihrer Laufnummer initialisiert werden, da No in _gameState.PointsOfInterest[No] nur die Position in der Liste repräsentiert, nicht die Laufnummer!
                    case 1:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ++numConnect;
                        break;
                    case 2:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ++numConnect;
                        break;
                    case 3:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        ++numConnect;
                        break;
                    case 12:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        numConnect = numConnect + 2;
                        break;
                    case 13:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 23:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 123:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 3;
                        break;
                    default:
                        noSuccess = 1;
                        break;
                }
            }

        }

        /// <summary>
        /// Creates the connection between the points of interest
        /// </summary>
        /// <param name="pointOfInterest1">First point of connection</param>
        /// <param name="pointOfInterest2">Second point of connection</param>
        /// <param name="ticketType">Type of connection</param>
        private void ConnectPois(PointOfInterest pointOfInterest1, PointOfInterest pointOfInterest2, TicketTypeEnum ticketType)
        {
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    pointOfInterest1.ConnectionBus.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionBus.Add(pointOfInterest1);
                    break;
                case TicketTypeEnum.Bike:
                    pointOfInterest1.ConnectionBike.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionBike.Add(pointOfInterest1);
                    break;
                case TicketTypeEnum.Scooter:
                    pointOfInterest1.ConnectionScooter.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionScooter.Add(pointOfInterest1);
                    break;
            }
        }

        /// <summary>
        /// Initializes players with start positions
        /// Checks if the start position is blocked
        /// </summary>
        /// <param name="numberOfPlayers">Number of players</param>
        private void InitPlayers(int numberOfPlayers)
        {
            //Array with random unique numbers
            //Set starting points
            var positions = Enumerable.Repeat<int>(-1, numberOfPlayers).ToArray();

            for (int i = 0; i < positions.Length; i++)
            {
                var newNumber = Random.Shared.Next(0, GameState.PointsOfInterest.Count - 1);
                while (positions.Contains(newNumber))
                {
                    newNumber = Random.Shared.Next(0, GameState.PointsOfInterest.Count - 1);
                }
                positions[i] = newNumber;
            }

            //Create players and set position
            for (int i = 0; i <= numberOfPlayers - 2; i++)
            {
                var startPosition = GameState.PointsOfInterest[positions[i]];
                GameState.Detectives[i].Position = startPosition;
            }
            GameState.MisterX.Position = GameState.PointsOfInterest[positions[numberOfPlayers - 1]];
        }
        private void NextRound()
        {
            GameState.ActivePlayer = GameState.AllPlayers[GameState.Round++ % GameState.AllPlayers.Count];
        }
    }
}
