using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;


namespace GameComponents
{
    internal class Player
    {
        public string Name { get; set; }
        public Image Avatar { get; set; }
        public PointOfInterest Position { get; set; }
        public int BusTicket { get; set; }
        public int BikeTicket { get; set; }
        public int ScooterTicket { get; set; }

        public Player(PointOfInterest position)
        {
            Position = position;
        }
    }
}
