using GameComponents;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
        }

        [Fact]
        // testen ob controller die bewegung verweigert, wenn keinen tickets vorhanden sind
        public void NoTicketsNoMovement()
        {
            // controller instanzieren & spieler anzahl festlegen
            // todo: get methoden des controllorer f�r den gamestate mit read only properties versehen
            // um sicherzustellen, dass nach initialisierung die Spieleranzahl nicht ver�ndert werden kann
            var controller = Controller.GetInstance();
            controller.Initialize(4);

            var pois = controller.PointsOfInterest;
            var X = controller.MisterX;

            /* d�rfte theoretisch nicht gehen! muss durch den Controller selbst initiiert werden und m�sste automatisch bei bewegung
             * durch den controller basierend auf der bewegungsart abgezogen werden um manipulation zu vermeiden
             * 
             * todo: hatten wir jetzt aber keinen bock dazu xD ticket muss noch geschrieben werden */
            X.ScooterTicket = 0;
            X.BikeTicket = 0;
            X.BusTicket = 0;

            // zuf�lllige Zielposition f�r den Zug festlegen
            PointOfInterest newPos = pois.First();

            // testen ob Zielposition ungleich aktueller position
            while (X.Position == newPos)
            {
                newPos = pois[Random.Shared.Next(0, pois.Count - 1)];
            }
            
            // testen der controller MovePlayer method
            controller.MovePlayer(X, newPos, TicketTypeEnum.Scooter);

            Assert.False(X.Position == newPos);
        }

        [Fact]
        // testen ob der controller einen validen Zug zul�sst
        public void TicketMovement()
        {
            var controller = Controller.GetInstance();
            controller.Initialize(1);

            var X = controller.MisterX;

            X.ScooterTicket = 5;
            X.BikeTicket = 5;
            X.BusTicket = 5;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgew�hlt um einen validen zug zu garantieren
            var newPos = X.Position.ConnectionBus.First();

            // �bergeben des spieler, neuen position und fortbewegungsmittel an den controller
            controller.MovePlayer(X, newPos, TicketTypeEnum.Bus);

            Assert.True(X.Position == newPos);
        }


        [Fact]
        public void DontMoveInactivePlayer() {

            // hier wird auch getestet, ob nur der aktive spieler seinen Zug durchf�hren darf

            var controller = Controller.GetInstance();

            // todo: logik check ben�tigt ob mindestens 2 spieler initalisiert wurden (fehlermeldung oder sowas)
            controller.Initialize(2);

            var player = controller.Detectives.First();

            player.BusTicket = 5;

            // Zielposition wird basierend auf den existierenden verbindungen des POI ausgew�hlt um einen validen zug zu garantieren
            var newPos = player.Position.ConnectionBus.First();

            // �bergeben des spieler, neuen position und fortbewegungsmittel an den controller
            controller.MovePlayer(player, newPos, TicketTypeEnum.Bus);

            // todo: Es muss ein check angelegt werden, dass nur der aktive spieler bewegt werden darf
            Assert.False(player.Position == newPos);

        }




    }
}