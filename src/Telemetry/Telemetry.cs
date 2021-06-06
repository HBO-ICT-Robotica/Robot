using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCvSharp;
using Robot.Components;

namespace Robot.Telemetry {
	public class Telemetry {
		private static HttpClient httpClient = new HttpClient();

		private static string address = "http://192.168.137.1:3000/api/apiPushTelemetry";

		private class Package {
			public string image = string.Empty;
			public float[] servos = default;
			public int[] motors = default;
			public float[] targetDegrees = default;

			public Package(string image, float[] servos, float[] targetDegrees, int[] motors) {
				this.image = image;
				this.servos = servos;
				this.motors = motors;
				this.targetDegrees = targetDegrees;
			}
		}

		public static async Task DoRequest(Mat frame, Body body) {
			Cv2.ImEncode(".png", frame, out var buffer);
			var bufferAsText = Convert.ToBase64String(buffer);

			float[] servos = {
				body.GetFrontBodyPart().GetLegs()[0].GetServo().GetAngle().AsDegrees(),
				body.GetFrontBodyPart().GetLegs()[1].GetServo().GetAngle().AsDegrees(),
				body.GetBackBodyPart().GetLegs()[0].GetServo().GetAngle().AsDegrees(),
				body.GetBackBodyPart().GetLegs()[1].GetServo().GetAngle().AsDegrees(),
			};

			float[] targetDegrees = {
				body.GetFrontBodyPart().GetLegs()[0].GetServo().GetTargetAngle().AsDegrees(),
				body.GetFrontBodyPart().GetLegs()[1].GetServo().GetTargetAngle().AsDegrees(),
				body.GetBackBodyPart().GetLegs()[0].GetServo().GetTargetAngle().AsDegrees(),
				body.GetBackBodyPart().GetLegs()[1].GetServo().GetTargetAngle().AsDegrees(),
			};

			int[] motors = {
				body.GetFrontBodyPart().GetLegs()[0].GetWheel().GetMotor().GetPwm(),
				body.GetFrontBodyPart().GetLegs()[1].GetWheel().GetMotor().GetPwm(),
				body.GetBackBodyPart().GetLegs()[0].GetWheel().GetMotor().GetPwm(),
				body.GetBackBodyPart().GetLegs()[1].GetWheel().GetMotor().GetPwm(),
			};

			var package = new Package(bufferAsText, servos, targetDegrees, motors);

			var json = JsonConvert.SerializeObject(package);

			var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

			try {
				var responseBody = await httpClient.PostAsync(address, httpContent);
			} catch {

			}
		}
	}
}