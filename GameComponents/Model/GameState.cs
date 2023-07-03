using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    /// <summary>
    /// Contains the informations about the game status
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// List of all the player with role detective
        /// </summary>
        public List<Player> Detectives { get; set; } = new List<Player>();
        /// <summary>
        /// Defines the misterX player
        /// </summary>
        public Player MisterX { get; set; }
        /// <summary>
        /// List of all the players in the game. Automatically generated from misterX and detectives
        /// </summary>
        public List<Player> AllPlayers => new List<Player>(new[] { MisterX }).Concat(Detectives).ToList();
        /// <summary>
        /// Currently active player, referenz to decetives list or misterX
        /// </summary>
        public Player ActivePlayer { get; set; }
        /// <summary>
        /// Player that created the game
        /// </summary>
        public Player GameCreator { get; set; }
        /// <summary>
        /// Move counter
        /// </summary>
        public int Move { get; set; } = 0;
        /// <summary>
        /// Round counter
        /// </summary>
        public int Round => Move / (AllPlayers.Count == 0 ? 0 : AllPlayers.Count);
        /// <summary>
        /// True if game is started
        /// </summary>
        public bool GameStarted => Move > 0;
        /// <summary>
        /// Contains all point of interest that are in the game
        /// </summary>
        public List<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();
        /// <summary>
        /// Contains the last Tickets than MisterX used
        /// </summary>
        public List<TicketTypeEnum> TicketHistoryMisterX { get; set; } = new List<TicketTypeEnum>();
        /// <summary>
        /// Contains the last known PointofInterest of MisterX
        /// </summary>
        public PointOfInterest MisterXLastKnownPOI { get; set; }
    }
}
