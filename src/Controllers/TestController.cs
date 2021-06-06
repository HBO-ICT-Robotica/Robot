using Robot.Components;
using Robot.Serial;
using Robot.Utility;
using System;
using Newtonsoft.Json;
using OpenCvSharp;
using System.Threading.Tasks;
using System.Net.Http;
using Robot.Utility.Logging;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		private float timeSinceLastTelemetryPush = float.MaxValue;

		private bool running = true;

		public TestController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();

			this.robot.GetBody().GoToRoot();

			var hardwareInterface = ServiceLocator.Get<IHardwareInterface>();
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

			timeSinceLastTelemetryPush += dt;

			if (timeSinceLastTelemetryPush >= (1.0f / 24.0f)) {
				try {
					videoCapture.Read(frame);

					if (frame.Empty())
						return;

					//Telemetry.Telemetry.DoRequest(frame, this.robot.GetBody()).Wait();
				} catch (Exception e) {
					Console.WriteLine(e);
				}

				timeSinceLastTelemetryPush = 0.0f;
			}

			var thrustJoystick = this.robot.GetRightJoystick();
			var steeringJoystick = this.robot.GetLeftJoystick();

			var leftSpeed = 0;
			var rightSpeed = 0;

			if (thrustJoystick.GetRelativeValue() < 0.1 && thrustJoystick.GetRelativeValue() > -0.1) {

			} else if (thrustJoystick.GetRelativeValue() > 0.1) {
				leftSpeed += (int)(thrustJoystick.GetRelativeValue() * 50);
				rightSpeed += (int)(thrustJoystick.GetRelativeValue() * 50);
			} else if (thrustJoystick.GetRelativeValue() < -0.1) {
				leftSpeed -= (int)(thrustJoystick.GetRelativeValue() * 50);
				rightSpeed -= (int)(thrustJoystick.GetRelativeValue() * 50);
			} 

			if (steeringJoystick.GetRelativeValue() > 0.1 || steeringJoystick.GetRelativeValue() < -0.1) {
				leftSpeed -= (int)(steeringJoystick.GetRelativeValue() * 30);
				rightSpeed += (int)(steeringJoystick.GetRelativeValue() * 30);
			}

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
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}