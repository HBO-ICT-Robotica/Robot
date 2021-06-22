using System;
using System.Threading;

namespace Robot.Controllers {
	public class DanceController : IRobotController {
		private Robot.Components.Robot robot = null;
		private Robot.Components.Leg leg = null;
		private Robot.Components.Servo servo = null;
		private Robot.Components.Motor motor = null;
		private Robot.Components.Gripper grijper = null;

		private float lastBeatTime = 0.0f;

		float currentTime = 0; // Current time of the song (Between 0 and lenth of song) in seconds. (Eg: 62.59f)
		int currentBar = 0; // Current bar
		int currentBeat = 0; // Current beat

		private float bpm = 120.4f;

		public DanceController(Robot.Components.Robot robot) {
			this.robot = robot;

			Console.WriteLine("3"); Console.Beep();
			Thread.Sleep(1000);
			Console.WriteLine("2"); Console.Beep();
			Thread.Sleep(1000);
			Console.WriteLine("1"); Console.Beep();
			Thread.Sleep(1000);
			Console.WriteLine("0"); Console.Beep();
		}

		public void Step(float dt) {
			bool isBar = false; // Is the next 'doStep' on a bar? 240
			bool isBeat = false; // Is the next 'doStep' on a beat?  121

			this.currentTime += dt;

			//Console.WriteLine(currentTime - lastBeatTime);
			if (currentTime - lastBeatTime >= 60 / this.bpm) {
				isBeat = true;
				currentBeat++;
					//Console.Beep();
				lastBeatTime = MathF.Round(currentTime / (60 / this.bpm)) * (60 / this.bpm);
				if ((currentBeat - 1) % 4 == 0) {
					isBar = true;
					currentBar++;
				}
			}

			DoStep(currentTime, isBar, isBeat, currentBar, currentBeat);
		}

		public void DoStep(float currentTime, bool isBar, bool isBeat, int currentBar, int currentBeat) {
			if (isBeat && currentBar >= 0 && currentBar <= 20) {
				Console.WriteLine(currentBeat);
				if (currentBeat % 2 == 0) {
					//robot.GetBody().GetFrontBodyPart().SetTargetHeight(108);
					robot.GetBody().GetBackBodyPart().SetTargetHeight(70);
					Console.WriteLine("Down");
				} else {
					//robot.GetBody().GetFrontBodyPart().SetTargetHeight(90);
					robot.GetBody().GetBackBodyPart().SetTargetHeight(108);
					Console.WriteLine("Up");
				}
			}
		}

		public void Dispose() {

		}
	}
}