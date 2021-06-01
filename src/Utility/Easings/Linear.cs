using System;
namespace Robot.Utility.Easings {
	public static partial class Easings {
		public static float Linear(float from, float to, float time) {
			return from + (to - from) * time;
		}

		public static float LinearClamped(float from, float to, float time) {
			return Math.Clamp(Linear(from, to, time), 0.0f, 1.0f);
		}
	}
}