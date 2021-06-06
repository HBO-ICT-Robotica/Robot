using Robot.Utility;

namespace Robot.Units.Angle {
	public class Degrees : IAngle {
		private float angle = default;

		public Degrees(float angle) {
			this.angle = angle;
		}

		public float AsDegrees() {
			return this.angle;
		}

		public float AsRadians() {
			return MathUtils.DegToRad(this.angle);
		}

		public static Degrees operator +(Degrees a) {
			return new Degrees(a.AsDegrees());
		}

		public static Degrees operator -(Degrees a) {
			return new Degrees(-a.AsDegrees());
		}

		public static Degrees operator +(Degrees a, IAngle b) {
			return new Degrees(a.AsDegrees() + b.AsDegrees());
		}

		public static Degrees operator -(Degrees a, IAngle b) {
			return new Degrees(a.AsDegrees() - b.AsDegrees());
		}

		public override string ToString() {
			return $"{this.angle} deg";
		}
	}
}