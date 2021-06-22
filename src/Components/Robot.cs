namespace Robot.Components {
	public class Robot {
		private Body body = null;

		private Joystick steeringJoystick = null;
		private Joystick thrustJoystick = null;

		private Joystick frontHeightJoystick = null;
		private Joystick backHeightJoystick = null;

		private Gripper gripper = null;

		public Robot(Body body, Joystick steeringJoystick, Joystick thrustJoystick, Joystick frontHeightJoystick, Joystick backHeightJoystick, Gripper gripper) {
			this.body = body;

			this.steeringJoystick = steeringJoystick;
			this.thrustJoystick = thrustJoystick;

			this.frontHeightJoystick = frontHeightJoystick;
			this.backHeightJoystick = backHeightJoystick;

			this.gripper = gripper;
		}

		public Body GetBody() {
			return this.body;
		}

		public Joystick GetSteeringJoystick() {
			return this.steeringJoystick;
		}

		public Joystick GetThrustJoystick() {
			return this.thrustJoystick;
		}

		public Joystick GetFrontHeightJoystick() {
			return this.frontHeightJoystick;
		}

		public Joystick GetBackHeightJoystick() {
			return this.backHeightJoystick;
		}

		public void GoToRoot() {
			this.body.GoToRoot();
		}

		public Gripper GetGripper(){
			return this.gripper;
		}

	}
}