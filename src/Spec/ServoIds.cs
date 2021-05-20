namespace Robot.Spec {
	public partial class RobotSpec {
		public class ServoIds {
			private byte frontLeftId = default;
			private byte frontRightId = default;
			private byte backLeftId = default;
			private byte backRightId = default;

			public ServoIds(byte frontLeftId, byte frontRightId, byte backLeftId, byte backRightId) {
				this.frontLeftId = frontLeftId;
				this.frontRightId = frontRightId;
				this.backLeftId = backLeftId;
				this.backRightId = backRightId;
			}

			public byte GetFrontLeftId() {
				return this.frontLeftId;
			}

			public byte GetFrontRightId() {
				return this.frontRightId;
			}

			public byte GetBackLeftId() {
				return this.backLeftId;
			}

			public byte GetBackRightId() {
				return this.backRightId;
			}
		}
	}
}