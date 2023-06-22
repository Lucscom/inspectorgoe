using GameComponents;
using GameComponents.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
            
        }

        /// <summary>
        /// tests if controller denies move, when no tickets availible
        /// </summary>
        [Fact]
        public void NoTicketsNoMovement()
        {
            GameState TestGameState = new GameState();      // GameState constructor
            Initializer.InitPois(TestGameState);        // generate POIs and Connections

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].Position = TestGameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            TestGameState.Detectives[0].BusTicket = 0;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 0;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 0;  // assign scooter tickets

            // test MovePlayer method from GameController : should not move, cause no tickets
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller
           
            TestGameController.StartGame();     // Start Game is requirement for MovePlayer
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer

            // choose random destination, which has a connection of type scooter
            PointOfInterest newPos = TestGameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameState.Detectives[0].Position.ConnectionScooter.Count - 1)];

            TestGameController.MovePlayer(TestGameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer
            
            Assert.NotEqual(TestGameState.Detectives[0].Position, newPos);

        }
        /// <summary>
        /// tests if controller executes a valid move
        /// </summary>
        [Fact]
        public void TicketMovement()
        {
            GameState TestGameState = new GameState();      // GameState constructor
            Initializer.InitPois(TestGameState);        // generate POIs and Connections

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets

            // test MovePlayer method from GameController : should move, cause tickets availible
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller
                        
            TestGameController.StartGame();     // Start Game is requirement for MovePlayer
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            // choose random destination, which has a connection of type scooter
            PointOfInterest newPos = TestGameController.GameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameController.GameState.Detectives[0].Position.ConnectionScooter.Count - 1)];
            TestGameController.GameState.ActivePlayer = TestGameController.GameState.Detectives[0];     // assign Detective[1] as active Player
            TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.Equal(TestGameController.GameState.Detectives[0].Position, newPos);
        }

        /// <summary>
        /// tests, if only the active player is able to move
        /// </summary>
        [Fact]
        public void DontMoveInactivePlayer() 
        {
            GameState TestGameState = new GameState();      // GameState constructor
            Initializer.InitPois(TestGameState);        // generate POIs and Connections

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets

            // test MovePlayer method from GameController : should not move, cause detective in not active player
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller

            TestGameController.StartGame();     // Start Game is requirement for MovePlayer
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            // choose random destination, which has a connection of type scooter
            PointOfInterest newPos = TestGameController.GameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameController.GameState.Detectives[0].Position.ConnectionScooter.Count - 1)];
            TestGameController.GameState.ActivePlayer = TestGameController.GameState.MisterX;   // assign MisterX (not Detective[1]) as active Player

            TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.NotEqual(TestGameController.GameState.Detectives[0].Position, newPos);
        }
    }   
}