using System;
using Robot.Units.Angle;
using Robot.Units.Distance;

namespace Robot.Components {
	public class Leg {
		private Servo servo = null;
		private Wheel wheel = null;

		private int length = default;
		private int distanceToWheel = default;

		public Leg(Servo servo, Wheel wheel, int length, int distanceToWheel) {
			this.servo = servo;
			this.wheel = wheel;

			this.length = length;
			this.distanceToWheel = distanceToWheel;
		}

		public Servo GetServo() {
			return this.servo;
		}

		public Wheel GetWheel() {
			return this.wheel;
		}

		public void SetHeight(int height, int offset = 0) {
			var length = this.GetMaxLength();

			var targetAngle = MathF.Acos((float)height / length);

			var targetAngleDegrees = (int)(targetAngle * (180.0 / MathF.PI)) + 45;

			this.servo.SetTargetAngle(targetAngleDegrees + offset);
		}

		public int GetMaxLength() {
			var aSquared = this.length * this.length;
			var bSquared = this.distanceToWheel * this.distanceToWheel;
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