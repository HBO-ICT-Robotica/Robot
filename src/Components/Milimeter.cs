using System;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Components {
	public class Milimeter : IDistance{

		private int HeightMM;

		public Milimeter(int height) {
			this.HeightMM = height;
		}

		public int GetDistanceInMM () {
			return HeightMM;
		}
	}
}