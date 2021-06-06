using System.Data;
using System;

namespace Robot.Serial {
	public partial class TeensyCommunicator {
		private class ReceiveJoystickPosition : InCommand {
			public ReceiveJoystickPosition(TeensyCommunicator communicator) : base(communicator, 2) {
			
			}

			public override void Execute(byte[] incomingBytes) {
				var id = incomingBytes[0];
				var value = incomingBytes[1];

				this.communicator.JoystickValueRecevied?.Invoke(id, value);
			}
		}
	}
}