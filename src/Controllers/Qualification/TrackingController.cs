using System;
using OpenCvSharp;
using Robot.Utility;
using Robot.VirtualWindow;

namespace Robot.Controllers {
	public class TrackingController : IRobotController {
		private Robot.Components.Robot robot = null;

		private VideoCapture videoCapture = null;
		private Mat frame = null;

		private Mat resizedFrame = null;

		private Scalar lowerRange = new Scalar(90, 160, 50);
		private Scalar upperRange = new Scalar(125, 255, 255);

		private VirtualWindow.VirtualWindow virtualWindow = null;

		public TrackingController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.frame = new Mat();
			this.robot.GetBody().GoToRoot();

			this.virtualWindow = new VirtualWindow.VirtualWindow(this.frame);
			ServiceLocator.Get<VirtualWindowHost>().AddVirtualWindow(this.virtualWindow);
		}

		private void ResizeFrame() {
			this.videoCapture.Read(this.frame);
			Cv2.Resize(frame, frame, new Size(640, 480));

			MakeRegionOfInterest();
		}

		private void MakeRegionOfInterest() {
			Rect regionRect = new Rect(0, 0, frame.Size().Width, frame.Size().Height / 2);
			resizedFrame = new Mat(frame, regionRect);
		}

		private Mat MakeMask() {
			var hsvFrame = new Mat();

			Cv2.CvtColor(resizedFrame, hsvFrame, ColorConversionCodes.BGR2HSV);
			Cv2.InRange(hsvFrame, lowerRange, upperRange, hsvFrame);

			return hsvFrame;
		}

		private Mat MaskedFrame() {
			var maskedFrame = new Mat();
			Cv2.BitwiseAnd(resizedFrame, resizedFrame, maskedFrame, MakeMask());

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

		private Rect CalculateSquareRect(Mat[] contours) {
			if (contours.Length <= 1)
				return Rect.Empty;

			Array.Sort(contours, (x, y) => y.ContourArea().CompareTo(x.ContourArea()));

			Mat biggestContour = contours[1];

			Mat approx = new Mat();
			var epsilon = 0.1f * biggestContour.ArcLength(true);

			Cv2.ApproxPolyDP(biggestContour, approx, epsilon, true);

			Rect approxRect = Cv2.BoundingRect(approx);
			float ar = approxRect.Width / (float)approxRect.Height;

			Cv2.Rectangle(frame, approxRect, Scalar.BurlyWood, 2);

			return approxRect;
		}

		public Tuple<float, bool> CalculateLocation() {
			Rect targetRect = CalculateSquareRect(FindContours());

			if (targetRect == Rect.Empty)
				return new Tuple<float, bool>(0.0f, false);

			Size frameSize = frame.Size();

			var middleX = (targetRect.Width / 2) + targetRect.X;

			return new Tuple<float, bool>(Map(middleX, 0, frameSize.Width, -1, 1), true);
		}


		private float Map(float from, float fromMin, float fromMax, float toMin, float toMax) {
			var fromAbs = from - fromMin;
			var fromMaxAbs = fromMax - fromMin;

			var normal = fromAbs / fromMaxAbs;

			var toMaxAbs = toMax - toMin;
			var toAbs = toMaxAbs * normal;

			var to = toAbs + toMin;

			return to;
		}

		public void Step(float dt) {
			ResizeFrame();
			Tuple<float, bool> objectLocation = CalculateLocation();

			if (!objectLocation.Item2)
				return;

			if (objectLocation.Item1 > 0.01f || objectLocation.Item1 < -0.01f)
				Steer(objectLocation.Item1);

		}


		private void Steer(float objectLocation) {
			foreach (var leg in this.robot.GetBody().GetLeftBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed((int)(objectLocation * 10));
			}

			foreach (var leg in this.robot.GetBody().GetRightBodyPart().GetLegs()) {
				var wheel = leg.GetWheel();
				wheel.SetSpeed((int)objectLocation * -10);
			}
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}