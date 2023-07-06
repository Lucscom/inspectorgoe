using GameComponents;
using GameComponents.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {

        }
        // all controller tests are run with the complete game map, not Testmap!

        /// <summary>
        /// tests if controller denies move, when no tickets availible
        /// </summary>
        [Fact]
        public void NoTicketsNoMovement()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].BusTicket = 0;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 0;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 0;  // assign scooter tickets
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)

            // test MovePlayer method from GameController : should not move, cause no tickets
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller

            TestGameController.StartGame();     // Start Game is requirement for MovePlayer, !calls InitPois() and InitPlayers()!, sets MisterX als active Player
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer

            // choose random destination, which has a connection of type scooter
            int newPos = TestGameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameState.Detectives[0].Position.ConnectionScooter.Count - 1)];

            TestGameController.MovePlayer(TestGameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.NotEqual(TestGameState.Detectives[0].Position.Number, newPos);

        }
        /// <summary>
        /// tests if controller executes a valid move
        /// </summary>
        [Fact]
        public void TicketMovement()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)


            // test MovePlayer method from GameController : should move, cause tickets availible
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller

            TestGameController.StartGame();     // Start Game is requirement for MovePlayer, !calls InitPois() and InitPlayers()!, sets MisterX als active Player
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            // choose random destination, which has a connection of type scooter
            int newPos = TestGameController.GameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameController.GameState.Detectives[0].Position.ConnectionScooter.Count - 1)];
            TestGameController.GameState.ActivePlayer = TestGameController.GameState.Detectives[0];     // assign Detective[0] as active Player
            TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.Equal(TestGameController.GameState.Detectives[0].Position.Number, newPos);
        }

        /// <summary>
        /// tests, if only the active player is able to move
        /// </summary>
        [Fact]
        public void DontMoveInactivePlayer()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)

            // test MovePlayer method from GameController : should not move, cause detective in not active player
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller

            TestGameController.StartGame();     // Start Game is requirement for MovePlayer; !calls InitPois() and InitPlayers()!, sets MisterX als active Player
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            // choose random destination, which has a connection of type scooter
            int newPos = TestGameController.GameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameController.GameState.Detectives[0].Position.ConnectionScooter.Count - 1)];
            TestGameController.GameState.ActivePlayer = TestGameController.GameState.MisterX;   // assign MisterX (not Detective[0]) as active Player

            TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.NotEqual(TestGameController.GameState.Detectives[0].Position.Number, newPos);
        }

        /// <summary>
        /// tests, if the automatic movement of an ai-controlled player works
        /// </summary>
        [Fact]
        public void AIMove()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.MisterX.Npc = true;         // make player ai-controlled
            //TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            //TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            //TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)

            // test AiMove method from GameController: 
            GameController TestGameController = new GameController(TestGameState);                      // generate Gamecontroller
            TestGameController.StartGame();                                                             // Start Game is requirement for MovePlayer, !calls InitPois() and InitPlayers()!, sets MisterX als active Player
            //TestGameController.GameState.ActivePlayer = TestGameController.GameState.Detectives[0];     // assign Detective[0] as active Player

            int oldPos = TestGameController.GameState.MisterX.Position.Number;   // store old position for comparision

            TestGameController.AiMove(TestGameController.GameState.MisterX);          // test moving ai-Detective

            Assert.NotEqual(TestGameController.GameState.MisterX.Position.Number, oldPos);
        }
        /// <summary>
        /// tests, if the game flow works with an active move an a automated move by an ai-controlled player
        /// </summary>
        [Fact]
        public void AiMoveInGame()
        {
            GameState TestGameState = new GameState();      // GameState constructor

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.Detectives[0].Npc = true;         // make player ai-controlled
            TestGameState.Detectives[0].BusTicket = 5;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 5;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 5;  // assign scooter tickets
            TestGameState.MisterX.BusTicket = 5;      // assign bus tickets
            TestGameState.MisterX.BikeTicket = 5;     // assign bike tickets
            TestGameState.MisterX.ScooterTicket = 5;  // assign scooter tickets
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)

            GameController TestGameController = new GameController(TestGameState);                      // generate Gamecontroller
            TestGameController.StartGame();   // Start Game is requirement for MovePlayer, !calls InitPois() and InitPlayers()!, sets MisterX als active Player

            int oldPosAi = TestGameController.GameState.Detectives[0].Position.Number;   // store old position of ai-player for comparision
            Dictionary<PointOfInterest, List<TicketTypeEnum>> TestMoves = Validator.GetValidMoves(TestGameController.GameState, TestGameController.GameState.MisterX); //get valid moves for MisterX

            // move MisterX; Detective[0] should move on his own, as he is the next active Player
            int randomNumber = Random.Shared.Next(0, TestMoves.Count - 1);
            int newPos = TestMoves.ElementAt(randomNumber).Key.Number;   //choose random (availible) new Position for MisterX
            TicketTypeEnum ticket = TestMoves.ElementAt(randomNumber).Value.ElementAt(Random.Shared.Next(0, TestMoves.ElementAt(randomNumber).Value.Count - 1));  //choose random (availible) ticketType to new Position
            TestGameController.MovePlayer(TestGameController.GameState.MisterX, newPos, ticket);    // execute move

            Assert.NotEqual(TestGameController.GameState.Detectives[0].Position.Number, oldPosAi);
        }
        /// <summary>
        /// tests, if the game ends, when MisterX is found or the round number reaches its limit
        /// </summary>
        [Fact]
        public void GameEnd()
        {
            GameState TestGameState = new GameState();      // generate a GameState

            TestGameState.Detectives.Add(new Player("TestPlayer", "TestPassword"));   // generate TestPlayer
            TestGameState.MisterX = (new Player("TestX", "TestPassword"));   // generate Mister X for tests (needed to Start game)
            TestGameState.GameCreator = TestGameState.MisterX; // assign GameCreater (requirement to start game!)

            GameController TestGameController = new GameController(TestGameState);      // generate a Gamecontroller
            TestGameController.StartGame();     // Start Game is requirement for MovePlayer, !calls InitPois() and InitPlayers()!, sets MisterX als active Player

            // Assign MisterX and Detective neighboured POIs; here: 1-4, tickettype = Scooter or Bike
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.ElementAt(3);
            TestGameController.GameState.MisterX.Position = TestGameController.GameState.PointsOfInterest.ElementAt(3);     // using move method to make detective[0] active player
            TestGameController.MovePlayer(TestGameController.GameState.MisterX, 1, TicketTypeEnum.Bike);    // move to POI 1

            //check if exception for MisterX found is thrown, when detective moves to MisterX position
            var exDetectiveWon = Assert.Throws<Exception>(() =>TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], TestGameController.GameState.MisterX.Position.Number, TicketTypeEnum.Bike));
            Assert.Equal("MisterX found!", exDetectiveWon.Message);

            TestGameController.GameState.Move=49;   //24 Rounds played
            //check if exception for MisterX found is thrown, when detective moves to MisterX position
            var exMisterXWon = Assert.Throws<Exception>(() => TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], 4, TicketTypeEnum.Scooter));   // try to move MisterX (active); should not works, cuase Round limit
            Assert.Equal("Round limit!", exMisterXWon.Message);

        }
    }
}