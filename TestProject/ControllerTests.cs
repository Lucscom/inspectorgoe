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
            //todo: change json location
            string jsonContent = new StreamReader(File.OpenRead("Testmap.json")).ReadToEnd();      //read JSON file from project folder
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

            // testen der GameController MovePlayer method: sollte den move nicht ausführen, da keine Scooter Tickets
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller
           
            TestGameController.StartGame();     // Start Game is requirement for MovePlayer
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer

            // zufällige Zielposition wählen, zu der eine Verbindung mit Scooter existiert
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

            // testen der GameController MovePlayer method: sollte den move ausführen, da Scooter Tickets vorhanden
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller
                        
            TestGameController.StartGame();     // Start Game is requirement for MovePlayer
            TestGameController.GameState.Detectives[0].Position = TestGameController.GameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            // zufällige Zielposition wählen, zu der eine Verbindung mit Scooter existiert
            PointOfInterest newPos = TestGameController.GameState.Detectives[0].Position.ConnectionScooter[Random.Shared.Next(0, TestGameController.GameState.Detectives[0].Position.ConnectionScooter.Count - 1)];

            TestGameController.MovePlayer(TestGameController.GameState.Detectives[0], newPos, TicketTypeEnum.Scooter);     // execute MovePlayer

            Assert.Equal(TestGameController.GameState.Detectives[0].Position, newPos);
        }

        /// <summary>
        /// hier wird auch getestet, ob nur der aktive spieler seinen Zug durchführen darf
        /// </summary>
        [Fact]
        public void DontMoveInactivePlayer() 
        {
        /*
            var controller = Controller.GetInstance();
            controller.Initialize(2);
            var player = controller.Detectives.First();

            
            var newPos = player.Position;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgewählt um einen validen zug zu garantieren
            // Es wird nicht garantiert, dass ein anderer Spieler auf diesem POI steht!
            if (player.Position.ConnectionBus.Count != 0)               // Busverbindung am Start-POI existiert
            {
                newPos = player.Position.ConnectionBus.First();             // Neue Position ist erste aus der Liste der Busverbindungen
                player.BusTicket = 5;                                       
                controller.MovePlayer(player, newPos, TicketTypeEnum.Bus);  // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
            else if (player.Position.ConnectionBike.Count != 0)         //Bikeverbindung am Start-POI existiert
            {
                newPos = player.Position.ConnectionBike.First();            // Neue Position ist erste aus der Liste der Bikeverbindungen
                player.BikeTicket = 5;
                controller.MovePlayer(player, newPos, TicketTypeEnum.Bike); // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
            else if (player.Position.ConnectionScooter.Count != 0)      //Scooterverbindung am Start-POI existiert
            {
                newPos = player.Position.ConnectionScooter.First();         // Neue Position ist erste aus der Liste der Scooterverbindungen
                player.ScooterTicket = 5;
                controller.MovePlayer(player, newPos, TicketTypeEnum.Scooter);// übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
            
            Assert.False(player.Position == newPos);*/
        }
    }   
}