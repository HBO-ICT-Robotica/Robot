using System;

namespace Robot.Spec {
	/// <summary>
	/// Defines the specification of a gripper
	/// </summary>
	public class GripperSpec {
		private ServoSpec servoSpec = null;
		private LoadCellSpec loadCellSpec = null;

		public GripperSpec(ServoSpec servoSpec, LoadCellSpec loadCellSpec) {
			this.servoSpec = servoSpec;
			this.loadCellSpec = loadCellSpec;
		}

		public ServoSpec GetServoSpec() {
			return this.servoSpec;
		}

		public LoadCellSpec GetLoadCellSpec() {
			return this.loadCellSpec;
		}
	}
}