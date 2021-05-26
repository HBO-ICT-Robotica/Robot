using Robot.Serial;
using Robot.Spec;
using static Robot.Spec.RobotSpec.ServoDatas;

namespace Robot.Components {
	public class Gripper {
		private GripperSpec gripperSpec = null;

		private Servo servo = null;
		private LoadCell loadCell = null;

		public Gripper(GripperSpec gripperSpec, TeensyCommunicator teensyCommunicator, ServoData servoData) {
			this.gripperSpec = gripperSpec;

			this.servo = new Servo(this.gripperSpec.GetServoSpec(), teensyCommunicator, servoData);
			this.loadCell = new LoadCell(this.gripperSpec.GetLoadCellSpec());
		}
	}
}