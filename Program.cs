using System;
using System.Threading;
using Robot.Components;
using Robot.Serial;
using System.Collections.Generic;

class Program {
	public static void Main() {
		Console.WriteLine("Program start");

		var communicator = new TeensyCommunicator("/dev/serial0", 9600);

		communicator.Open();

		List<Servo> servos = new List<Servo>();
		servos.Add(new Servo(communicator, 0, 903));
		servos.Add(new Servo(communicator, 1, 120));
		servos.Add(new Servo(communicator, 2, 120));
		servos.Add(new Servo(communicator, 3, 903));

		// foreach (var servo in servos) {
		// 	servo.SetSpeed(200);
		// }

		while (true) {
			// servos[0].SetTargetPosition(1023 - 430);
			// servos[1].SetTargetPosition(750);
			// servos[2].SetTargetPosition(750);
			// servos[3].SetTargetPosition(1023 - 430);

			// Thread.Sleep(2000);

			// servos[0].SetTargetPosition(1023 - 120);
			// servos[1].SetTargetPosition(120);
			// servos[2].SetTargetPosition(120);
			// servos[3].SetTargetPosition(1023 - 120);

			// Thread.Sleep(2000);

			if (Console.KeyAvailable) {
				break;
			}
		}

		communicator.Close();
	}
}