namespace Robot.Components {
	public class Robot {
		private Body body = null;

		public Robot(Body body) {
			this.body = body;
		}

		public Body GetBody() {
			return this.body;
		}

		public void GoToRoot() {
			this.body.GoToRoot();
		}
	}
}