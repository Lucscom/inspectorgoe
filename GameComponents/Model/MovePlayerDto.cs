using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    public class MovePlayerDto
    {
        public MovePlayerDto(PointOfInterest poi, TicketTypeEnum ticketType)
        {
            PointOfInterest = poi;
            TicketType = ticketType;
        }

        public PointOfInterest PointOfInterest { get; set; }
        public TicketTypeEnum TicketType { get; set; }
    }
}
