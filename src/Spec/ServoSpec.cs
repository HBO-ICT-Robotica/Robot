namespace Robot.Spec {
	/// <summary>
	/// Defines the specification a servo
	/// </summary>
	public class ServoSpec {
		private int minDegree = default;
		private int maxDegree = default;
		private int zeroDegree = default;

		public ServoSpec(int minDegree, int maxDegree, int zeroDegree) {
			this.minDegree = minDegree;
			this.maxDegree = maxDegree;
			this.zeroDegree = zeroDegree;
		}

		public int GetMinDegree() {
			return this.minDegree;
		}

		public int GetMaxDegree() {
			return this.maxDegree;
		}

		public int GetZeroDegree() {
			return this.zeroDegree;
		}
	}
}