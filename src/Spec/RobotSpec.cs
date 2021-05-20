namespace Robot.Spec {
	/// <summary>
	/// Defines the specification a robot
	/// </summary>
	public partial class RobotSpec {
		private LegSpec legSpec = null;
		private GripperSpec gripperSpec = null;

		private ServoIds servoIds = null;

		public RobotSpec(LegSpec legSpec, GripperSpec gripperSpec, ServoIds servoIds) {
			this.legSpec = legSpec;
			this.gripperSpec = gripperSpec;

			this.servoIds = servoIds;
		}

		public LegSpec GetLegSpec() {
			return this.legSpec;
		}

		public GripperSpec GetGripperSpec() {
			return this.gripperSpec;
		}

		public ServoIds GetServoIds() {
			return this.servoIds;
		}
	}
}