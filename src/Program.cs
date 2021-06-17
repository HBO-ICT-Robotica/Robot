using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using Robot.Components;
using Robot.Controllers;
using Robot.Serial;
using Robot.Units.Angle;
using Robot.Utility;
using Robot.Utility.Logging;
using Robot.VirtualWindow;

namespace Robot {
	public class Program : IDisposable {
		private ILogger logger = null;

		private IHardwareInterface hardwareInterface = null;

		private Robot.Components.Robot robot = null;

		private IRobotController robotController = null;

		private VirtualWindowHost virtualWindowHost = null;

		public Program() {
			this.logger = ServiceLocator.Get<ILogger>();

			this.InitializeHardwareInterface("/dev/serial0", 9600);
			this.InitializeVirtualWindowHost();
			this.InitializeRobot();

			//this.robotController = new TestController(this.robot);
			//this.robotController = new TrackingController(this.robot);
			//this.robotController = new PebblesController(this.robot);
			this.robotController = new DanceController(this.robot);
			this.logger.LogDebug($"Initialized controller '{this.robotController}'");
		}

		public void Step(float dt) {
			this.robotController.Step(dt);
		}

		private void InitializeHardwareInterface(string port, int baudRate) {
			//this.hardwareInterface = new TeensyInterface(port, baudRate);
			this.hardwareInterface = new VoidInterface();
			this.hardwareInterface.Open();

			ServiceLocator.Register<IHardwareInterface>(this.hardwareInterface);

			this.logger.LogDebug("Initialized hardware interface");
		}

		private void DisposeHardwareInterface() {
			ServiceLocator.Unregister<IHardwareInterface>(this.hardwareInterface);
			this.hardwareInterface.Close();

			this.hardwareInterface = null;

			this.logger.LogDebug("Disposed hardware interface");
		}

		private void InitializeRobot() {
			var legZeroAngle = new Degrees(135);
			var legMinAngle = new Degrees(45);
			var legMaxAngle = new Degrees(175);

			var legLength = 108;
			var legDistanceToWheel = 29;

			// 3 Back right
			// 0 Front left
			// 1 Back left
			// 2 Front Right

			var frontLeftLeg = new Leg(
				new Servo(2, true, legZeroAngle, legMinAngle, legMaxAngle),
				new Wheel(
					new Motor(0)
				),
				legLength,
				legDistanceToWheel
			);

			var frontRightLeg = new Leg(
				new Servo(3, false, legZeroAngle, legMinAngle, legMaxAngle),
				new Wheel(
					new Motor(2)
				),
				legLength,
				legDistanceToWheel
			);

			var backLeftLeg = new Leg(
				new Servo(0, true, legZeroAngle, legMinAngle, legMaxAngle),
				new Wheel(
					new Motor(1)
				),
				legLength,
				legDistanceToWheel
			);

			var backRightLeg = new Leg(
				new Servo(1, false, legZeroAngle, legMinAngle, legMaxAngle),
				new Wheel(
					new Motor(3)
				),
				legLength,
				legDistanceToWheel
			);

			this.robot = new Robot.Components.Robot(
				new Body(
					new BodyPart(new List<Leg>() { frontLeftLeg, frontRightLeg }),
					new BodyPart(new List<Leg>() { backLeftLeg, backRightLeg }),
					new BodyPart(new List<Leg>() { frontLeftLeg, backLeftLeg }),
					new BodyPart(new List<Leg>() { frontRightLeg, backRightLeg })
				),
				new Joystick(0, 0, 63),
				new Joystick(1, 0, 63),
				new Gripper(
					new Servo(4, false, new Degrees(135), new Degrees(90), new Degrees(135)), new LoadCell()
				)
			);

			this.logger.LogDebug("Initialized robot");
		}

		private void InitializeVirtualWindowHost() {
			//this.virtualWindowHost = new VirtualWindowHost();

			//ServiceLocator.Register<VirtualWindowHost>(this.virtualWindowHost);

			//this.logger.LogDebug($"Initialized virtual window host");
		}

		private void DisposeVirtualWindowHost() {
			ServiceLocator.Unregister<VirtualWindowHost>(this.virtualWindowHost);

			this.virtualWindowHost.Dispose();

			this.virtualWindowHost = null;
		}

		public void Dispose() {
			this.DisposeHardwareInterface();
			this.DisposeVirtualWindowHost();
		}
	}
}