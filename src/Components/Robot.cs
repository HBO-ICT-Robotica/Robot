namespace Robot.Components {
	public class Robot {
		private Body body = null;

		private Joystick leftJoystick = null;
		private Joystick rightJoystick = null;
		private Gripper gripper = null;

		public Robot(Body body, Joystick leftJoystick, Joystick rightJoystick, Gripper gripper) {
			this.body = body;

			this.leftJoystick = leftJoystick;
			this.rightJoystick = rightJoystick;

			this.gripper = gripper;
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

		public Gripper GetGripper(){
			return this.gripper;
		}

	}
}