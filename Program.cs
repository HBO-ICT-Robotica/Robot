using System;
using System.IO.Ports;
using System.Threading;

class Program {
	public static void Main() {
		Console.WriteLine("Program start");

		var communicator = TeensyCommunicator.Instance;
		communicator.Open();

		communicator.SetServoLight(3, true);;
		communicator.SetServoTargetPosition(2, 300);
		Thread.Sleep(200);
		communicator.SetServoLight(3, false);
		Thread.Sleep(50);
		communicator.SetServoLight(3, true);
		Thread.Sleep(50);
		communicator.SetServoTargetPosition(2, 0);

		//communicator.SetServoTargetPosition(2, 5);

		communicator.Close();

		// Console.WriteLine("Creating Serial Port");
		// var serial = new SerialPort();

		// try {

		// 	serial.PortName = "/dev/serial0";
		// 	serial.BaudRate = 9600;
		// 	serial.Parity = Parity.None;
		// 	serial.DataBits = 8;
		// 	serial.StopBits = StopBits.One;
		// 	serial.Handshake = Handshake.None;
		// 	serial.WriteTimeout = 500;

		// 	Console.WriteLine("Created Serial Port");

		// 	var package = new byte[] {
		// 	0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x8, 13, 39, 0xFF
		// };

		// 	Console.WriteLine("Opening Serial Port");
		// 	serial.Open();
		// 	Console.WriteLine("Opened Serial Port");

		// 	Console.WriteLine("Sending package");
		// 	serial.Write(package, 0, package.Length);
		// 	//serial.Write("p");
		// 	Console.WriteLine("Sent package");

		// 	Console.WriteLine("Closing Serial Port");
		// 	serial.Close();
		// 	Console.WriteLine("Closed Serial Port");

		// } catch (Exception e) {
		// 	Console.WriteLine("Error:");
		// 	Console.WriteLine(e.ToString());
		// } finally {
		// 	Console.WriteLine("Closing Serial Port");
		// 	serial.Close();
		// 	Console.WriteLine("Closed Serial Port");
		// }

		// Console.WriteLine("Program end");
	}
}