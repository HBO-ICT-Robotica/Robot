namespace Robot.Controllers {
	public class DanceController : IRobotController {
		private Robot.Components.Robot robot = null;

		private float time = 0.0f;

		public DanceController(Robot.Components.Robot robot) {
			this.robot = robot;
		}

		public void Step(float dt) {
			time += dt;

			float currentTime = time; // Current time of the song (Between 0 and lenth of song) in seconds. (Eg: 62.59f)
			bool isBar = false; // Is the next 'doStep' on a bar?
			bool isBeat = false; // Is the next 'doStep' on a beat?
			int currentBar = 0; // Current bar
			int currentBeat = 0; // Current beat

			DoStep(currentTime, isBar, isBeat, currentBar, currentBeat);
		}

		public void DoStep(float currentTime, bool isBar, bool isBeat, int currentBar, int currentBeat) {
			// Hier komt de dans
		}

		public void Dispose() {

		}
	}
}