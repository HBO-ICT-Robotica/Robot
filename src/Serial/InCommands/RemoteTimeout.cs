namespace Robot.Serial.InCommands {
	public class RemoteTimeout : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public RemoteTimeout(IHardwareInterface hardwareInterface) : base(0) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			this.hardwareInterface.InvokeRemoteTimeout();
		}
	}
}