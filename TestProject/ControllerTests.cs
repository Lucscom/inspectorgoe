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
            TestGameState.Detectives[0].Position = TestGameState.PointsOfInterest.First(); // assign Startposition for TestPlayer
            TestGameState.Detectives[0].BusTicket = 0;      // assign bus tickets
            TestGameState.Detectives[0].BikeTicket = 0;     // assign bike tickets
            TestGameState.Detectives[0].ScooterTicket = 0;  // assign scooter tickets

            // zufälllige Zielposition für den Zug festlegen
            //PointOfInterest newPos = TestGameState.PointsOfInterest.First();

            // Zielposition wählen, zu der eine Verbindung mit Bike existiert
            PointOfInterest newPos = TestGameState.Detectives[0].Position.ConnectionBike[Random.Shared.Next(0, TestGameState.Detectives[0].Position.ConnectionBike.Count - 1)];

            // testen ob Zielposition ungleich aktueller position  //wird aktuell nicht mehr benötigt, da die gleich Position ausgeschlossen 
            /*while (TestGameState.Detectives[0].Position == newPos)
            {
                newPos = TestGameState.PointsOfInterest[Random.Shared.Next(0, TestGameState.PointsOfInterest.Count - 1)];
            }*/

            // testen der GameController MovePlayer method
            GameController TestGameController = new GameController(TestGameState);      // generate Gamecontroller
            TestGameController.AddPlayer(TestGameState.Detectives[0]);      // add Player to GameController

            TestGameController.MovePlayer(TestGameState.Detectives[0], newPos, TicketTypeEnum.Scooter);
            
            Assert.NotEqual(TestGameState.MisterX.Position, newPos);

        }
        /// <summary>
        /// testen ob der controller einen validen Zug zulässt
        /// </summary>
        [Fact]
        public void TicketMovement()
        {
        /*
            var controller = Controller.GetInstance();
            controller.Initialize(2);

            var X = controller.MisterX;

            X.ScooterTicket = 5;
            X.BikeTicket = 5;
            X.BusTicket = 5;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgewählt um einen validen zug zu garantieren
            // Es wird nicht garantiert, dass ein anderer Spieler auf diesem POI steht!
            var newPos = X.Position;

            if (X.Position.ConnectionBus.Count != 0)                    // Busverbindung am Start-POI existiert
            {
                newPos = X.Position.ConnectionBus.First();                  // Neue Position ist erste aus der Liste der Busverbindungen
                controller.MovePlayer(X, newPos, TicketTypeEnum.Bus);       // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
            else if (X.Position.ConnectionBike.Count != 0)              //Bikeverbindung am Start-POI existiert
            {
                newPos = X.Position.ConnectionBike.First();                 // Neue Position ist erste aus der Liste der Bikeverbindungen
                controller.MovePlayer(X, newPos, TicketTypeEnum.Bike);      // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
            else if (X.Position.ConnectionScooter.Count != 0)           //Scooterverbindung am Start-POI existiert
            {
                newPos = X.Position.ConnectionScooter.First();              // Neue Position ist erste aus der Liste der Scooterverbindungen
                controller.MovePlayer(X, newPos, TicketTypeEnum.Scooter);   // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            }
                             
            Assert.Equal(X.Position, newPos);      // Klappt nicht immer (aber häufig), wenn durch Zufall ein Detective auf dem Feld steht.
        */
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