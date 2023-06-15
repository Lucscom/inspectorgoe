using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameComponents.Model;


namespace GameComponents
{
    /// <summary>
    /// Controlls the game and validates the game state
    /// </summary>
    public class Controller
    {

        #region Singleton
        private Controller() { }

        private static Controller _instance;
        /// <summary>
        /// Creates one instance of controller or returns it
        /// </summary>
        /// <returns>Unique controller instance</returns>
        public static Controller GetInstance() { return _instance ??= new Controller(); }
        #endregion

        public GameState _gameState { get; private set; }

        /// <summary>
        /// Creating get methods for gamestate properties over controller
        /// </summary>
        public IReadOnlyList<PointOfInterest> PointsOfInterest => _gameState.PointsOfInterest;
        public IReadOnlyList<Player> Detectives => _gameState.Detectives;
        public Player MisterX => _gameState.MisterX;
        public int Round => _gameState.Round;
        public Player ActivePlayer => _gameState.ActivePlayer;


        /// <summary>
        /// Initializes the game state with given number of players
        /// </summary>
        /// <param name="playerNumber">number of players</param>
        public void Initialize(int playerNumber) 
        {
            if (playerNumber < 2 || playerNumber > 6)
            {
                throw new Exception("Invalid number of players");
            }

            _gameState = new GameState();

            InitPois();
            if (playerNumber > PointsOfInterest.Count)
            {
                throw new Exception("Not enough Pois");
            }
            InitPlayers(playerNumber);

            _gameState.ActivePlayer = _gameState.MisterX;
        }
        /// <summary>
        /// Initializes points of interest from game config
        /// </summary>
        private void InitPois()
        {
            //read POIs from JSON file Testmap in project folder 
            string jsonPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName;
            jsonPath += "\\Testmap.json";      
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(@jsonPath));  //read JSON to dynamic variable

            // generate POIs 
            int numPoi = 0;     //number of POIs in JSON-File
            foreach (var Nodes in json.Nodes)
            {
               _gameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name , new Vector2((float)Nodes.Location_x, (float)Nodes.Location_y)));
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
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo -1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ++numConnect;
                        break;
                    case 2:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ++numConnect;
                        break;
                    case 3:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        ++numConnect;
                        break;
                    case 12:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        numConnect = numConnect + 2;
                        break;
                    case 13:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 23:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 123:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
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
                var newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                while (positions.Contains(newNumber))
                {
                    newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                }
                positions[i] = newNumber;
            }

            //Create players and set position
            _gameState.Detectives.Clear();
            for (int i = 0; i <= numberOfPlayers - 2; i++)
            {
                var startPosition = _gameState.PointsOfInterest[positions[i]];
                var newPlayer = new Player(startPosition);
                _gameState.Detectives.Add(newPlayer);
            }
            _gameState.MisterX = new Player(_gameState.PointsOfInterest[positions[numberOfPlayers-1]]);
        }

        /// <summary>
        /// Checks if given point is blocked by other detective, ignores misterX
        /// </summary>
        /// <param name="point">Point of interest to check</param>
        /// <returns>true if point is blocked</returns>
        private bool PoiBlockedByDetective(PointOfInterest point)
        {
            //check if another player is on the field
            foreach (var p in _gameState.Detectives)
            {
                if (p.Position == point)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if given move is possible for given player
        /// </summary>
        /// <param name="player">Player to check move</param>
        /// <param name="point">Destination of move</param>
        /// <param name="ticketType">Chosen ticketype</param>
        /// <returns>true if move is possible</returns>
        private bool ValidateMove(Player player, PointOfInterest point, TicketTypeEnum ticketType)
        {
            if (PoiBlockedByDetective(point)) 
            {
                //throw new Exception("POI blocked by Detective!");
                return false;
            }

            //check for connection between points
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    if (player.BusTicket > 0) //check for valid ticket
                    {
                        if (!player.Position.ConnectionBus.Contains(point))
                            //throw new Exception("Connection Bus doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Bus Tickets!");
                        return false;
                    break;
                case TicketTypeEnum.Bike:
                    if (player.BikeTicket > 0)
                    {
                        if (!player.Position.ConnectionBike.Contains(point))
                            //throw new Exception("Connection Bike doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Bike Tickets!");
                        return false;
                    break;
                case TicketTypeEnum.Scooter:
                    if (player.ScooterTicket > 0)
                    {
                        if (!player.Position.ConnectionScooter.Contains(point))
                            //throw new Exception("Connection Scooter doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Scooter Tickets!");
                        return false;
                    break;
                default:
                    return false;
            }


            return true;
        }

        /// <summary>
        /// Checks and excecutes move
        /// </summary>
        /// <param name="player">Player to move</param>
        /// <param name="newPosition">Destination of move</param>
        /// <param name="ticketType">Chosen ticketype</param>
        /// <exception cref="Exception">Game over</exception>
        public bool MovePlayer(Player player, PointOfInterest newPosition, TicketTypeEnum ticketType)
        {
            if (player != ActivePlayer) return false;

            if (!ValidateMove(player, newPosition, ticketType))
            {
                return false;
            }

            player.Position = newPosition;

            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    player.BusTicket -= 1;
                    break;
                case TicketTypeEnum.Bike:
                    player.BikeTicket -= 1;
                    break;
                case TicketTypeEnum.Scooter:
                    player.ScooterTicket -= 1;
                    break;
                default:
                    break;
            }

            if (player != MisterX && FoundMisterX(player))
                throw new Exception("Game OVER");

            return true;
        }

        /// <summary>
        /// Checks if player position equals misterX position
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool FoundMisterX(Player player)
        {
            if (_gameState.MisterX.Position.Equals(player.Position)) return true;
            else return false;
        }
    }
}
