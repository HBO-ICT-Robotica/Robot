using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Serial.InCommands {
	public class DCMotorsCommand : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public DCMotorsCommand(IHardwareInterface hardwareInterface) : base(1) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var value = incomingBytes[0];

			switch (value) {
				case 0:
					ServiceLocator.Get<ILogger>().LogDebug("Drive mode as draaien");
					break;
				case 1:
					ServiceLocator.Get<ILogger>().LogDebug("Drive mode 1 helft draaien");
					break;
				default:
					break;
			}
		}
	}
}