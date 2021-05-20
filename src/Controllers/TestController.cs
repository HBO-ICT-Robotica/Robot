using System.Threading;
using Robot.Components;
using Robot.Serial;
using Robot.Spec;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		public TestController(RobotSpec robotSpec, TeensyCommunicator teensyCommunicator) {
			var body = new Body(robotSpec, teensyCommunicator);

			var front = body.GetFrontBodyPart();
			var back = body.GetFrontBodyPart();

			front.SetTargetHeight(front.GetMaxHeight());
			back.SetTargetHeight(back.GetMaxHeight() * 0.7f);


			while (true) {
				// Twerk it
				back.SetTargetHeight(back.GetMaxHeight() * 0.6f);

				Thread.Sleep(100);

				back.SetTargetHeight(back.GetMaxHeight() * 0.7f);

				Thread.Sleep(100);
			}
		}
	}
}