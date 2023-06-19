using GameComponents.Model;


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
    }
}
