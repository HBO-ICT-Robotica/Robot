namespace Robot.Serial.InCommands {
	public class UpdateLoadCellValue : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public UpdateLoadCellValue(IHardwareInterface hardwareInterface) : base(4) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var value = (int)((incomingBytes[2] << 24) + (incomingBytes[3] << 16) + (incomingBytes[2] << 8) + incomingBytes[1]);

			this.hardwareInterface.InvokeLoadCellValueUpdated(value);
		}
	}
}