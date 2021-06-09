namespace Robot.Components {
	public class Wheel {
		private Motor motor = null;

		public Wheel(Motor motor) {
			this.motor = motor;
		}

		public Motor GetMotor() {
			return this.motor;
		}

		public void SetSpeed(int speed) {
			if (speed < 0) {
				//this.GetMotor().SetMode(Motor.Mode.REVERSE);
				this.GetMotor().SetPwm(speed);
			} else {
				//this.GetMotor().SetMode(Motor.Mode.FORWARD);
				this.GetMotor().SetPwm(speed);
			}

		}
	}
}