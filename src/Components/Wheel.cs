namespace Robot.Components {
	public class Wheel {
		public enum Mode {
			BRAKE,
			FORWARD,
			REVERSE,
			NEUTRAL
		}

		private Motor motor = null;
		private Mode mode = Mode.BRAKE;

		public Wheel(Motor motor) {
			this.motor = motor;
		}

		public Motor GetMotor() {
			return this.motor;
		}

		public Mode GetMode() {
			return this.mode;
		}

		public void setMode(Mode mode) {
			this.mode = mode;
		}
	}
}