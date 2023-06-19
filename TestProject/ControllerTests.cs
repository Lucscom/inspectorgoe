using GameComponents;
using GameComponents.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
            //todo: change json location
            //string jsonContent = new StreamReader(File.OpenRead("Testmap.json")).ReadToEnd();      //read JSON file from project folder
        }

        /// <summary>
        /// testen ob controller die bewegung verweigert, wenn keinen tickets vorhanden sind
        /// </summary>
        [Fact]
        public void NoTicketsNoMovement()
        {
            GameState TestGameState = new GameState();      // GameState constructor
            //dynamic jsonContent = new StreamReader(File.OpenRead("Testmap.json")).ReadToEnd();

            dynamic jsonContent = JsonConvert.DeserializeObject(File.ReadAllText("Testmap.json"));
            // generate POIs 
            int numPoi = 0;     //number of POIs in JSON-File
            foreach (var Nodes in jsonContent.Nodes)
            {
                TestGameState.PointsOfInterest.Add(new PointOfInterest((int)Nodes.Number, (string)Nodes.Name, new Vector2((float)Nodes.Location_x, (float)Nodes.Location_y)));
                ++numPoi;
            }


            // generate Connections 
            int numConnect = 0;     //total number of Connections 
            /*var noSuccess = 0;      //  =1 in default case
            foreach (var Connections in jsonContent.Connections)
            {
                switch ((int)Connections.type) //defines number and type of connections between POIs
                {
                    // !!! Diese Logik stimmt bisher nur, wenn die POIs in der Reihenfolge ihrer Laufnummer initialisiert werden, da No in _gameState.PointsOfInterest[No] nur die Position in der Liste repräsentiert, nicht die Laufnummer!
                    case 1:
                        ConnectPois(TestGameState.PointsOfInterest[(int)Connections.sourceNo - 1], TestGameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ++numConnect;
                        break;
                    case 2:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ++numConnect;
                        break;
                    case 3:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        ++numConnect;
                        break;
                    case 12:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        numConnect = numConnect + 2;
                        break;
                    case 13:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 23:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 2;
                        break;
                    case 123:
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bus);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Bike);
                        ConnectPois(_gameState.PointsOfInterest[(int)Connections.sourceNo - 1], _gameState.PointsOfInterest[(int)Connections.targetNo - 1], TicketTypeEnum.Scooter);
                        numConnect = numConnect + 3;
                        break;
                    default:
                        noSuccess = 1;
                        break;

                        // controller instanzieren & spieleranzahl festlegen
                        /*var controller = Controller.GetInstance();
                        controller.Initialize(4);

                        var pois = controller.PointsOfInterest;
                        var X = controller.MisterX;

                        /* dürfte theoretisch nicht gehen! muss durch den Controller selbst initiiert werden und müsste automatisch bei bewegung
                         * durch den controller basierend auf der bewegungsart abgezogen werden um manipulation zu vermeiden
                         * 
                         * todo: hatten wir jetzt aber keinen bock dazu xD ticket muss noch geschrieben werden */
                        // Geht halt trotzdem, sollte uns nicht aufhalten
                        /*X.ScooterTicket = 0;
                        X.BikeTicket = 0;
                        X.BusTicket = 0;

                        // zufälllige Zielposition für den Zug festlegen
                        PointOfInterest newPos = pois.First();

                        // testen ob Zielposition ungleich aktueller position
                        while (X.Position == newPos)
                        {
                            newPos = pois[Random.Shared.Next(0, pois.Count - 1)];
                        }

                        // testen der controller MovePlayer method
                        controller.MovePlayer(X, newPos, TicketTypeEnum.Scooter);

                        Assert.NotEqual(X.Position, newPos);*/
                }

        /// <summary>
        /// testen ob der controller einen validen Zug zulässt
        /// </summary>
        [Fact]
        public void TicketMovement()
        {
            /*var controller = Controller.GetInstance();
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
                             
            Assert.Equal(X.Position, newPos); */     // Klappt nicht immer (aber häufig), wenn durch Zufall ein Detective auf dem Feld steht.
        }

        /// <summary>
        /// hier wird auch getestet, ob nur der aktive spieler seinen Zug durchführen darf
        /// </summary>
        [Fact]
        public void DontMoveInactivePlayer() 
        {

            /*var controller = Controller.GetInstance();
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