namespace Robot.Serial.InCommands {
	public class ReceiveJoystickPosition : InCommand {
		private JoystickValueReceivedHandler joystickValueReceived = null;

		public ReceiveJoystickPosition(JoystickValueReceivedHandler joystickValueReceived) : base(2) {
			this.joystickValueReceived = joystickValueReceived;
			}

		public override void Execute(byte[] incomingBytes) {
			var id = incomingBytes[0];
			var value = incomingBytes[1];

			this.joystickValueReceived?.Invoke(id, value);
		}
	}

}