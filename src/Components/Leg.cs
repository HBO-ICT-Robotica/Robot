using System;
using Robot.Serial;
using Robot.Spec;
using static Robot.Spec.RobotSpec.ServoDatas;

namespace Robot.Components {
	public class Leg {
		private Servo servo = null;
		private Wheel wheel = null;

		public Leg(Servo servo, Wheel wheel) {
			this.servo = servo;
			this.wheel = wheel;
		}

		public Servo GetServo() {
			return this.servo;
		}

		public Wheel GetWheel() {
			return this.wheel;
		}

		public void SetHeight(int height) {
			
		}

		public int GetLength() {
			var aSquared = this.legSpec.GetLength() * 2;
			var bSquared = this.legSpec.GetDistanceToWheel() * 2;
			var cSquared = aSquared + bSquared;

			var length = (int)(MathF.Sqrt(cSquared));

			return length;
		}

		public void GoToRoot() {
			this.servo.GoToRoot();
			// TODO: Move wheel with servo
		}
	}
}