using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    internal class Controller
    {

        #region Singleton
        private Controller() { }

        private static Controller _instance;
        public static Controller GetInstance() { return _instance ??= new Controller(); }
        #endregion

        private GameState _gameState;

        public void Initialize(int playerNumber) 
        {
            _gameState = new GameState();

            InitPois();
            InitPlayers(playerNumber);

            _gameState.ActivePlayer = _gameState.MisterX;
        }

        public void InitPois()
        {
            //TODO: read JSON
            int numberOfPois = 5;

            for (int i = 0; i < numberOfPois; i++)
            {
                _gameState.PointsOfInterest.Add(new PointOfInterest(i, new Point()));
            }

            connectPOIs(_gameState.PointsOfInterest[0], _gameState.PointsOfInterest[1], TicketTypeEnum.Bus);
            connectPOIs(_gameState.PointsOfInterest[1], _gameState.PointsOfInterest[2], TicketTypeEnum.Bus);
            connectPOIs(_gameState.PointsOfInterest[3], _gameState.PointsOfInterest[4], TicketTypeEnum.Bus);
            connectPOIs(_gameState.PointsOfInterest[4], _gameState.PointsOfInterest[0], TicketTypeEnum.Bus);
            
            connectPOIs(_gameState.PointsOfInterest[4], _gameState.PointsOfInterest[2], TicketTypeEnum.Bike);
        }
        
        private void connectPOIs(PointOfInterest pointOfInterest1, PointOfInterest pointOfInterest2, TicketTypeEnum ticketType)
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


        public void InitPlayers(int numberOfPlayers) 
        {
            int[] positions = new int[numberOfPlayers];
            for (int i = 0; i < positions.Length; i++)
            {
                var newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                while (positions.Contains(newNumber))
                {
                    newNumber = Random.Shared.Next(0, _gameState.PointsOfInterest.Count - 1);
                }
                positions[i] = newNumber;
            }


            _gameState.Detectives.Clear();
            for (int i = 0; i < numberOfPlayers - 2; i++)
            {
                _gameState.Detectives.Add(new Player(_gameState.PointsOfInterest[positions[i]]));
            }
            _gameState.MisterX = new Player(_gameState.PointsOfInterest[positions[numberOfPlayers-1]]);
        }


        private bool CheckForOtherDetectives(PointOfInterest point)
        {
            //check if another player is on the field
            //TODO: check if misterx is on field? --> Game Over
            foreach (var p in _gameState.Detectives)
            {
                if (p.Position == point)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateMove(Player player, PointOfInterest point, TicketTypeEnum ticketType)
        {
            if (!CheckForOtherDetectives(point)) 
            { 
                return false;
            }

            //check for connection between points
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    if (player.BusTicket > 0) //check for valid ticket
                        if (!player.Position.ConnectionBus.Contains(point)) 
                            return false; 
                    else 
                        return false;
                    break;
                case TicketTypeEnum.Bike:
                    if (player.BikeTicket > 0)
                        if (!player.Position.ConnectionBike.Contains(point))
                            return false;
                        else
                            return false;
                    break;
                case TicketTypeEnum.Scooter:
                    if (player.ScooterTicket > 0)
                        if (!player.Position.ConnectionScooter.Contains(point))
                            return false;
                        else
                            return false;
                    break;
                default:
                    break;
            }


            return true;
        }

        public void MovePlayer(Player player, PointOfInterest newPosition, TicketTypeEnum ticketType)
        {
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

                if (FoundMisterX(player))
                    throw new Exception("Game OVER");
            }
        }

        private bool FoundMisterX(Player player)
        {
            if (_gameState.MisterX.Position.Equals(player.Position)) return true;
            else return false;
        }
    }
}
