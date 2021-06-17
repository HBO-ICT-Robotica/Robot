using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Serial.InCommands {
	public class ServoCommand : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public ServoCommand(IHardwareInterface hardwareInterface) : base(1) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var value = incomingBytes[0];

			switch (value) {
				case 0:
					ServiceLocator.Get<ILogger>().LogDebug("Voorarmen servos");
					break;
				case 1:
					ServiceLocator.Get<ILogger>().LogDebug("Achterarmen servos");
					break;
				case 2:
					ServiceLocator.Get<ILogger>().LogDebug("Alle servos");
					break;
				case 3:
					ServiceLocator.Get<ILogger>().LogDebug("Grijper open");
					break;
				case 4:
					ServiceLocator.Get<ILogger>().LogDebug("Grijper dicht");
					break;
				default:
					break;
			}
		}
	}
}