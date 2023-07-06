using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameComponents.Model
{
    public class MovePlayerDto
    {
        public int PointOfInterest { get; set; }
        public TicketTypeEnum TicketType { get; set; }

        public bool IsDoubleTicket { get; set; }

        [JsonConstructor]
        public MovePlayerDto() { 
        }
        public MovePlayerDto(int poi, TicketTypeEnum ticket)
        {
            PointOfInterest = poi;
            TicketType = ticket;
            IsDoubleTicket = false;
        }

        public MovePlayerDto(int pointOfInterest, TicketTypeEnum ticket, bool isDoubleTicket)
        {
            PointOfInterest = pointOfInterest;
            TicketType = ticket;
            IsDoubleTicket = isDoubleTicket;
        }
    }
}
