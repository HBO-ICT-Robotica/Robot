using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Robot.Utility {
	public class Vision {
		private VideoCapture videoCapture = null;
		private Mat frame;
		private Mat frameROI;

		private Scalar lowerRange;
		private Scalar upperRange;

		private Rect regionRect;

		private bool useRegionRect;

		public Vision() {
			this.videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);
			this.videoCapture.Set(VideoCaptureProperties.BufferSize, 1);

			this.frame = new Mat();

			this.regionRect = Rect.Empty;
			this.lowerRange = new Scalar();
			this.upperRange = new Scalar();

			this.useRegionRect = false;
		}

		public void SetupSearch(Scalar lowerRange, Scalar upperRange, Rect regionRect) {
			this.lowerRange = lowerRange;
			this.upperRange = upperRange;
			this.regionRect = regionRect;

			if (regionRect == Rect.Empty)
				this.useRegionRect = false;
		}

		private void ReziseFrame() {
			this.videoCapture.Read(this.frame);
			Cv2.Resize(frame, frame, new Size(640, 480));

			MakeRegionOfInterest();
		}

		private void MakeRegionOfInterest() {
			if (useRegionRect)
				frameROI = new Mat(frame, regionRect);
			else
				frameROI = new Mat(frame);
		}

		private Mat MakeMask() {
			var hsvFrame = new Mat();

			Cv2.CvtColor(frameROI, hsvFrame, ColorConversionCodes.BGR2HSV);
			Cv2.InRange(hsvFrame, lowerRange, upperRange, hsvFrame);

			return hsvFrame;
		}

		private Mat MaskedFrame() {
			var maskedFrame = new Mat();
			Cv2.BitwiseAnd(frameROI, frameROI, maskedFrame, MakeMask());

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

		private Rect CalculateRect(Mat[] contours) {
			if (contours.Length <= 1)
				return Rect.Empty;

			Array.Sort(contours, (x, y) => y.ContourArea().CompareTo(x.ContourArea()));

			Mat biggestContour = contours[1];

			Mat approx = new Mat();
			var epsilon = 0.1f * biggestContour.ArcLength(true);

			Cv2.ApproxPolyDP(biggestContour, approx, epsilon, true);
			Rect approxRect = Cv2.BoundingRect(approx);

			return approxRect;
		}

		public Tuple<float, float, bool> CalculateDirection() {
			Rect targetRect = CalculateRect(FindContours());

			if (targetRect == Rect.Empty)
				return new Tuple<float, float, bool>(0.0f, 0.0f, false);

			Size frameSize = frameROI.Size();

			var leftX = targetRect.X;
			var rightX = targetRect.X + targetRect.Width;

			return new Tuple<float, float, bool>(Map(leftX, 0, frameSize.Width, -1, 1), Map(rightX, 0, frameSize.Width, -1, 1), true);
		}

		public Tuple<Rect, bool> CalculateLocation() {
			Rect targetRect = CalculateRect(FindContours());

			if (targetRect == Rect.Empty)
				return new Tuple<Rect, bool>(targetRect, false);

			return new Tuple<Rect, bool>(targetRect, true);
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

	}
}