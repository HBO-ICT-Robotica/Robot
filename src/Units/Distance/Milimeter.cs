namespace Robot.Units.Distance {
	public class Milimeter : IDistance {
		private int height;

		public Milimeter(int height) {
			this.height = height;
		}

		public int GetDistanceInMM() {
			return height;
		}
	}
}