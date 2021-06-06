using System;
using OpenCvSharp;
using Robot.Utility;
using Robot.VirtualWindow;

namespace Robot.Controllers {
	public class TrackingController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		private VirtualWindow.VirtualWindow virtualWindow = null;

		public TrackingController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();
			this.robot.GetBody().GoToRoot();

			this.virtualWindow = new VirtualWindow.VirtualWindow(this.frame);

			ServiceLocator.Get<VirtualWindowHost>().AddVirtualWindow(this.virtualWindow);
		}

		public void Step(float dt) {
			this.videoCapture.Read(this.frame);
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}