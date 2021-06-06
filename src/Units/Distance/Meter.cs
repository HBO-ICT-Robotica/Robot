namespace Robot.Units.Distance {
	public class Meter : IDistance{
		private int height;

		public Meter(int height) {
			this.height = height;
		}

		public int GetDistanceInMM () {
			return height * 1000;
		}
	}
}