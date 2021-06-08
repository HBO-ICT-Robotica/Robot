using System;
using OpenCvSharp;
using Robot.Utility;
using Robot.VirtualWindow;

namespace Robot.Controllers {
	public class TrackingController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		//private Mat testFrame = null;

		private Scalar lowerRange = new Scalar(90, 160, 80);
		private Scalar upperRange = new Scalar(125, 255, 255);

		private VirtualWindow.VirtualWindow virtualWindow = null;
		//private VirtualWindow.VirtualWindow testWindow = null;

		private Rect targetRect = Rect.Empty;

		public TrackingController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();
			this.robot.GetBody().GoToRoot();

			this.virtualWindow = new VirtualWindow.VirtualWindow(this.frame);
			//this.testWindow = new VirtualWindow.VirtualWindow(this.testFrame);
			
			ServiceLocator.Get<VirtualWindowHost>().AddVirtualWindow(this.virtualWindow);
			//ServiceLocator.Get<VirtualWindowHost>().AddVirtualWindow(this.testWindow);
		}

		private void ResizeFrame() {

		}

		private Mat MakeMask() {
			var hsvFrame = new Mat();

			Cv2.CvtColor(frame, hsvFrame, ColorConversionCodes.BGR2HSV);
			Cv2.InRange(hsvFrame, lowerRange, upperRange, hsvFrame);

			return hsvFrame;
		}

		private Mat MaskedFrame() {
			var maskedFrame = new Mat();
			Cv2.BitwiseAnd(frame, frame, maskedFrame, MakeMask());
			return maskedFrame;
		}


		private Mat MakeThreshold() {
			var gray = new Mat();
			var grayThres = new Mat();

			Cv2.CvtColor(MaskedFrame(), gray, ColorConversionCodes.BGR2GRAY);
			Cv2.AdaptiveThreshold(gray, grayThres, 150, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 5, 5);

			return grayThres;
		}

		private Mat[] FindContours() {
			Mat hierarchyIndex = new Mat();
			Cv2.FindContours(MakeThreshold(), out Mat[] contours, hierarchyIndex, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

			return contours;
		}

		private void CalculateSquareRect(Mat[] contours) {
			for (int i = 0; i < contours.Length; i++) {
				Mat approx = new Mat();
				var epsilon = 0.1 * Cv2.ArcLength(contours[i], true);

				Cv2.ApproxPolyDP(contours[i], approx, epsilon, true);
				if (approx.Total() == 4) {
					Rect approxRect = Cv2.BoundingRect(approx);
					float ar = approxRect.Width / (float)approxRect.Height;

					if (ar > 0.3 && ar < 0.45) {
						targetRect = approxRect;
					}
				}
			}
		}



		private void CalculateLocation() {
			CalculateSquareRect(FindContours());

			Size frameSize = frame.Size();

			var middleX = (targetRect.Width / 2) + targetRect.X;
      var middleY = (targetRect.Height / 2) + targetRect.Y;

			var windowMiddleX = frameSize.Width / 2;
			Console.WriteLine($"X-as: {middleX}, Y-as: {middleY}");
		}

		public void Step(float dt) {
			this.videoCapture.Read(this.frame);
			//this.testFrame = MaskedFrame();
			CalculateLocation();
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}