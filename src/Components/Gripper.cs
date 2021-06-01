using Robot.Serial;

namespace Robot.Components {
	public class Gripper {
		private Servo servo = null;
		private LoadCell loadCell = null;

		public Gripper() {
			// this.gripperSpec = gripperSpec;

			// this.servo = new Servo(this.gripperSpec.GetServoSpec(), teensyCommunicator, servoData);
			// this.loadCell = new LoadCell(this.gripperSpec.GetLoadCellSpec());
		}
	}
}