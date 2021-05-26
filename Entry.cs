using System.Threading;
using System;
using Robot.Components;
using Robot.Controllers;
using Robot.Serial;
using Robot.Spec;
using Robot.Utility.Logging;
using Robot.Utility;

namespace Robot {
	/// <summary>
	/// Entry point of the application
	/// </summary>
	public class Entry {
		public static void Main() {
			var logger = new CompositeLogger();

			ServiceLocator.Register<ILogger>(logger);
			
			var communicator = new TeensyCommunicator("/dev/serial0", 9600);
			communicator.Open();

			var robotSpec = new RobotSpec(
				new LegSpec(
					new ServoSpec(45, 175, 135),
					new WheelSpec(
						new MotorSpec()
					),
					108,
					29
				),
				new GripperSpec(
					new ServoSpec(0, 0, 0), // TODO: Figure out these values
					new LoadCellSpec()
				),

				new RobotSpec.ServoDatas(
					new RobotSpec.ServoDatas.ServoData(0, true),
					new RobotSpec.ServoDatas.ServoData(1, false),
					new RobotSpec.ServoDatas.ServoData(2, false),
					new RobotSpec.ServoDatas.ServoData(3, true)
				)
			);

			var controller = new TestController(robotSpec, communicator);


			// var communicator = new TeensyCommunicator("/dev/serial0", 9600);

			// communicator.Open();

			// List<Servo> servos = new List<Servo>();
			// servos.Add(new Servo(communicator, 0, 903));
			// servos.Add(new Servo(communicator, 1, 120));
			// servos.Add(new Servo(communicator, 2, 120));
			// servos.Add(new Servo(communicator, 3, 903));

			// // foreach (var servo in servos) {
			// // 	servo.SetSpeed(200);
			// // }

			// while (true) {
			// 	// servos[0].SetTargetPosition(1023 - 430);
			// 	// servos[1].SetTargetPosition(750);
			// 	// servos[2].SetTargetPosition(750);
			// 	// servos[3].SetTargetPosition(1023 - 430);

			// 	// Thread.Sleep(2000);

			// 	// servos[0].SetTargetPosition(1023 - 120);
			// 	// servos[1].SetTargetPosition(120);
			// 	// servos[2].SetTargetPosition(120);
			// 	// servos[3].SetTargetPosition(1023 - 120);

			// 	// Thread.Sleep(2000);

			// 	if (Console.KeyAvailable) {
			// 		break;
			// 	}
			// }

			// communicator.Close();
		}
	}
}