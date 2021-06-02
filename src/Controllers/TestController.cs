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

		public TestController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();

			Console.WriteLine("Going to root");
			this.robot.GetBody().GoToRoot();
		}

		public void Step(float dt) {
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

			var rJoystick = this.robot.GetRightJoystick();
			var lJoystick = this.robot.GetLeftJoystick();

			var leftSpeed = 0;
			var rightSpeed = 0;

			if (rJoystick.GetRelativeValue() < 0.1 && rJoystick.GetRelativeValue() > -0.1) {
				
			} else if (rJoystick.GetRelativeValue() > 0.1) 
			{
				leftSpeed = (int)(((rJoystick.GetRelativeValue() - 0.1) * (1.0 / 0.9)) * 255.0);
				rightSpeed = (int)(((rJoystick.GetRelativeValue() - 0.1) * (1.0 / 0.9)) * 255.0);
			} else if (rJoystick.GetRelativeValue() < -0.1) 
			{
				leftSpeed = (int)((((rJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * -255.0);
				rightSpeed = (int)((((rJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * -255.0);
			}

			if (lJoystick.GetRelativeValue() > 0.1) {
				leftSpeed += (int)((((rJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * 255.0);
			} else if (lJoystick.GetRelativeValue() < - 0.1) {
				rightSpeed += (int)((((rJoystick.GetRelativeValue() * -1) - 0.1) * (1.0 / 0.9)) * 255.0);
			}

			if (leftSpeed > 255) {
				var factor = 255 / leftSpeed;
				leftSpeed *= factor;
				rightSpeed *= factor;
			}

			if (leftSpeed < -255) {
				var factor = -255 / leftSpeed;
				leftSpeed *= factor;
				rightSpeed *= factor;
			}

			if (rightSpeed > 255) {
				var factor = 255 / rightSpeed;
				rightSpeed *= factor;
				rightSpeed *= factor;
			}

			if (rightSpeed < -255) {
				var factor = -255 / rightSpeed;
				rightSpeed *= factor;
				rightSpeed *= factor;
			}

			var logger = ServiceLocator.Get<ILogger>();
			Console.WriteLine($"Left: {leftSpeed}\tRight: {rightSpeed}");

			foreach (var leg in this.robot.GetBody().GetLeftBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				var motor = wheel.GetMotor();

				//motor.SetPwm(leftSpeed);
			}
			foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				var motor = wheel.GetMotor();

				//motor.SetPwm(rightSpeed);
			}
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}