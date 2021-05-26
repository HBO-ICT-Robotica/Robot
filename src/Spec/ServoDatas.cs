namespace Robot.Spec {
	public partial class RobotSpec {
		public class ServoDatas {
			public class ServoData {
				private byte id = default;
				private bool winding = default;

				public ServoData(byte id, bool winding) {
					this.id = id;
					this.winding = winding;
				}

				public byte GetId() {
					return this.id;
				}

				public bool GetWinding() {
					return this.winding;
				}
			}

			private ServoData frontLeft = null;
			private ServoData frontRight = null;
			private ServoData backLeft = null;
			private ServoData backRight = null;

			public ServoDatas(ServoData frontLeft, ServoData frontRight, ServoData backLeft, ServoData backRight) {
				this.frontLeft = frontLeft;
				this.frontRight = frontRight;
				this.backLeft = backLeft;
				this.backRight = backRight;
			}

			public ServoData GetFrontLeftData() {
				return this.frontLeft;
			}

			public ServoData GetFrontRightData() {
				return this.frontRight;
			}

			public ServoData GetBackLeftData() {
				return this.backLeft;
			}

			public ServoData GetBackRightData() {
				return this.backRight;
			}
		}
	}
}