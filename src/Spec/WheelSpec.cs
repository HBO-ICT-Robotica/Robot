namespace Robot.Spec {
	/// <summary>
	/// Defines the specification of a wheel
	/// </summary>
	public class WheelSpec {
		private MotorSpec motorSpec = null;

		public WheelSpec(MotorSpec motorSpec) {
			this.motorSpec = motorSpec;
		}

		public MotorSpec GetMotorSpec() {
			return this.motorSpec;
		}
	}
}