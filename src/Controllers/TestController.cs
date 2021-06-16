using System.Threading;
using Robot.Serial;
using Robot.Utility;
using System;
using OpenCvSharp;
using Robot.Units.Distance;
using Robot.Utility.Logging;
using Robot.Steering;

namespace Robot.Controllers {
	public class TestController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		private float timeSinceLastTelemetryPush = float.MaxValue;

		private bool running = true;

		private JoystickSteering steering;

		public TestController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();

			//this.robot.GetBody().GoToRoot();
			this.robot.GetBody().GetFrontBodyPart().SetTargetHeight(new Milimeter(100));
			this.robot.GetBody().GetBackBodyPart().SetTargetHeight(new Milimeter(100));

			var hardwareInterface = ServiceLocator.Get<TeensyInterface>();
			hardwareInterface.remoteTimeoutEvent += OnRemoteTimeout;

			this.steering = new JoystickSteering(this.robot.GetLeftJoystick(), this.robot.GetRightJoystick(), 255);
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
			// if (!running)
			// 	return;

			// timeSinceLastTelemetryPush += dt;

			// if (timeSinceLastTelemetryPush >= (1.0f / 24.0f)) {
			// 	try {
			// 		videoCapture.Read(frame);

			// 		if (frame.Empty())
			// 			return;

			// 		Telemetry.Telemetry.DoRequest(frame, this.robot).Wait();
			// 	} catch (Exception e) {
			// 		Console.WriteLine(e);
			// 	}

			// 	timeSinceLastTelemetryPush = 0.0f;
			// }

			var thrustJoystick = this.robot.GetRightJoystick();
			var steeringJoystick = this.robot.GetLeftJoystick();

			//Console.WriteLine("weight = " + this.robot.GetGripper().GetLoadCell().GetWeight());
			//Console.Write(thrustJoystick.GetRelativeValue());
			this.steering.UpdateSpeed();

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed(this.steering.GetFrontLeftSpeed());
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed(this.steering.GetFrontRightSpeed());

			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed(this.steering.GetBackLeftSpeed());
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed(this.steering.GetBackRightSpeed());


			// 	Thread.Sleep(1000);

				// foreach (var leg in this.robot.GetBody().GetFrontBodyPart().GetLegs()) {
				// 	var wheel = leg.GetWheel();
				// 	//wheel.SetSpeed(this.steering.GetLeftSpeed());
				// 	wheel.SetSpeed(30);
				// }

			// 	foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
			// 		var wheel = leg.GetWheel();
			// 		//wheel.SetSpeed(this.steering.GetRightSpeed());
			// 		wheel.SetSpeed(-30);
			// 	}

			// 	Thread.Sleep(1000);
			// }

			// var gripper = robot.GetGripper();
			// gripper.Open();
			// gripper.Close(Components.Gripper.Pickupable.BALL);

		}
		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}