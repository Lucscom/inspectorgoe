using GameComponents;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
        }

        [Fact]
        public void NoTicketsNoMovement()
        {
            var controller = Controller.GetInstance();
            controller.Initialize(4);

            var pois = controller.PointsOfInterest;
            var X = controller.MisterX;

            X.ScooterTicket = 0;
            X.BikeTicket = 0;
            X.BusTicket = 0;

            PointOfInterest newPos = pois.First();
            while (X.Position == newPos)
            {
                newPos = pois[Random.Shared.Next(0, pois.Count - 1)];
            }
            

            controller.MovePlayer(X, newPos, TicketTypeEnum.Scooter);

            Assert.True(X.Position != newPos);
        }

        [Fact]
        public void TicketMovement()
        {
            var controller = Controller.GetInstance();
            controller.Initialize(4);

            var X = controller.MisterX;

            X.ScooterTicket = 5;
            X.BikeTicket = 5;
            X.BusTicket = 5;

            var newPos = X.Position.ConnectionBus.First();

            controller.MovePlayer(X, newPos, TicketTypeEnum.Bus);

            Assert.True(X.Position == newPos);
        }

    }
}