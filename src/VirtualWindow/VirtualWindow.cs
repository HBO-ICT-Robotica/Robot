using OpenCvSharp;

namespace Robot.VirtualWindow {
	public class VirtualWindow {
		private Mat image = null;

		public VirtualWindow(Mat image) {
			this.image = image;
		}

		public Mat GetImage() {
			return this.image;
		}
	}
}