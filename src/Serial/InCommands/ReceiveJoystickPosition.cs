using System;
using Robot.Serial;

namespace Robot.Serial.InCommands {
	public class ReceiveJoystickPosition : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public ReceiveJoystickPosition(IHardwareInterface hardwareInterface) : base(2) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var id = incomingBytes[0];
			var value = incomingBytes[1];

			this.hardwareInterface.InvokeJoystickValueReceived(id, value);
		}
	}
}