using Robot.Spec;

namespace Robot.Components {
	public class Wheel {
		private WheelSpec wheelSpec = null;

		private Motor motor = null;

		public Wheel(WheelSpec wheelSpec) {
			this.wheelSpec = wheelSpec;

			this.motor = new Motor(this.wheelSpec.GetMotorSpec());
		}
	}
}