namespace Robot.Components {
	public class Wheel {
		private Motor motor = null;

		public Wheel(Motor motor) {
			this.motor = motor;
		}

		public Motor GetMotor() {
			return this.motor;
		}
	}
}