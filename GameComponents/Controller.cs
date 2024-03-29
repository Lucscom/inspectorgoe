﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    /// <summary>
    /// Controlls the game and validates the game state
    /// </summary>
    public class Controller
    {

        #region Singleton
        private Controller() { }

        private static Controller _instance;
        /// <summary>
        /// Creates one instance of controller or returns it
        /// </summary>
        /// <returns>Unique controller instance</returns>
        public static Controller GetInstance() { return _instance ??= new Controller(); }
        #endregion

        private GameState _gameState;

        /// <summary>
        /// Creating get methods for gamestate properties over controller
        /// </summary>
        public IReadOnlyList<PointOfInterest> PointsOfInterest => _gameState.PointsOfInterest;
        public IReadOnlyList<Player> Detectives => _gameState.Detectives;
        public Player MisterX => _gameState.MisterX;
        public int Round => _gameState.Round;
        public Player ActivePlayer => _gameState.ActivePlayer;


        /// <summary>
        /// Initializes the game state with given number of players
        /// </summary>
        /// <param name="playerNumber">number of players</param>
        public void Initialize(int playerNumber) 
        {
            if (playerNumber < 2 || playerNumber > 6)
            {
                throw new Exception("Invalid number of players");
            }

            _gameState = new GameState();

            InitPois();
            if (playerNumber > PointsOfInterest.Count)
            {
                throw new Exception("Not enough Pois");
            }
            InitPlayers(playerNumber);

            _gameState.ActivePlayer = _gameState.MisterX;
        }
        /// <summary>
        /// Initializes points of interest from game config
        /// </summary>
        private void InitPois()
        {
            //TODO: read JSON
            int numberOfPois = 5;

            for (int i = 0; i < numberOfPois; i++)
            {
                _gameState.PointsOfInterest.Add(new PointOfInterest(i, new Vector2()));
            }

            ConnectPois(_gameState.PointsOfInterest[0], _gameState.PointsOfInterest[1], TicketTypeEnum.Bus);
            ConnectPois(_gameState.PointsOfInterest[1], _gameState.PointsOfInterest[2], TicketTypeEnum.Bus);
            ConnectPois(_gameState.PointsOfInterest[3], _gameState.PointsOfInterest[4], TicketTypeEnum.Bus);
            ConnectPois(_gameState.PointsOfInterest[4], _gameState.PointsOfInterest[0], TicketTypeEnum.Bus);
            
            ConnectPois(_gameState.PointsOfInterest[4], _gameState.PointsOfInterest[2], TicketTypeEnum.Bike);
        }

        /// <summary>
        /// Creates the connection between the points of interest
        /// </summary>
        /// <param name="pointOfInterest1">First point of connection</param>
        /// <param name="pointOfInterest2">Second point of connection</param>
        /// <param name="ticketType">Type of connection</param>
        private void ConnectPois(PointOfInterest pointOfInterest1, PointOfInterest pointOfInterest2, TicketTypeEnum ticketType)
        {
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    pointOfInterest1.ConnectionBus.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionBus.Add(pointOfInterest1);
                    break;
                case TicketTypeEnum.Bike:
                    pointOfInterest1.ConnectionBike.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionBike.Add(pointOfInterest1);
                    break;
                case TicketTypeEnum.Scooter:
                    pointOfInterest1.ConnectionScooter.Add(pointOfInterest2);
                    pointOfInterest2.ConnectionScooter.Add(pointOfInterest1);
                    break;
            }
        }

        /// <summary>
        /// Initializes players with start positions
        /// Checks if the start position is blocked
        /// </summary>
        /// <param name="numberOfPlayers">Number of players</param>
        private void InitPlayers(int numberOfPlayers) 
        {
            //Array with random unique numbers
            //Set starting points
            var positions = Enumerable.Repeat<int>(-1, numberOfPlayers).ToArray();

            for (int i = 0; i < positions.Length; i++)
            {
                var newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                while (positions.Contains(newNumber))
                {
                    newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                }
                positions[i] = newNumber;
            }

            //Create players and set position
            _gameState.Detectives.Clear();
            for (int i = 0; i <= numberOfPlayers - 2; i++)
            {
                var startPosition = _gameState.PointsOfInterest[positions[i]];
                var newPlayer = new Player(startPosition);
                _gameState.Detectives.Add(newPlayer);
            }
            _gameState.MisterX = new Player(_gameState.PointsOfInterest[positions[numberOfPlayers-1]]);
        }

        /// <summary>
        /// Checks if given point is blocked by other detective, ignores misterX
        /// </summary>
        /// <param name="point">Point of interest to check</param>
        /// <returns>true if point is blocked</returns>
        private bool PoiBlockedByDetective(PointOfInterest point)
        {
            //check if another player is on the field
            foreach (var p in _gameState.Detectives)
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
        private bool ValidateMove(Player player, PointOfInterest point, TicketTypeEnum ticketType)
        {
            if (PoiBlockedByDetective(point)) 
            { 
                return false;
            }

            //check for connection between points
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    if (player.BusTicket > 0) //check for valid ticket
                    {
                        if (!player.Position.ConnectionBus.Contains(point))
                            return false;
                    }
                    else
                        return false;
                    break;
                case TicketTypeEnum.Bike:
                    if (player.BikeTicket > 0)
                    {
                        if (!player.Position.ConnectionBike.Contains(point))
                            return false;
                    }
                    else
                        return false;
                    break;
                case TicketTypeEnum.Scooter:
                    if (player.ScooterTicket > 0)
                    {
                        if (!player.Position.ConnectionScooter.Contains(point))
                            return false;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
            }


            return true;
        }

        /// <summary>
        /// Checks and excecutes move
        /// </summary>
        /// <param name="player">Player to move</param>
        /// <param name="newPosition">Destination of move</param>
        /// <param name="ticketType">Chosen ticketype</param>
        /// <exception cref="Exception">Game over</exception>
        public void MovePlayer(Player player, PointOfInterest newPosition, TicketTypeEnum ticketType)
        {
            if (player != ActivePlayer) return;

            if (ValidateMove(player, newPosition, ticketType))
            {
                player.Position = newPosition;

                switch (ticketType)
                {
                    case TicketTypeEnum.Bus:
                        player.BusTicket = -1;
                        break;
                    case TicketTypeEnum.Bike:
                        player.BikeTicket = -1;
                        break;
                    case TicketTypeEnum.Scooter:
                        player.ScooterTicket = -1;
                        break;
                    default:
                        break;
                }

                if (player != MisterX && FoundMisterX(player))
                        throw new Exception("Game OVER");
            }
        }

        /// <summary>
        /// Checks if player position equals misterX position
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool FoundMisterX(Player player)
        {
            if (_gameState.MisterX.Position.Equals(player.Position)) return true;
            else return false;
        }
    }
}
