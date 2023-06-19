using GameComponents.Model;
using Newtonsoft.Json;
using System.Numerics;

namespace GameComponents
{
    /// <summary>
    /// Validates moves
    /// </summary>
    public static class Validator
    {

        /// <summary>
        /// Checks if given point is blocked by other detective, ignores misterX
        /// </summary>
        /// <param name="point">Point of interest to check</param>
        /// <returns>true if point is blocked</returns>
        public static bool PoiBlockedByDetective(PointOfInterest point, List<Player> detectives)
        {
            //check if another player is on the field
            foreach (var p in detectives)
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
        public static bool ValidateMove(Player player, PointOfInterest point, TicketTypeEnum ticketType, List<Player> detectives)
        {
            if (PoiBlockedByDetective(point, detectives)) 
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
        /// Find all valid pois with ticket types for a given player
        /// </summary>
        /// <param name="gameState">The current gameState</param>
        /// <param name="player">The player who wants to make a move</param>
        /// <returns>Dictionary that contains all possible pois with ticket type</returns>
        public static Dictionary<PointOfInterest, List<TicketTypeEnum>> GetValidMoves(GameState gameState, Player player)
        {
            Dictionary<PointOfInterest, List<TicketTypeEnum>> moves = new Dictionary<PointOfInterest, List<TicketTypeEnum>> ();

            if(player.BikeTicket > 0)
            {
                moves = AddValidMovesForTicket(moves, TicketTypeEnum.Bike, player.Position.ConnectionBike, gameState.Detectives);
            }
            if (player.ScooterTicket > 0)
            {
                moves = AddValidMovesForTicket(moves, TicketTypeEnum.Scooter, player.Position.ConnectionScooter, gameState.Detectives);
            }
            if (player.BusTicket > 0)
            {
                moves = AddValidMovesForTicket(moves, TicketTypeEnum.Bike, player.Position.ConnectionBus, gameState.Detectives);
            }
            //TODO: doppeltickets
            if (player == gameState.MisterX && player.BlackTicket > 0)
            {
                List<PointOfInterest> pois = new List<PointOfInterest>();
                pois.Concat(player.Position.ConnectionBus);
                pois.Concat(player.Position.ConnectionScooter);
                pois.Concat(player.Position.ConnectionBike);

                moves = AddValidMovesForTicket(moves, TicketTypeEnum.Black, pois, gameState.Detectives);
            }

            return moves;
        }

        /// <summary>
        /// Find all valid Pois for one ticket type
        /// </summary>
        /// <param name="moves">Possible pois are added to this directory</param>
        /// <param name="ticketType">Ticket type for valid moves</param>
        /// <param name="pois">Connectins pois to player position</param>
        /// <param name="detectives">Detective players to get blocked pois</param>
        /// <returns>Dictionary that contains all possible pois for one ticket type</returns>
        private static Dictionary<PointOfInterest, List<TicketTypeEnum>> AddValidMovesForTicket(Dictionary<PointOfInterest, List<TicketTypeEnum>> moves, TicketTypeEnum ticketType, List<PointOfInterest> pois, List<Player> detectives)
        {
            //Remove pois that are blocked by other detectives
            foreach (Player player in detectives)
            {
                pois.Remove(player.Position);
            }
            foreach (PointOfInterest poi in pois)
            {
                if (moves.ContainsKey(poi))
                {
                    List<TicketTypeEnum> tickets = new List<TicketTypeEnum>();
                    tickets.AddRange(moves[poi]);
                    tickets.Add(TicketTypeEnum.Bike);
                    moves[poi] = tickets;
                }
                else
                {
                    moves.Add(poi, new List<TicketTypeEnum> { TicketTypeEnum.Bike });
                }
            }
            return moves;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //The following methods are only for test pourposes and will be removed in the future!
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes points of interest from game config
        /// </summary>
        public GameState InitPois()
        {
            //read POIs from JSON file Testmap in project folder 
            //todo: change json location
            string jsonContent = new StreamReader(File.OpenRead("C:/Users/phili/source/repos/inspectorgoe/GameComponents/Testmap.json")).ReadToEnd();      //read JSON file from project folder
            dynamic json = JsonConvert.DeserializeObject(jsonContent);  //read JSON to dynamic variable

            GameState GameState = new GameState();

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
            return GameState;
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
    }
}
