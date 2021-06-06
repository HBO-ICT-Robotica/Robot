using Robot.Utility;

namespace Robot.Units.Angle {
	public struct Radians : IAngle {
		private float angle;

		public Radians(float angle) {
			this.angle = angle;
		}

		public float AsDegrees() {
			return MathUtils.RadToDeg(this.angle);
		}

		public float AsRadians() {
			return this.angle;
		}

		public static Radians operator +(Radians a) {
			return new Radians(a.AsRadians());
		}

		public static Radians operator -(Radians a) {
			return new Radians(-a.AsRadians());
		}

		public static Radians operator +(Radians a, IAngle b) {
			return new Radians(a.AsRadians() + b.AsRadians());
		}

		public static Radians operator -(Radians a, IAngle b) {
			return new Radians(a.AsRadians() - b.AsRadians());
		}

		public override string ToString() {
			return $"{this.angle} rad";
		}
	}
}