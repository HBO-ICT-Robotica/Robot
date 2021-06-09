using Robot.Serial;
using Robot.Utility;
using System;
using OpenCvSharp;
using Robot.Units.Distance;
using Robot.Utility.Logging;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		private Robot.Components.Robot robot = null;

		// private VideoCapture videoCapture = null;
		// private Mat frame = null;

		private float timeSinceLastTelemetryPush = float.MaxValue;

		private bool running = true;

		public TestController(Robot.Components.Robot robot) {
			this.robot = robot;

			// this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			// this.frame = new Mat();

			//this.robot.GetBody().GoToRoot();
			this.robot.GetBody().GetFrontBodyPart().SetTargetHeight(new Milimeter(100));
			this.robot.GetBody().GetBackBodyPart().SetTargetHeight(new Milimeter(100));

			var hardwareInterface = ServiceLocator.Get<TeensyInterface>();
			hardwareInterface.remoteTimeoutEvent += OnRemoteTimeout;
		}

		private void OnRemoteTimeout() {
			this.running = false;

			foreach (var leg in this.robot.GetBody().GetLeftBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed(0);
			}

			foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed(0);
			}
		}

		public void Step(float dt) {
			if (!running)
				return;

			// timeSinceLastTelemetryPush += dt;

			// if (timeSinceLastTelemetryPush >= (1.0f / 24.0f)) {
			// 	try {
			// 		videoCapture.Read(frame);

			// 		if (frame.Empty())
			// 			return;

			// 		//Telemetry.Telemetry.DoRequest(frame, this.robot.GetBody()).Wait();
			// 	} catch (Exception e) {
			// 		Console.WriteLine(e);
			// 	}

			// 	timeSinceLastTelemetryPush = 0.0f;
			// }

			var thrustJoystick = this.robot.GetRightJoystick();
			var steeringJoystick = this.robot.GetLeftJoystick();

			//Console.WriteLine(thrustJoystick.GetRelativeValue());


			var leftSpeed = 0;
			var rightSpeed = 0;

			if (thrustJoystick.GetRelativeValue() < 0.1 && thrustJoystick.GetRelativeValue() > -0.1) {

			} else if (thrustJoystick.GetRelativeValue() > 0.1) {
				leftSpeed += (int)(thrustJoystick.GetRelativeValue() * 200);
				rightSpeed += (int)(thrustJoystick.GetRelativeValue() * 200);
			} else if (thrustJoystick.GetRelativeValue() < -0.1) {
				leftSpeed -= (int)(thrustJoystick.GetRelativeValue() * 200);
				rightSpeed -= (int)(thrustJoystick.GetRelativeValue() * 200);
			} 

			if (steeringJoystick.GetRelativeValue() > 0.1) {
				leftSpeed += 0;
				rightSpeed += 130;
			} else if (steeringJoystick.GetRelativeValue() < -0.1) {
				leftSpeed += 130;
				rightSpeed += 0;
			}

			// robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed(30);

			// if (thrustJoystick.GetRelativeValue() < 0.1 && thrustJoystick.GetRelativeValue() > -0.1) {
			// 	// Deadzone
			// } else if (thrustJoystick.GetRelativeValue() > 0.1) {
			// 	leftSpeed = (int)(((thrustJoystick.GetRelativeValue() - 0.1) * (1.0 / 0.9)) * 255.0);
			// 	rightSpeed = (int)(((thrustJoystick.GetRelativeValue() - 0.1) * (1.0 / 0.9)) * 255.0);
			// } else if (thrustJoystick.GetRelativeValue() < -0.1) {
			// 	leftSpeed = (int)((((thrustJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * -255.0);
			// 	rightSpeed = (int)((((thrustJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * -255.0);
			// }

			// if (steeringJoystick.GetRelativeValue() > 0.1) {
			// 	leftSpeed += (int)((((thrustJoystick.GetRelativeValue()) - 0.1) * (1.0 / 0.9)) * 255.0);
			// } else if (steeringJoystick.GetRelativeValue() < -0.1) {
			// 	rightSpeed += (int)((((thrustJoystick.GetRelativeValue()) - 0.1) * (1.0 / 0.9)) * 255.0);
			// }

			// if (leftSpeed > 255) {
			// 	var factor = 255 / leftSpeed;
			// 	leftSpeed *= factor;
			// 	rightSpeed *= factor;
			// }

			// if (leftSpeed < -255) {
			// 	var factor = -255 / leftSpeed;
			// 	leftSpeed *= factor;
			// 	rightSpeed *= factor;
			// }

			// if (rightSpeed > 255) {
			// 	var factor = 255 / rightSpeed;
			// 	rightSpeed *= factor;
			// 	rightSpeed *= factor;
			// }

			// if (rightSpeed < -255) {
			// 	var factor = -255 / rightSpeed;
			// 	rightSpeed *= factor;
			// 	rightSpeed *= factor;
			// }

			//Console.WriteLine($"Left: {leftSpeed}\t Right:{rightSpeed}");
			//Console.WriteLine($"Left: {leftSpeed}, Right: {rightSpeed}");

			//var logger = ServiceLocator.Get<ILogger>();
			// Console.WriteLine($"Left speed {leftSpeed}");
			// Console.WriteLine($"Right speed {rightSpeed}");

			// foreach (var leg in this.robot.GetBody().GetLeftBodyPart().GetLegs()) {
			// 	var wheel = leg.GetWheel();

			// 	//wheel.SetSpeed(leftSpeed);
			// }


			foreach (var leg in this.robot.GetBody().GetLeftBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed(leftSpeed);
			}

			foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed(rightSpeed);
			}
		}

		public void Dispose() {
			// this.videoCapture.Dispose();
			// this.frame.Dispose();
		}
	}
}