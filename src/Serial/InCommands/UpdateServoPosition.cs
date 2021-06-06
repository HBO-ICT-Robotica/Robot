namespace Robot.Serial.InCommands {
	public class UpdateServoPosition : InCommand {
		private ServoPositionUpdatedHandler servoPositionUpdated = null;

		public UpdateServoPosition(ServoPositionUpdatedHandler servoPositionUpdated) : base(3) {
			this.servoPositionUpdated = servoPositionUpdated;
		}

		public override void Execute(byte[] incomingBytes) {
			var id = incomingBytes[0];
			var position = (ushort)((incomingBytes[2] << 8) + incomingBytes[1]);

			this.servoPositionUpdated?.Invoke(id, position);
		}

	}
}