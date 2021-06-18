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
		int currentBeat = -1; // Current beat

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
			currentBar += 1;

			if (isBar && currentBar >= 0 && currentBar <= 4) {
				Console.WriteLine("Draaiend overeind");
			}

			if (isBar && currentBar >= 5 && currentBar <= 12) {
				Console.WriteLine("Piroette");
			}

			if (isBar && currentBar >= 13 && currentBar <= 20) {
				Console.WriteLine("Twerken");
			}

			if (isBar && currentBar >= 21 && currentBar <= 28) {
				Console.WriteLine("Wiggelen");
			}

			if (isBar && currentBar >= 29 && currentBar <= 32) {
				Console.WriteLine("poot uitsteken en wiggelen");
			}	

			if (isBar && currentBar >= 33 && currentBar <= 36) {
				Console.WriteLine("andere poot uitsteken en wiggelen");
			}

			if (isBar && currentBar >= 37 && currentBar <= 44) {
				Console.WriteLine("russisch shuffelen");
			}

			if (isBar && currentBar >= 45 && currentBar <= 52) {
				Console.WriteLine("donuts");
			}

			if (isBar && currentBar >= 53 && currentBar <= 60) {
				Console.WriteLine("wiggelen");
			}

			if (isBar && currentBar >= 61) {
				Console.WriteLine("Buiging");
			}

			Console.WriteLine(currentBar);

		}

		public void Dispose() {

		}
	}
}