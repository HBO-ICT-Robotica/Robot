using System.Data;
using System;

namespace Robot.Serial {
	public partial class TeensyCommunicator {
		private class ReceiveJoystickPosition : InCommand {
			public ReceiveJoystickPosition(TeensyCommunicator communicator) : base(communicator, 1) {
			
			}

			public override void Execute(byte[] incomingBytes) {
				var value = incomingBytes[0];

				this.communicator.JoystickValueRecevied?.Invoke(value);
			}
		}
	}
}