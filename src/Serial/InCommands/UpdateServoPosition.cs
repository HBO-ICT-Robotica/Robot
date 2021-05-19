using System.Data;
using System;

namespace Robot.Serial {
	public partial class TeensyCommunicator {
		private class UpdateServoPosition : InCommand {
			public UpdateServoPosition(TeensyCommunicator communicator) : base(communicator, 3) {
			}

			public override void Execute(byte[] incomingBytes) {
				var id = incomingBytes[0];
				var position = (ushort)((incomingBytes[2] << 8) + incomingBytes[1]);

				this.communicator.ServoPositionUpdated?.Invoke(id, position);
			}
		}
	}
}