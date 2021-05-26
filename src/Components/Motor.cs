using Robot.Spec;

namespace Robot.Components {
	public class Motor {
		private MotorSpec motorSpec = null;

		public Motor(MotorSpec motorSpec) {
			this.motorSpec = motorSpec;
		}
	}
}