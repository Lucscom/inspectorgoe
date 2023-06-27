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
        /// Tests if POIs and connections read from a JSON-Testmap were generated correctly
        /// </summary>
        [Fact]
        public void ReadPoisAndConnections()
        {
            GameState TestGameState = new GameState();      // GameState constructor
    
            Initializer.InitPois(TestGameState);        // generate POIs and Connections according to Testmap.JSON

            // Test with test map:
            Assert.Equal(12, TestGameState.PointsOfInterest.Count);      // check if right number of POIs was created


            // check number of connections for each ticket type testwise for one POI (here Matheinstitut, Number 11)
            Assert.Equal(2, TestGameState.PointsOfInterest[10].ConnectionBus.Count());      // Bus connections should be 3
            Assert.Equal(3, TestGameState.PointsOfInterest[10].ConnectionBike.Count());     // Bike connections should be 1
            Assert.Equal(2, TestGameState.PointsOfInterest[10].ConnectionScooter.Count());  // Scooter connections should be 1

            // Test with complete map:
            /*Assert.Equal(128, TestGameState.PointsOfInterest.Count);      // check if right number of POIs was created
            // check number of connections for each ticket type testwise for one POI (here Sartorius, Number 13)
            Assert.Equal(3, TestGameState.PointsOfInterest[12].ConnectionBus.Count());      // Bus connections should be 2
            Assert.Equal(1, TestGameState.PointsOfInterest[12].ConnectionBike.Count());     // Bike connections should be 3
            Assert.Equal(1, TestGameState.PointsOfInterest[12].ConnectionScooter.Count());  // Scooter connections should be 2*/

        }

        /// <summary>
        /// Tests if all valid moves are found correctly
        /// </summary>
        [Fact]
        public void AllValidMoves()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            Initializer.InitPois(TestGameState);        // generate POIs and Connections

            TestGameState.MisterX = new Player("TestPlayer", "TestPassword");   // generate TestPlayer
            TestGameState.MisterX.Position = TestGameState.PointsOfInterest.First(); // assign Startposition for TestPlayer (here 1: Gänseliesel in Testmap/ Pagoda in complete map)
            TestGameState.MisterX.BusTicket = 3;      // assign bus tickets
            TestGameState.MisterX.BikeTicket = 0;     // assign no bike tickets
            TestGameState.MisterX.ScooterTicket = 2;  // assign scooter tickets
            

            Dictionary<PointOfInterest, List<TicketTypeEnum>> TestMoves = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();      // create dictonary for valid moves 
            TestMoves = Validator.GetValidMoves(TestGameState, TestGameState.MisterX);      // test method getValidMoves for MisterX in generated TestGameState

            
            Dictionary<PointOfInterest, List<TicketTypeEnum>> TestMovesCheck = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();      // create dictonary with known valid moves for comparison
            // Test with test map:
            TestMovesCheck.Add(TestGameState.PointsOfInterest.ElementAt(1), new List<TicketTypeEnum> { TicketTypeEnum.Scooter });      // hardcoding from testmap (no Bike connection here, cause no ticket)
            TestMovesCheck.Add(TestGameState.PointsOfInterest.ElementAt(4), new List<TicketTypeEnum> { TicketTypeEnum.Scooter });
            TestMovesCheck.Add(TestGameState.PointsOfInterest.ElementAt(8), new List<TicketTypeEnum> { TicketTypeEnum.Scooter });
            
            // Test with complete map:
            /*TestMovesCheck.Add(TestGameState.PointsOfInterest.ElementAt(1), new List<TicketTypeEnum> { TicketTypeEnum.Scooter });      // hardcoding from complete map (no Bike connection here, cause no ticket)
            TestMovesCheck.Add(TestGameState.PointsOfInterest.ElementAt(3), new List<TicketTypeEnum> { TicketTypeEnum.Scooter });*/
            
            Assert.Equal(TestMovesCheck, TestMoves);
        }


    }
}
