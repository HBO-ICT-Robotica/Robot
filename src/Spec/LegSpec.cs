using System;

namespace Robot.Spec {
	/// <summary>
	/// Defines the specification of a leg
	/// </summary>
	public class LegSpec {
		private ServoSpec servoSpec = null;
		private WheelSpec wheelSpec = null;

		private int length = default;
		private int distanceToWheel = default;

		public LegSpec(ServoSpec servoSpec, WheelSpec wheelSpec, int length, int distanceToWheel) {
			this.servoSpec = servoSpec;
			this.wheelSpec = wheelSpec;

			this.length = length;
			this.distanceToWheel = distanceToWheel;
		}

		public ServoSpec GetServoSpec() {
			return this.servoSpec;
		}

		public WheelSpec GetWheelSpec() {
			return this.wheelSpec;
		}

		public int GetLength() {
			return this.length;
		}

		public int GetDistanceToWheel() {
			return this.distanceToWheel;
		}
	}
}