using System;
using System.Collections.Generic;
using Robot.Components;
using Robot.Serial;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot {
	public class Program : IDisposable {
		private ILogger logger = null;
		private TeensyCommunicator communicator = null;

		private Robot.Components.Robot robot = null;

		public Program() {
			this.InitializeLogger(LogLevel.DEBUG);
			//this.InitializeCommunicator("/dev/serial0", 9600);
			this.InitializeCommunicator("COM1", 9600);

			this.InitializeRobot();
		}

		public void Step(float dt) {
			Console.WriteLine(dt);
		}

		private void InitializeLogger(LogLevel logLevel) {
			var loggerComposite = new LoggerComposite(logLevel);
			loggerComposite.AddLogger(new LoggerConsole());

			this.logger = loggerComposite;
			ServiceLocator.Register(this.logger);
		}

		private void DisposeLogger() {
			ServiceLocator.Unregister<ILogger>(this.logger);
			this.logger.Flush();
		}

		private void InitializeCommunicator(string port, int baudRate) {
			this.communicator = new TeensyCommunicator(port, baudRate);
			communicator.Open();

			ServiceLocator.Register(communicator);
		}

		private void DisposeCommunicator() {
			ServiceLocator.Unregister<TeensyCommunicator>(this.communicator);
			this.communicator.Close();
		}

		private void InitializeRobot() {
			var legZeroDegree = 135;
			var legMinDegree = 45;
			var legMaxDegree = 175;

			var legLength = 108;
			var legDistanceToWheel = 29;

			var frontLeftLeg = new Leg(
				new Servo(0, true, legZeroDegree, legMinDegree, legMaxDegree),
				new Wheel(
					new Motor(0)
				),
				legLength,
				legDistanceToWheel
			);

			var frontRightLeg = new Leg(
				new Servo(1, false, legZeroDegree, legMinDegree, legMaxDegree),
				new Wheel(
					new Motor(1)
				),
				legLength,
				legDistanceToWheel
			);

			var backLeftLeg = new Leg(
				new Servo(2, false, legZeroDegree, legMinDegree, legMaxDegree),
				new Wheel(
					new Motor(3)
				),
				legLength,
				legDistanceToWheel
			);

			var backRightLeg = new Leg(
				new Servo(3, true, legZeroDegree, legMinDegree, legMaxDegree),
				new Wheel(
					new Motor(2)
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
				new Joystick(0, -32, 31),
				new Joystick(1, -32, 31)
			);
		}

		public void Dispose() {
			this.DisposeLogger();
			this.DisposeCommunicator();
		}
	}
}