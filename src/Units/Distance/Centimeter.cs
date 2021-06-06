namespace Robot.Units.Distance {
	public class Centimeter : IDistance{
		private int height;

		public Centimeter(int height) {
			this.height = height;
		}

		public int GetDistanceInMM () {
			return height * 10;
		}
	}
}