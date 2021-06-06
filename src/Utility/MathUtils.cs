using System;

namespace Robot.Utility {
	public static class MathUtils {
		public static float RadToDeg(float radians) {
			return (180.0f / MathF.PI) * radians;
		}

		public static double RadToDeg(double radians) {
			return (180.0 / Math.PI) * radians;
		}

		public static float DegToRad(float degrees) {
			return (MathF.PI / 180.0f) * degrees;
		}

		public static double DegToRad(double degrees) {
			return (Math.PI / 180.0) * degrees;
		}

		public static int ToInt(float value) {
			return (int)MathF.Round(value);
		}

		public static int ToInt(double value) {
			return (int)Math.Round(value);
		}
	}
}