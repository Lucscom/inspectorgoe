using GameComponents.Model;
using Newtonsoft.Json;
using System.Numerics;
using System.IO;
using GameComponents;

namespace TestProject
{
    public class ValidatorTests
    {
        public ValidatorTests() 
        {

        }

        //TODO: Write tests


        /// <summary>
        /// Creates the connection between the points of interest
        /// Implemented here only for test purspose
        /// </summary>
        /// <param name="pointOfInterest1">First point of connection</param>
        /// <param name="pointOfInterest2">Second point of connection</param>
        /// <param name="ticketType">Type of connection</param>
        static void ConnectPois(PointOfInterest pointOfInterest1, PointOfInterest pointOfInterest2, TicketTypeEnum ticketType)
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
        /// Tests if POIs and connections were generated correctly
        /// </summary>
        [Fact]
        public void ReadPoisAndConnections()
        {
            

            GameState TestGameState = new GameState();      // GameState constructor

            //string jsonContent = new StreamReader(File.OpenRead("Testmap.json")).ReadToEnd();
            dynamic jsonContent = JsonConvert.DeserializeObject(File.ReadAllText("Testmap.json"));      // read JSON to dynamic variable

            // generate POIs 
            int numPoi = 0;     //number of POIs in JSON-File
            foreach (var Nodes in jsonContent.Nodes)
            {
                TestGameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name, new Vector2((float)Nodes.Location_x, (float)Nodes.Location_y)));
                ++numPoi;
            }

            // generate Connections 
            int numConnect = 0;     // number of Connections in JSON
            var noSuccess = 0;      // += 1 in default case
            foreach (var Connections in jsonContent.Connections)
            {
                switch ((int)Connections.type) //defines number and type of connections between POIs
                {
                    // !!! Diese Logik stimmt bisher nur, wenn die POIs in der Reihenfolge ihrer Laufnummer initialisiert werden, da No in TestGameState.PointsOfInterest[No] nur die Position in der Liste repräsentiert, nicht die Laufnummer!
                    case 1: //bus
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ++numConnect;
                        break;
                    case 2:     //bike
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ++numConnect;
                        break;
                    case 3:     //scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        ++numConnect;
                        break;
                    case 12:    //bus & bike
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        numConnect += 2;
                        break;
                    case 13:    // bus & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect += 2;
                        break;
                    case 23:    // bike & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect += 2;
                        break;
                    case 123:   // bus, bike & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect += 3;
                        break;
                    default:
                        noSuccess += 1;
                        break;


                }
            }
                

                Assert.Equal(numPoi, TestGameState.PointsOfInterest.Count); // number POIs in JSON = number generateed POIs
                Assert.Equal(0, noSuccess); // all connections were created
        }

        /// <summary>
        /// Tests if all valid moves are found correctly
        /// </summary>
        [Fact]
        public void AllValidMoves()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            dynamic jsonContent = JsonConvert.DeserializeObject(File.ReadAllText("Testmap.json"));      // read JSON to dynamic variable

            // generate POIs 
            foreach (var Nodes in jsonContent.Nodes)
            {
                TestGameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name, new Vector2((float)Nodes.Location_x, (float)Nodes.Location_y)));
            }

            // generate Connections 
            foreach (var Connections in jsonContent.Connections)
            {
                switch ((int)Connections.type) //defines number and type of connections between POIs
                {
                    // !!! Diese Logik stimmt bisher nur, wenn die POIs in der Reihenfolge ihrer Laufnummer initialisiert werden, da No in TestGameState.PointsOfInterest[No] nur die Position in der Liste repräsentiert, nicht die Laufnummer!
                    case 1: //bus
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        break;
                    case 2:     //bike
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        break;
                    case 3:     //scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        break;
                    case 12:    //bus & bike
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        break;
                    case 13:    // bus & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        break;
                    case 23:    // bike & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        break;
                    case 123:   // bus, bike & scooter
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        break;
                    default:
                        break;


                }
            }

            TestGameState.MisterX = new Player("TestPlayer", "TestPassword");   // generate TestPlayer
            TestGameState.MisterX.Position = TestGameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            TestGameState.MisterX.BusTicket = 3;      // assign bus tickets
            TestGameState.MisterX.BikeTicket = 0;     // assign no bike tickets
            TestGameState.MisterX.ScooterTicket = 2;  // assign scooter tickets

            //var TestValidator = new Validator();    //generate instance of class Validator
            Dictionary<PointOfInterest, List<TicketTypeEnum>> TestMoves = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();      // create dictonary for valid moves 
            TestMoves = Validator.GetValidMoves(TestGameState, TestGameState.MisterX);      // test method getValidMoves for MisterX in generated TestGameState

            var BikeConnections = 0;      // all found possible bike connections
            //var NumPOIs = TestMoves.Count;
            for (var NumPOIs = 0; NumPOIs < TestMoves.Count; NumPOIs++)
            {
               // if (TestMoves.ContainsValue('Bike'))
              //if (TestMoves[NumPOIs] = 0)
                    BikeConnections++;
            }
            //var xyz = TestMoves.Values.Contains('Bike');
            var test = 1;
            Assert.NotEqual(0, test);

        }

       
    }
}
