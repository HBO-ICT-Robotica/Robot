namespace Robot.Serial {
	public partial class TeensyCommunicator {
		private class RemoteTimeout : InCommand {
			public RemoteTimeout(TeensyCommunicator communicator) : base(communicator, 0) {
			
			}

			public override void Execute(byte[] incomingBytes) {
				this.communicator.RemoteTimeoutEvent?.Invoke();
			}
		}
	}
}