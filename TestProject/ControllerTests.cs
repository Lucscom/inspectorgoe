using GameComponents;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
        }

        /// <summary>
        /// testen ob controller die bewegung verweigert, wenn keinen tickets vorhanden sind
        /// </summary>
        [Fact]
        public void NoTicketsNoMovement()
        {
            // controller instanzieren & spieleranzahl festlegen
            var controller = Controller.GetInstance();
            controller.Initialize(4);

            var pois = controller.PointsOfInterest;
            var X = controller.MisterX;

            /* dürfte theoretisch nicht gehen! muss durch den Controller selbst initiiert werden und müsste automatisch bei bewegung
             * durch den controller basierend auf der bewegungsart abgezogen werden um manipulation zu vermeiden
             * 
             * todo: hatten wir jetzt aber keinen bock dazu xD ticket muss noch geschrieben werden */
            // Geht halt trotzdem, sollte uns nicht aufhalten
            X.ScooterTicket = 0;
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

            Assert.NotEqual(X.Position, newPos);
        }

        /// <summary>
        /// testen ob der controller einen validen Zug zulässt
        /// </summary>
        [Fact]
        public void TicketMovement()
        {
            var controller = Controller.GetInstance();
            controller.Initialize(2);

            var X = controller.MisterX;

            X.ScooterTicket = 5;
            X.BikeTicket = 5;
            X.BusTicket = 5;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgewählt um einen validen zug zu garantieren
            var newPos = X.Position.ConnectionBus.First();

            // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            controller.MovePlayer(X, newPos, TicketTypeEnum.Bus);

            Assert.Equal(X.Position, newPos);
        }

        /// <summary>
        /// hier wird auch getestet, ob nur der aktive spieler seinen Zug durchführen darf
        /// </summary>
        [Fact]
        public void DontMoveInactivePlayer() 
        {

            var controller = Controller.GetInstance();
            controller.Initialize(2);
            var player = controller.Detectives.First();

            player.BusTicket = 5;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgewählt um einen validen zug zu garantieren
            var newPos = player.Position.ConnectionBus.First();

            // übergeben des spieler, neuen position und fortbewegungsmittel an den controller
            controller.MovePlayer(player, newPos, TicketTypeEnum.Bus);

            Assert.False(player.Position == newPos);
        }
    }
}