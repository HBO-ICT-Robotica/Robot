using Robot.Steering;

namespace Robot.Controllers {
	public class RaceController : IRobotController {
		private Robot.Components.Robot robot = null;

		private WheelsControl steering;

		public RaceController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.robot.GoToRoot();

			this.steering = new WheelsControl(this.robot.GetSteeringJoystick(), this.robot.GetThrustJoystick(), 0.05f, 180);
		}

		public void Step(float dt) {
			var speed = this.steering.GetSpeed();

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.frontLeft);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.frontRight);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.backLeft);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.backRight);
		}

		public void Dispose() {
		}
	}
}