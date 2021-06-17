using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Serial.InCommands {
	public class ModusCommand : InCommand {
		private IHardwareInterface hardwareInterface = null;

		public ModusCommand(IHardwareInterface hardwareInterface) : base(1) {
			this.hardwareInterface = hardwareInterface;
		}

		public override void Execute(byte[] incomingBytes) {
			var value = incomingBytes[0];

			switch (value) {
				case 0:
					ServiceLocator.Get<ILogger>().LogDebug("Maatschappelijk doel");
					break;
				case 1:
					ServiceLocator.Get<ILogger>().LogDebug("Blauw blok volgen");
					break;
				case 2:
					ServiceLocator.Get<ILogger>().LogDebug("Wegen");
					break;
				case 3:
					ServiceLocator.Get<ILogger>().LogDebug("Poortje");
					break;
				case 4:
					ServiceLocator.Get<ILogger>().LogDebug("Helling");
					break;
				case 5:
					ServiceLocator.Get<ILogger>().LogDebug("Obstakel");
					break;
				default:
					break;
			}
		}
	}
}