using Robot.Serial;
using Robot.Utility;
using OpenCvSharp;
using Robot.Steering;

namespace Robot.Controllers {
	public class GateController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		private float timeSinceLastTelemetryPush = float.MaxValue;

		private bool running = true;

		private WheelsControl steering;

		public GateController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();

			//this.robot.GetBody().GoToRoot();
			// this.robot.GetBody().GetFrontBodyPart().SetTargetHeight(100);
			// this.robot.GetBody().GetBackBodyPart().SetTargetHeight(100);

			var hardwareInterface = ServiceLocator.Get<IHardwareInterface>();
			hardwareInterface.remoteTimeoutEvent += OnRemoteTimeout;

			this.steering = new WheelsControl(this.robot.GetSteeringJoystick(), this.robot.GetThrustJoystick(), 255);
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

			var thrustJoystick = this.robot.GetThrustJoystick();
			var steeringJoystick = this.robot.GetSteeringJoystick();

			//Console.WriteLine("weight = " + this.robot.GetGripper().GetLoadCell().GetWeight());
			//Console.Write(thrustJoystick.GetRelativeValue());
			var speed = this.steering.GetSpeed();

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.frontLeft);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.frontRight);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.backLeft);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.backRight);


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