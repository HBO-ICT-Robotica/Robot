using Robot.Serial;
using Robot.Spec;

namespace Robot.Components {
	public class Leg {
		private LegSpec legSpec = null;

		private Servo servo = null;
		private Wheel wheel = null;

		public Leg(LegSpec legSpec, TeensyCommunicator teensyCommunicator, byte servoId, ushort initialPosition) {
			this.legSpec = legSpec;

			this.servo = new Servo(this.legSpec.GetServoSpec(), teensyCommunicator, servoId, initialPosition);
			this.wheel = new Wheel(this.legSpec.GetWheelSpec());
		}

		public Servo GetServo() {
			return this.servo;
		}
	}
}