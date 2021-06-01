using System.Threading;
using Robot.Components;
using Robot.Serial;
using Robot.Spec;
using Robot.Utility;
using System;
using Newtonsoft.Json;
using OpenCvSharp;
using System.Threading.Tasks;
using System.Net.Http;

namespace Robot.Controllers {
	public class TestController : IRobotController {

		HttpClient httpClient = new HttpClient();

		string address = "http://192.168.137.1:3000/api/apiPushTelemetry";

		private class Package {
			public string image = string.Empty;
			public int servo0 = default;
			public int servo1 = default;
			public int servo2 = default;
			public int servo3 = default;
			public int motor0 = default;
			public int motor1 = default;
			public int motor2 = default;
			public int motor3 = default;
			public int targetDegree0 = default;
			public int targetDegree1 = default;
			public int targetDegree2 = default;
			public int targetDegree3 = default;


			public Package(string image, int servo0, int servo1, int servo2, int servo3, int motor0, int motor1, int motor2, int motor3, int targetDegree0, int targetDegree1, int targetDegree2, int targetDegree3) {
				this.image = image;
				this.servo0 = servo0;
				this.servo1 = servo1;
				this.servo2 = servo2;
				this.servo3 = servo3;
				this.motor0 = motor0;
				this.motor1 = motor1;
				this.motor2 = motor2;
				this.motor3 = motor3;
				this.targetDegree0 = targetDegree0;
				this.targetDegree1 = targetDegree1;
				this.targetDegree2 = targetDegree2;
				this.targetDegree3 = targetDegree3;
			
			}
		}

		public static async Task DoRequest(HttpClient httpClient, string address, Mat frame, Body body) {
			Cv2.ImEncode(".png", frame, out var buffer);
			var bufferAsText = Convert.ToBase64String(buffer);

			var servo0 = body.GetFrontBodyPart().GetLegs()[0].GetServo().GetDegree();
			var servo1 = body.GetFrontBodyPart().GetLegs()[1].GetServo().GetDegree();
			var servo2 = body.GetBackBodyPart().GetLegs()[0].GetServo().GetDegree();
			var servo3 = body.GetBackBodyPart().GetLegs()[1].GetServo().GetDegree();

			var targetDegree0 = body.GetFrontBodyPart().GetLegs()[0].GetServo().GetTargetDegree();
			var targetDegree1 = body.GetFrontBodyPart().GetLegs()[1].GetServo().GetTargetDegree();
			var targetDegree2 = body.GetBackBodyPart().GetLegs()[0].GetServo().GetTargetDegree();
			var targetDegree3 = body.GetBackBodyPart().GetLegs()[1].GetServo().GetTargetDegree();

			var motor0 = body.GetFrontBodyPart().GetLegs()[0].GetWheel().GetMotor().GetPwm();
			var motor1 = body.GetFrontBodyPart().GetLegs()[1].GetWheel().GetMotor().GetPwm();
			var motor2 = body.GetBackBodyPart().GetLegs()[0].GetWheel().GetMotor().GetPwm();
			var motor3 = body.GetBackBodyPart().GetLegs()[1].GetWheel().GetMotor().GetPwm();

			var package = new Package(bufferAsText, servo0, servo1, servo2, servo3, motor0, motor1, motor2, motor3, targetDegree1, targetDegree0, targetDegree2, targetDegree3);

			var json = JsonConvert.SerializeObject(package);

			var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

			try {
				var responseBody = await httpClient.PostAsync(address, httpContent);
			}
			catch {

			}

		}

		public TestController(Robot.Components.Robot robot) {
			var body = robot.GetBody();

			var front = body.GetFrontBodyPart();
			var back = body.GetBackBodyPart();
			var left = body.GetLeftBodyPart();
			var right = body.GetRightBodyPart();

			body.GoToRoot();
			Console.WriteLine("Going to root");

			var communicator = ServiceLocator.Get<TeensyCommunicator>();

			communicator.JoystickValueRecevied += (value) => {

				var val = value - 32;

				if (val >= 0) {
					var pwm = (int)((val / 31.0f) * 255);

					Console.WriteLine("Pwm: " + pwm);
					foreach (var leg in body.GetLegs()) {
						var wheel = leg.GetWheel();
						var motor = wheel.GetMotor();


						motor.SetPwm(pwm);
					}
				}
			};



			// front.SetTargetHeight(100);
			// back.SetTargetHeight(100);


			try {
				var videoCapture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);

				using var frame = new Mat();
				//using var window = new Window("src");

				//using var frame = Cv2.ImRead("sans.png", ImreadModes.Color);

				while (true) {
					videoCapture.Read(frame);


					if (frame.Empty())
						continue;

					//window.Image = frame;

					//using var newFrame = frame.Resize(new Size(240, 135));

					DoRequest(httpClient, address, frame, body).Wait();

					Thread.Sleep(1000 / 24);
				}
			} catch (Exception e) {
				Console.WriteLine(e);
			}

			// front.SetTargetHeight(front.GetMaxHeight());
			// back.SetTargetHeight(front.GetMaxHeight());

			// Thread.Sleep(1000);

			// left.SetTargetHeight(0);

			// Thread.Sleep(1000);
			// right.SetTargetHeight(0);

			// Thread.Sleep(1000);
			// front.SetTargetHeight(front.GetMaxHeight());

			// Thread.Sleep(1000);
			// back.SetTargetHeight(back.GetMaxHeight());
		}
	}
}