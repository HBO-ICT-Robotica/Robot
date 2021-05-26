using System.Threading;
using Robot.Components;
using Robot.Serial;
using Robot.Spec;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		public TestController(RobotSpec robotSpec, TeensyCommunicator teensyCommunicator) {
			var body = new Body(robotSpec, teensyCommunicator);

			// foreach (var leg in body.GetFrontBodyPart().GetLegs()) {
			// 	leg.GetServo().SetTargetDegree(150);
			// }

			body.GetFrontBodyPart().GetLegs()[0].GetServo().SetTargetDegree(175);
			body.GetFrontBodyPart().GetLegs()[1].GetServo().SetTargetDegree(175);
			body.GetBackBodyPart().GetLegs()[0].GetServo().SetTargetDegree(175);
			body.GetBackBodyPart().GetLegs()[1].GetServo().SetTargetDegree(175);

			// var front = body.GetFrontBodyPart();
			// var back = body.GetFrontBodyPart();

			// front.SetTargetHeight(front.GetMaxHeight());
			// back.SetTargetHeight((int)(back.GetMaxHeight() * 0.7));


			// while (true) {
			// 	// Twerk it
			// 	back.SetTargetHeight((int)(back.GetMaxHeight() * 0.6));

			// 	Thread.Sleep(100);

			// 	back.SetTargetHeight((int)(back.GetMaxHeight() * 0.7));

			// 	Thread.Sleep(100);
			// }
		}
	}
}