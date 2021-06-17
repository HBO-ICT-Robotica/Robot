using System.Threading;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using Robot.Serial.InCommands;

namespace Robot.Serial {
	public partial class TeensyInterface : IHardwareInterface {
		public event ServoPositionUpdatedHandler servoPositionUpdated;
		public event JoystickValueReceivedHandler joystickValueReceived;
		public event RemoteTimeoutHandler remoteTimeoutEvent;
		public event LoadCellValueUpdatedHandler loadCellValueUpdated;

		private SerialPort serialPort = null;

		private InCommand currentParsingCommand = null;
		private byte[] incomingBytes = null;
		private int nextIncomingByteIndex = default;

		private Dictionary<byte, InCommand> commands = null;

		public TeensyInterface(string portName, int baudRate) {
			this.serialPort = new SerialPort();

			this.serialPort.PortName = portName;
			this.serialPort.BaudRate = baudRate;
			this.serialPort.Parity = Parity.None;
			this.serialPort.DataBits = 8;
			this.serialPort.StopBits = StopBits.One;
			this.serialPort.Handshake = Handshake.None;
			this.serialPort.WriteTimeout = 500;

			this.currentParsingCommand = null;
			this.incomingBytes = new byte[10];
			this.nextIncomingByteIndex = 0;

			this.commands = new Dictionary<byte, InCommand>();
			this.commands.Add(0x00, new UpdateServoPosition(this.servoPositionUpdated));
			this.commands.Add(0x01, new ReceiveJoystickPosition(this));
			this.commands.Add(0x02, new UpdateLoadCellValue(this));
			this.commands.Add(0x03, new ShutdownCommand(this));
			this.commands.Add(0x04, new ModusCommand(this));
			this.commands.Add(0x05, new DCMotorsCommand(this));
			this.commands.Add(0x06, new ServoCommand(this));
			//this.commands.Add(0x02, new RemoteTimeout(this.remoteTimeoutEvent));
		}

		public void Open() {
			if (this.serialPort.IsOpen)
				return;

			this.serialPort.Open();

			this.serialPort.DataReceived += OnDataReceived;

			var handshake = new byte[] { 0xFF };
			this.serialPort.Write(handshake, 0, handshake.Length);
		}

		public void Close() {
			if (!this.serialPort.IsOpen)
				return;

			this.serialPort.DataReceived -= OnDataReceived;

			this.serialPort.Close();
		}

		private void OnDataReceived(object sender, SerialDataReceivedEventArgs e) {
			while (this.serialPort.BytesToRead > 0) {
				var data = (byte)this.serialPort.ReadByte();
				Console.WriteLine(data);

				if (this.currentParsingCommand == null) {
					if (!this.commands.TryGetValue(data, out var command)) {
						// throw new Exception($"Command '{data}' not implemented");
						continue;
					} else {
						this.currentParsingCommand = command;
						Console.WriteLine(this.currentParsingCommand);
					}
				} else {
					this.incomingBytes[this.nextIncomingByteIndex] = data;
					this.nextIncomingByteIndex++;
				}

				if (this.nextIncomingByteIndex == this.currentParsingCommand.GetRequiredBytes()) {
					this.currentParsingCommand.Execute(this.incomingBytes);

					this.currentParsingCommand = null;
					this.nextIncomingByteIndex = 0;
				}
			}
		}

		public void WriteBytes(byte[] buffer) {
			// foreach (var data in buffer) {
			// 	Console.WriteLine(data);
			// }

			this.serialPort.Write(buffer, 0, buffer.Length);
		}

		public void SetServoTargetDegree(byte servoId, ushort position) {
			var _position = BitConverter.GetBytes(position);

			this.WriteBytes(new byte[] {
				0x01,
				servoId,
				_position[0],
				_position[1]
			});
		}

		public void SetServoLight(byte servoId, bool enabled) {
			var _enabled = BitConverter.GetBytes(enabled);

			this.WriteBytes(new byte[] {
				0x02,
				servoId,
				_enabled[0]
			});
		}

		public void SetServoSpeed(byte servoId, ushort speed) {
			var _speed = BitConverter.GetBytes(speed);

			this.WriteBytes(new byte[] {
				0x03,
				servoId,
				_speed[0],
				_speed[1]
			});
		}

		public void SetMotorPwm(byte motorId, byte pwm) {
			this.WriteBytes(new byte[] {
				0x04,
				motorId,
				pwm,
			});
		}

		public void SetMotorMode(byte motorId, byte mode) {
			this.WriteBytes(new byte[] {
				0x05,
				motorId,
				mode,
			});
		}

		public void InvokeJoystickValueReceived(byte id, byte value) {
			this.joystickValueReceived?.Invoke(id, value);
		}

		public void InvokeLoadCellValueUpdated(int value) {
			this.loadCellValueUpdated?.Invoke(value);
		}
	}
}