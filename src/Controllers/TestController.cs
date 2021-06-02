using Robot.Components;
using Robot.Serial;
using Robot.Utility;
using System;
using Newtonsoft.Json;
using OpenCvSharp;
using System.Threading.Tasks;
using System.Net.Http;

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

			var communicator = ServiceLocator.Get<TeensyCommunicator>();

			communicator.JoystickValueRecevied += (value) => {
				var val = value - 32;

				if (val >= 0) {
					var pwm = (int)((val / 31.0f) * 255);

					Console.WriteLine("Pwm: " + pwm);
					foreach (var leg in this.robot.GetBody().GetLegs()) {
						var wheel = leg.GetWheel();
						var motor = wheel.GetMotor();

						motor.SetPwm(pwm);
					}
				}
			};
		}

		public void Step(float dt) {
			timeSinceLastTelemetryPush += dt;

			if (timeSinceLastTelemetryPush >= (1.0f / 24.0f)) {
				try {
					videoCapture.Read(frame);

					if (frame.Empty())
						return;

					Telemetry.Telemetry.DoRequest(frame, this.robot.GetBody()).Wait();
				} catch (Exception e) {
					Console.WriteLine(e);
				}

				timeSinceLastTelemetryPush = 0.0f;
			}

			var joystick = this.robot.GetLeftJoystick();

			var pwm = (int)(joystick.GetRelativeValue() * 255.0f);

			foreach (var leg in this.robot.GetBody().GetLegs()) {
				var wheel = leg.GetWheel();
				var motor = wheel.GetMotor();

				motor.SetPwm(pwm);
			}
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}