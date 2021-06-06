namespace Robot.Serial.InCommands {
	public class RemoteTimeout : InCommand {
		private RemoteTimeoutHandler remoteTimeoutEvent = null;

		public RemoteTimeout(RemoteTimeoutHandler remoteTimeoutEvent) : base(0) {
			this.remoteTimeoutEvent = remoteTimeoutEvent;
		}

		public override void Execute(byte[] incomingBytes) {
			this.remoteTimeoutEvent?.Invoke();
		}
	}
}