namespace Robot.Components {
	public class Robot {
		private Body body = null;

		private Joystick leftJoystick = null;
		private Joystick rightJoystick = null;

		public Robot(Body body, Joystick leftJoystick, Joystick rightJoystick) {
			this.body = body;

			this.leftJoystick = leftJoystick;
			this.rightJoystick = rightJoystick;
		}

		public Body GetBody() {
			return this.body;
		}

		public Joystick GetLeftJoystick() {
			return this.leftJoystick;
		}

		public Joystick GetRightJoystick() {
			return this.rightJoystick;
		}

		public void GoToRoot() {
			this.body.GoToRoot();
		}
	}
}