namespace Robot.Spec {
	/// <summary>
	/// Defines the specification a robot
	/// </summary>
	public partial class RobotSpec {
		private LegSpec legSpec = null;
		private GripperSpec gripperSpec = null;

		private ServoDatas servoDatas = null;

		public RobotSpec(LegSpec legSpec, GripperSpec gripperSpec, ServoDatas servoDatas) {
			this.legSpec = legSpec;
			this.gripperSpec = gripperSpec;

			this.servoDatas = servoDatas;
		}

		public LegSpec GetLegSpec() {
			return this.legSpec;
		}

		public GripperSpec GetGripperSpec() {
			return this.gripperSpec;
		}

		public ServoDatas GetServoDatas() {
			return this.servoDatas;
		}
	}
}