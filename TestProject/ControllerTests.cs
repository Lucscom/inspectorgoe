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
            controller.Initialize(6);

            var pois = controller.PointsOfInterest;
            var X = controller.MisterX;

            X.ScooterTicket = 0;
            X.BikeTicket = 0;
            X.BusTicket = 0;

            controller.MovePlayer(X, pois.First(), TicketTypeEnum.Scooter);

            Assert.True(X.Position != pois.First());
        }
    }
}