namespace Robot.Serial {
	public partial class TeensyCommunicator {
		private abstract class InCommand {
			protected TeensyCommunicator communicator = null;

			private int requiredBytes = default;

			public InCommand(TeensyCommunicator communicator, int requiredBytes) {
				this.communicator = communicator;

				this.requiredBytes = requiredBytes;

				this.communicator.ServoPositionUpdated?.Invoke(0, 0);
			}

			public int GetRequiredBytes() {
				return this.requiredBytes;
			}

			public abstract void Execute(byte[] incomingBytes);
		}
	}
}