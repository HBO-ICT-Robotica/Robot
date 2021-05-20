using Robot.Serial;
using Robot.Spec;

namespace Robot.Components {
	public class Gripper {
		private GripperSpec gripperSpec = null;

		private Servo servo = null;
		private LoadCell loadCell = null;

		public Gripper(GripperSpec gripperSpec, TeensyCommunicator teensyCommunicator, byte servoId, ushort initialPosition) {
			this.gripperSpec = gripperSpec;

			this.servo = new Servo(this.gripperSpec.GetServoSpec(), teensyCommunicator, servoId, initialPosition);
			this.loadCell = new LoadCell(this.gripperSpec.GetLoadCellSpec());
		}
	}
}