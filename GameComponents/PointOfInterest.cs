using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace GameComponents
{
    internal class PointOfInterest
    {
        public int Number { get; private set;  }
        public Point Location { get; private set; }
        public List<PointOfInterest> ConnectionBus { get; private set; } = new List<PointOfInterest>();
        public List<PointOfInterest> ConnectionBike { get; private set; } = new List<PointOfInterest>();
        public List<PointOfInterest> ConnectionScooter { get; private set; } = new List<PointOfInterest>();

        public PointOfInterest(int number, Point location)
        {
            Number = number;
            Location = location;
        }
    }
}
