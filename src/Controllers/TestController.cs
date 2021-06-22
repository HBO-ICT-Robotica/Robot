using System.Threading;
using Robot.Serial;
using Robot.Utility;
using System;
using OpenCvSharp;
using Robot.Units.Distance;
using Robot.Utility.Logging;
using Robot.Steering;
using Robot.Units.Angle;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		private Robot.Components.Robot robot = null;

		private WheelsControl steering;

		public TestController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.steering = new WheelsControl(this.robot.GetSteeringJoystick(), this.robot.GetThrustJoystick(), 0.05f, 180);

			//this.robot.GoToRoot();
			// this.robot.GetBody().GetFrontBodyPart().SetTargetHeight(70, -30);
			// this.robot.GetBody().GetBackBodyPart().SetTargetHeight(70, -30);

			// Thread.Sleep(1000);

			// this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].SetHeight(0);

			// Thread.Sleep(1000);

			// foreach (var leg in this.robot.GetBody().GetFrontBodyPart().GetLegs()) {
			// 	leg.GetWheel().SetSpeed(60);
			// }

			// foreach (var leg in this.robot.GetBody().GetBackBodyPart().GetLegs()) {
			// 	leg.GetWheel().SetSpeed(60);
			// }

			// Thread.Sleep(2000);

			// foreach (var leg in this.robot.GetBody().GetFrontBodyPart().GetLegs()) {
			// 	leg.GetWheel().SetSpeed(0);
			// }

			// foreach (var leg in this.robot.GetBody().GetBackBodyPart().GetLegs()) {
			// 	leg.GetWheel().SetSpeed(0);
			// }

			// Thread.Sleep(1000);

			// this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].SetHeight(108, -30);
		}

		public void Step(float dt) {
			var speed = this.steering.GetSpeed();

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.frontLeft);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.frontRight);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.backLeft);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.backRight);

			var rawFrontHeightInput = Math.Clamp(Map(this.robot.GetFrontHeightJoystick().GetRelativeValue(), 0.5f, 1f, 1f, 0f), 0f, 1f);
			var rawBackHeightInput = Math.Clamp(Map(this.robot.GetBackHeightJoystick().GetRelativeValue(), 0.5f, 1f, 1f, 0f), 0f, 1f);

			var front = this.robot.GetBody().GetFrontBodyPart();
			var back = this.robot.GetBody().GetBackBodyPart();

			front.GetLegs()[0].GetServo().SetTargetAngle((int)(front.GetLegs()[0].GetServo().GetTargetDegree() + rawFrontHeightInput));
			front.GetLegs()[1].GetServo().SetTargetAngle((int)(front.GetLegs()[0].GetServo().GetTargetDegree() + rawFrontHeightInput));

			back.GetLegs()[0].GetServo().SetTargetAngle((int)(back.GetLegs()[0].GetServo().GetTargetDegree() + rawBackHeightInput));
			back.GetLegs()[1].GetServo().SetTargetAngle((int)(back.GetLegs()[0].GetServo().GetTargetDegree() + rawBackHeightInput));


			//back.SetTargetHeight((int)(back.GetMaxHeight() * rawBackHeightInput));
		}

		public void Dispose() {
		}

		private float Map(float x, float inMin, float inMax, float outMin, float outMax) {
			return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}
}