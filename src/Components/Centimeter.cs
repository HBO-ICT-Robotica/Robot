using System;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Components {
	public class Centimeter : IDistance{

		private int HeightCM;

		public Centimeter(int height) {
			this.HeightCM = height;
		}

		public int GetDistanceInMM () {
			return HeightCM * 10;
		}
	}
}