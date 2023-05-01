using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    internal class GameState
    {
        //public ReadOnlyCollection<Player> Players => (new List<Player>() { MisterX }).Concat(Detectives).ToList().AsReadOnly();
        public List<Player> Detectives { get; set; } = new List<Player>();
        public Player MisterX { get; set; }
        public Player ActivePlayer { get; set; }
        public int Round { get; set; }

        public List<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();
    }
}
