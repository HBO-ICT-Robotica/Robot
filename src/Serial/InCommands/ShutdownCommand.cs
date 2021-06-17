using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Serial.InCommands {
	public class ShutdownCommand : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public ShutdownCommand(IHardwareInterface hardwareInterface) : base(1) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var value = incomingBytes[0];

			switch (value) {
				case 0:
					ServiceLocator.Get<ILogger>().LogDebug("All shutdown");
					break;
				case 1:
					ServiceLocator.Get<ILogger>().LogDebug("Stop alles (motoren)");
					break;
				case 2:
					ServiceLocator.Get<ILogger>().LogDebug("Stop modus");
					break;
				default:
					break;
			}
		}
	}
}