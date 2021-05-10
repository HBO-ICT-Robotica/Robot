using System;
using OpenCvSharp;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program {
	public static void Main(string[] args) {
		var httpClient = new HttpClient();

		string address = "http://localhost:3000/api/apiPushTelemetry";

		try {
			var videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);

			using var frame = new Mat();
			using var window = new Window("src");

			//using var frame = Cv2.ImRead("sans.png", ImreadModes.Color);

			while (true) {
				videoCapture.Read(frame);

				if (frame.Empty())
					continue;

				window.Image = frame;

				//using var newFrame = frame.Resize(new Size(240, 135));

				DoRequest(httpClient, address, frame).Wait();

				Cv2.WaitKey(1000 / 24);
			}
		} catch (Exception e) {
			Console.WriteLine(e);
		}
	}

	private class Package {
		public string image = string.Empty;

		public Package(string image) {
			this.image = image;
		}
	}

	public static async Task DoRequest(HttpClient httpClient, string address, Mat frame) {
		Cv2.ImEncode(".png", frame, out var buffer);
		var bufferAsText = Convert.ToBase64String(buffer);

		var package = new Package(bufferAsText);

		var json = JsonConvert.SerializeObject(package);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var responseBody = await httpClient.PostAsync(address, httpContent);
	}
}