using System.Collections.Generic;
using Robot.Serial;
using Robot.Spec;

namespace Robot.Components {
	public class BodyPart {
		private RobotSpec robotSpec = null;

		private List<Leg> legs = null;

		public BodyPart(RobotSpec robotSpec, TeensyCommunicator teensyCommunicator, List<Leg> legs) {
			this.robotSpec = robotSpec;

			this.legs = new List<Leg>(legs);
		}

		public IReadOnlyList<Leg> GetServos() {
			return this.legs;
		}

		public void SetTargetHeight(float height) {
			// TODO: Implement this
		}

		public float GetMaxHeight() {
			// TODO: Implement this
			return 0.0f;
		}
	}
}