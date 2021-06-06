namespace Robot.Serial {
	public abstract class InCommand {
		private int requiredBytes = default;

		public InCommand(int requiredBytes) {
			this.requiredBytes = requiredBytes;
		}

		public int GetRequiredBytes() {
			return this.requiredBytes;
		}

		public abstract void Execute(byte[] incomingBytes);
	}

}