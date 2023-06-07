using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    public class MovePlayerDto
    {
        public PointOfInterest PointOfInterest { get; set; }
        public TicketTypeEnum TicketType { get; set; }
    }
}
