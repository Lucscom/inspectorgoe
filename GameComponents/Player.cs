using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;


namespace GameComponents
{
    /// <summary>
    /// Contains information about player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Playername
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Picture of player
        /// </summary>
        public Image Avatar { get; set; }
        /// <summary>
        /// The current position as point of interest
        /// </summary>
        public PointOfInterest Position { get; set; }
        /// <summary>
        /// Number of bustickets available to the player
        /// </summary>
        public int BusTicket { get; set; }
        /// <summary>
        /// Number of biketickets available to the player
        /// </summary>
        public int BikeTicket { get; set; }
        /// <summary>
        /// Number of scootertickets available to the player
        /// </summary>
        public int ScooterTicket { get; set; }

        /// <summary>
        /// Constructor to init player
        /// </summary>
        /// <param name="position">Start position as point of interest</param>
        public Player(PointOfInterest position)
        {
            Position = position;
        }
    }
}
