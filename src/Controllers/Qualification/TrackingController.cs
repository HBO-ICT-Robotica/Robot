using System.Threading;
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

		private Scalar lowerRange = new Scalar(90, 160, 30);
		private Scalar upperRange = new Scalar(140, 255, 255);

		private VirtualWindow.VirtualWindow virtualWindow = null;

		public TrackingController(Robot.Components.Robot robot) {
			this.robot = robot;

			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.videoCapture.Set(VideoCaptureProperties.BufferSize, 1);
			this.frame = new Mat();
			this.robot.GetBody().GoToRoot();

			this.virtualWindow = new VirtualWindow.VirtualWindow(this.frame);
			ServiceLocator.Get<VirtualWindowHost>().AddVirtualWindow(this.virtualWindow);

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)-40);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)-40);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)-40);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)-40);

			Thread.Sleep(1700);

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);

		}

		private void ResizeFrame() {
			this.videoCapture.Read(this.frame);
			Cv2.Resize(frame, frame, new Size(640, 480));

			MakeRegionOfInterest();
		}

		private void MakeRegionOfInterest() {
			Rect regionRect = new Rect(0, 0, frame.Size().Width, frame.Size().Height);
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
			var hierarchyIndex = OutputArray.Create(new Mat());
			var threshold = MakeThreshold();
			Cv2.FindContours(threshold, out Mat[] contours, hierarchyIndex, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

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

			Cv2.Rectangle(frame, approxRect, Scalar.Green, 2);

			return approxRect;
		}

		public Tuple<float, float, bool> CalculateLocation() {
			Rect targetRect = CalculateSquareRect(FindContours());

			if (targetRect == Rect.Empty)
				return new Tuple<float, float, bool>(0.0f, 0.0f, false);

			Size frameSize = resizedFrame.Size();

			var leftX = targetRect.X;
			var rightX = targetRect.X + targetRect.Width;

			return new Tuple<float, float, bool>(Map(leftX, 0, frameSize.Width, -1, 1), Map(rightX, 0, frameSize.Width, -1, 1), true);
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
			Tuple<float, float, bool> objectLocation = CalculateLocation();

			if (!objectLocation.Item3) {
				//Console.WriteLine("No blocky");
				this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);

				return;
			}

			Console.WriteLine((objectLocation.Item1 + objectLocation.Item2) / 2.0f);

			if ((objectLocation.Item1 > 0.2 && objectLocation.Item2 > 0.2) || (objectLocation.Item1 < -0.2 && objectLocation.Item2 < -0.2)) {
				Steer((objectLocation.Item1 + objectLocation.Item2) / 2.0f);
			} else {
				this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
				this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
			}
		}


		private void Steer(float objectLocation) {
			float winding = objectLocation < 0.0f ? 1 : -1;

			var frontLeftSpeed = 0.0f;
			var backLeftSpeed = 0.0f;
			var frontRightSpeed = 0.0f;
			var backRightSpeed = 0.0f;

			var safetyFactor = 0.5f;
			var carMaxSpeed = 70.0f;

			if (winding > 0.0f) {
				frontRightSpeed += safetyFactor * carMaxSpeed * 2.5f;
				backLeftSpeed -= safetyFactor * carMaxSpeed * 2.5f;
			} else {
				frontLeftSpeed += safetyFactor * carMaxSpeed * 2.5f;
				backRightSpeed -= safetyFactor * carMaxSpeed * 2.5f;
			}

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)(frontLeftSpeed * 0.87f));
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)(frontRightSpeed * 0.87f));
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)backLeftSpeed);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)backRightSpeed);
			/*
			Thread.Sleep(250);

			this.robot.GetBody().GetFrontBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetFrontBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[0].GetWheel().SetSpeed((int)0);
			this.robot.GetBody().GetBackBodyPart().GetLegs()[1].GetWheel().SetSpeed((int)0);
			*/
		}

		public void Dispose() {
			this.videoCapture.Dispose();
			this.frame.Dispose();
		}
	}
}