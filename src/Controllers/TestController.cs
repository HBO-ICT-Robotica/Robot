using System.Threading;
using Robot.Components;
using Robot.Serial;
using Robot.Spec;
using Robot.Utility;
using System;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		public TestController(Robot.Components.Robot robot) {
			var body = robot.GetBody();

			var front = body.GetFrontBodyPart();
			var back = body.GetBackBodyPart();
			var left = body.GetLeftBodyPart();
			var right = body.GetRightBodyPart();

			body.GoToRoot();
			
			var communicator = ServiceLocator.Get<TeensyCommunicator>();

			communicator.JoystickValueRecevied += (value) => {

				var val = value - 32;

				if (val >= 0) {
					var pwm = (int)((val / 31.0f) * 255);

					Console.WriteLine("Pwm: " + pwm);
					foreach (var leg in body.GetLegs()) {
						var wheel = leg.GetWheel();
						var motor = wheel.GetMotor();


						motor.SetPwm(pwm);
					}
				}
			};

			// front.SetTargetHeight(100);
			// back.SetTargetHeight(100);


			while (true) {
				Thread.Sleep(50);
			}

			// front.SetTargetHeight(front.GetMaxHeight());
			// back.SetTargetHeight(front.GetMaxHeight());

			// Thread.Sleep(1000);

			// left.SetTargetHeight(0);

			// Thread.Sleep(1000);
			// right.SetTargetHeight(0);

			// Thread.Sleep(1000);
			// front.SetTargetHeight(front.GetMaxHeight());

			// Thread.Sleep(1000);
			// back.SetTargetHeight(back.GetMaxHeight());
		}
	}
}