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
        /// Currently active player, referenz to decetives list or misterX
        /// </summary>
        public Player ActivePlayer { get; set; }
        /// <summary>
        /// Roundcounter
        /// </summary>
        public int Round { get; set; }
        /// <summary>
        /// Contains all point of interest that are in the game
        /// </summary>
        public List<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();
        /// <summary>
        /// Contains the last Tickets than MisterX used
        /// </summary>
        public List<TicketTypeEnum> TicketHistoryMisterX { get; set; }   
        /// <summary>
        /// Contains the last known PointofInterest of MisterX
        /// </summary>
        public PointOfInterest MisterXLastKnownPOI { get; set; }
    }
}
