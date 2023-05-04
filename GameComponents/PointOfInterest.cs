using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace GameComponents
{
    /// <summary>
    /// Contains information to a point of interest and links to other points of interest
    /// </summary>
    public class PointOfInterest
    {
        /// <summary>
        /// Acts as the unique identifier for the point of interest
        /// </summary>
        public int Number { get; private set;  }
        /// <summary>
        /// Coordinates on game board
        /// </summary>
        public Point Location { get; private set; }
        /// <summary>
        /// List of connected points of interest reachable by bus
        /// </summary>
        public List<PointOfInterest> ConnectionBus { get; private set; } = new List<PointOfInterest>();
        /// <summary>
        /// List of connected points of interest reachable by bike
        /// </summary>
        public List<PointOfInterest> ConnectionBike { get; private set; } = new List<PointOfInterest>();
        /// <summary>
        /// List of connected points of interest reachable by scooter
        /// </summary>
        public List<PointOfInterest> ConnectionScooter { get; private set; } = new List<PointOfInterest>();

        /// <summary>
        /// Constructor to init point of interest
        /// </summary>
        /// <param name="number">Unique identifier</param>
        /// <param name="location">Coordinates on game board</param>
        public PointOfInterest(int number, Point location)
        {
            Number = number;
            Location = location;
        }
    }
}
