using GameComponents.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    public static class Initializer
    {
        /// <summary>
        /// Initializes points of interest from game config
        /// </summary>
        public static GameState InitPois(GameState GameState)
        {
            //read POIs from JSON file Testmap in project folder 
            string jsonContent = new StreamReader(File.OpenRead("poimap.json")).ReadToEnd();
            dynamic json = JsonConvert.DeserializeObject(jsonContent);  //read JSON to dynamic variable

            // generate POIs 
            int numPoi = 0;     //number of POIs in JSON-File
            foreach (var Nodes in json.Nodes)
            {
                GameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name, (int)Nodes.Location_x, (int)Nodes.Location_y));
                ++numPoi;
            }

            // generate Connections 
            int numConnect = 0;     //total number of Connections 
            var noSuccess = 0;      //  =1 in default case
            foreach (var Connections in json.Connections)
            {
                switch ((int)Connections.type) //defines number and type of connections between POIs
                {
                    // !!! Diese Logik stimmt bisher nur, wenn die POIs in der Reihenfolge ihrer Laufnummer initialisiert werden, da No in _gameState.PointsOfInterest[No] nur die Position in der Liste repräsentiert, nicht die Laufnummer!
                    case 1:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ++numConnect;
                        break;
                    case 2:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ++numConnect;
                        break;
                    case 3:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        ++numConnect;
                        break;
                    case 12:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        numConnect = numConnect + 2;
                        break;
                    case 13:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 23:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 123:
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(GameState.PointsOfInterest[(int)Connections.sourceNo - 1], GameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 3;
                        break;
                    default:
                        noSuccess = 1;
                        break;
                }
            }
            return GameState;
        }

        /// <summary>
        /// Creates the connection between the points of interest
        /// </summary>
        /// <param name="pointOfInterest1">First point of connection</param>
        /// <param name="pointOfInterest2">Second point of connection</param>
        /// <param name="ticketType">Type of connection</param>
        private static void ConnectPois(PointOfInterest pointOfInterest1, PointOfInterest pointOfInterest2, TicketTypeEnum ticketType)
        {
            switch (ticketType)
            {
                case TicketTypeEnum.Bus:
                    pointOfInterest1.ConnectionBus.Add(pointOfInterest2.Number);
                    pointOfInterest2.ConnectionBus.Add(pointOfInterest1.Number);
                    break;
                case TicketTypeEnum.Bike:
                    pointOfInterest1.ConnectionBike.Add(pointOfInterest2.Number);
                    pointOfInterest2.ConnectionBike.Add(pointOfInterest1.Number);
                    break;
                case TicketTypeEnum.Scooter:
                    pointOfInterest1.ConnectionScooter.Add(pointOfInterest2.Number);
                    pointOfInterest2.ConnectionScooter.Add(pointOfInterest1.Number);
                    break;
            }
        }

        /// <summary>
        /// Initializes players with start positions
        /// Checks if the start position is blocked
        /// </summary>
        /// <param name="numberOfPlayers">Number of players</param>
        public static GameState InitPlayers(GameState GameState, int numberOfPlayers)
        {
            //Array with random unique numbers
            //Set starting points
            var positions = Enumerable.Repeat<int>(-1, numberOfPlayers).ToArray();

            for (int i = 0; i < positions.Length; i++)
            {
                var newNumber = Random.Shared.Next(0, GameState.PointsOfInterest.Count - 1);
                while (positions.Contains(newNumber))
                {
                    newNumber = Random.Shared.Next(0, GameState.PointsOfInterest.Count - 1);
                }
                positions[i] = newNumber;
            }

            //Create players and set position
            for (int i = 0; i <= numberOfPlayers - 2; i++)
            {
                var startPosition = GameState.PointsOfInterest[positions[i]];
                GameState.Detectives[i].Position = startPosition;
            }
            GameState.MisterX.Position = GameState.PointsOfInterest[positions[numberOfPlayers - 1]];

            return GameState;
        }
    }
}
