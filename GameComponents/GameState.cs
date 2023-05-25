using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
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
    }
}
