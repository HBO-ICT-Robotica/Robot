using System;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Components {
	public class Meter : IDistance{

		private int HeightM;

		public Meter(int height) {
			this.HeightM = height;
		}

		public int GetDistanceInMM () {
			return HeightM * 1000;
		}
	}
}