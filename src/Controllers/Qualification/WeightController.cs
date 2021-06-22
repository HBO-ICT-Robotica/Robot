using Robot.Serial;
using Robot.Utility;
using Robot.Steering;

namespace Robot.Controllers {
	public class WeightController : IRobotController {
		private Robot.Components.Robot robot = null;

		private WheelsControl steering;

		public WeightController(Robot.Components.Robot robot) {
			this.robot = robot;

			var hardwareInterface = ServiceLocator.Get<IHardwareInterface>();

			this.steering = new WheelsControl(this.robot.GetSteeringJoystick(), this.robot.GetThrustJoystick());
		}

		public void Step(float dt) {
			var thrustJoystick = this.robot.GetThrustJoystick();
			var steeringJoystick = this.robot.GetSteeringJoystick();

			var speed = this.steering.GetSpeed();

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.frontLeft);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.frontRight);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)speed.backLeft);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)speed.backRight);


			// 	Thread.Sleep(1000);

				// foreach (var leg in this.robot.GetBody().GetFrontBodyPart().GetLegs()) {
				// 	var wheel = leg.GetWheel();
				// 	//wheel.SetSpeed(this.steering.GetLeftSpeed());
				// 	wheel.SetSpeed(30);
				// }

			// 	foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
			// 		var wheel = leg.GetWheel();
			// 		//wheel.SetSpeed(this.steering.GetRightSpeed());
			// 		wheel.SetSpeed(-30);
			// 	}

			// 	Thread.Sleep(1000);
			// }

			// var gripper = robot.GetGripper();
			// gripper.Open();
			// gripper.Close(Components.Gripper.Pickupable.BALL);

		}
		public void Dispose() {

		}
	}
}