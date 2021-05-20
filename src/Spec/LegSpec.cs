using System;

namespace Robot.Spec {
	/// <summary>
	/// Defines the specification of a leg
	/// </summary>
	public class LegSpec {
		private ServoSpec servoSpec = null;
		private WheelSpec wheelSpec = null;

		public LegSpec(ServoSpec servoSpec, WheelSpec wheelSpec) {
			this.servoSpec = servoSpec;
			this.wheelSpec = wheelSpec;
		}

		public ServoSpec GetServoSpec() {
			return this.servoSpec;
		}

		public WheelSpec GetWheelSpec() {
			return this.wheelSpec;
		}
	}
}