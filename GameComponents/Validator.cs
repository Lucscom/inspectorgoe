﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameComponents.Model;


namespace GameComponents
{
    /// <summary>
    /// Controlls the game and validates the game state
    /// </summary>
    public class Validator
    {

        /// <summary>
        /// Checks if given point is blocked by other detective, ignores misterX
        /// </summary>
        /// <param name="point">Point of interest to check</param>
        /// <returns>true if point is blocked</returns>
        private bool PoiBlockedByDetective(PointOfInterest point, List<Player> detectives)
        {
            //check if another player is on the field
            foreach (var p in detectives)
            {
                if (p.Position == point)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if given move is possible for given player
        /// </summary>
        /// <param name="player">Player to check move</param>
        /// <param name="point">Destination of move</param>
        /// <param name="ticketType">Chosen ticketype</param>
        /// <returns>true if move is possible</returns>
        private bool ValidateMove(Player player, PointOfInterest point, TicketTypeEnum ticketType, List<Player> detectives)
        {
            if (PoiBlockedByDetective(point, detectives)) 
            {
                //throw new Exception("POI blocked by Detective!");
                return false;
            }

            //check for connection between points
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    if (player.BusTicket > 0) //check for valid ticket
                    {
                        if (!player.Position.ConnectionBus.Contains(point))
                            //throw new Exception("Connection Bus doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Bus Tickets!");
                        return false;
                    break;
                case TicketTypeEnum.Bike:
                    if (player.BikeTicket > 0)
                    {
                        if (!player.Position.ConnectionBike.Contains(point))
                            //throw new Exception("Connection Bike doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Bike Tickets!");
                        return false;
                    break;
                case TicketTypeEnum.Scooter:
                    if (player.ScooterTicket > 0)
                    {
                        if (!player.Position.ConnectionScooter.Contains(point))
                            //throw new Exception("Connection Scooter doesn't exist!");
                            return false;
                    }
                    else
                        //throw new Exception("No Scooter Tickets!");
                        return false;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}