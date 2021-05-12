using System.IO.Ports;
using System;

public class TeensyCommunicator {
	private static TeensyCommunicator instance = null;
	public static TeensyCommunicator Instance {
		get {
			if (TeensyCommunicator.instance == null)
				TeensyCommunicator.instance = new TeensyCommunicator();

			return TeensyCommunicator.instance;
		}
	}

	private SerialPort serialPort = null;

	private TeensyCommunicator() {
		this.serialPort = new SerialPort();

		this.serialPort.PortName = "/dev/serial0";
		this.serialPort.BaudRate = 9600;
		this.serialPort.Parity = Parity.None;
		this.serialPort.DataBits = 8;
		this.serialPort.StopBits = StopBits.One;
		this.serialPort.Handshake = Handshake.None;
		this.serialPort.WriteTimeout = 500;
	}

	public void Open() {
		this.serialPort.Open();
	}

	public void Close(){
		if (!this.serialPort.IsOpen)
			return;

		this.serialPort.Close();
	}

	public void WriteBytes(byte[] buffer) {
		for (int i = 0; i < buffer.Length; i++) {
			Console.WriteLine(buffer[i]);
		}

		this.serialPort.Write(buffer, 0, buffer.Length);
	}

	public void SetServoTargetPosition(byte servoId, short position) {
		Console.WriteLine("Set Target Pos");
		var _position = BitConverter.GetBytes(position);

		this.WriteBytes(new byte[] {
			0x01,
			servoId,
			_position[0],
			_position[1]
		});
	}

	public void SetServoLight(byte servoId, bool enabled) {
		Console.WriteLine("Set Light");
		var _enabled = BitConverter.GetBytes(enabled);

		this.WriteBytes(new byte[] {
			0x02,
			servoId,
			_enabled[0]
		});
	}
}