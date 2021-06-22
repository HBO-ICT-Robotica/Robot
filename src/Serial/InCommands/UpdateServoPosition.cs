namespace Robot.Serial.InCommands {
	public class UpdateServoPosition : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public UpdateServoPosition(IHardwareInterface hardwareInterface) : base(3) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var id = incomingBytes[0];
			var position = (ushort)((incomingBytes[2] << 8) + incomingBytes[1]);

			this.hardwareInterface.InvokeServoPositionUpdated(id, position);
		}
	}
}