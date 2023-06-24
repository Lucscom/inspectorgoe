using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    /// <summary>
    /// Class to display the tickets 
    /// </summary>
    public class TicketsView
    {
        /// <summary>
        /// Ticket Imagepath
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Number of the Round
        /// </summary>
        public int NumberRound { get; set; }

        /// <summary>
        /// Borderthickness for the border of the ticket if Misterx Position is discovered
        /// </summary>
        public int BorderThickness { get; set; }

        /// <summary>
        /// Color for the Frame behind the Roundnumber (only if ther is no ticket)
        /// </summary>
        public Color NumberFrameColor { get; set; }

        /// <summary>
        /// Color of the Roundnumber
        /// </summary>
        public Color NumberColor { get; set; }

    }
}
