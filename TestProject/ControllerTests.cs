using GameComponents;

namespace TestProject
{
    public class ControllerTests
    {
        public ControllerTests()
        {
            Initialize();
        }

        [Fact]
        public void Initialize()
        {
            var controller = Controller.GetInstance();
            controller.Initialize(6);

            //Assert.True(controller.MovePlayer(,))
        }
    }
}