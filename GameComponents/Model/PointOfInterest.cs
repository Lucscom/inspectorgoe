using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    /// <summary>
    /// Contains information to a point of interest and links to other points of interest
    /// </summary>
    [JsonObject(IsReference = true)]
    [NotMapped]
    public class PointOfInterest
    {
        /// <summary>
        /// Acts as the unique identifier for the point of interest
        /// </summary>
        /// 
        public int Number { get; private set; }
        /// <summary>
        /// Coordinates on game board
        /// </summary>
        public Vector2 Location { get; private set; }
        /// <summary>
        /// Real name of POI
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// List of connected points of interest reachable by bus
        /// </summary>
        public List<int> ConnectionBus { get; set; } = new List<int>();
        /// <summary>
        /// List of connected points of interest reachable by bike
        /// </summary>
        public List<int> ConnectionBike { get; set; } = new List<int>();
        /// <summary>
        /// List of connected points of interest reachable by scooter
        /// </summary>
        public List<int> ConnectionScooter { get; set; } = new List<int>();

        /// <summary>
        /// Constructor to init point of interest
        /// </summary>
        /// <param name="number">Unique identifier</param>
        /// <param name="name">Real name</param>
        /// <param name="location">Coordinates on game board</param>
        public PointOfInterest(int number, string name, Vector2 location)
        {
            Number = number;
            Name = name;
            Location = location;
        }
    }
}
