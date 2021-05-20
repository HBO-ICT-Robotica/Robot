using System.Collections.Generic;
using Robot.Serial;
using Robot.Spec;

namespace Robot.Components {
	public class Body {
		private BodyPart frontBodyPart = null;
		private BodyPart backBodyPart = null;
		private BodyPart leftBodyPart = null;
		private BodyPart rightBodyPart = null;

		public Body(RobotSpec robotSpec, TeensyCommunicator teensyCommunicator) {
			var legSpec = robotSpec.GetLegSpec();
			var servoIds = robotSpec.GetServoIds();

			this.frontBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
				new Leg(legSpec, teensyCommunicator, servoIds.GetFrontLeftId(), 0),
				new Leg(legSpec, teensyCommunicator, servoIds.GetFrontRightId(), 0),
			});

			this.backBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
				new Leg(legSpec, teensyCommunicator, servoIds.GetBackLeftId(), 0),
				new Leg(legSpec, teensyCommunicator, servoIds.GetBackRightId(), 0),
			});

			this.leftBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
				new Leg(legSpec, teensyCommunicator, servoIds.GetFrontLeftId(), 0),
				new Leg(legSpec, teensyCommunicator, servoIds.GetBackLeftId(), 0),
			});

			this.rightBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
				new Leg(legSpec, teensyCommunicator, servoIds.GetFrontRightId(), 0),
				new Leg(legSpec, teensyCommunicator, servoIds.GetBackRightId(), 0),
			});
		}

		public BodyPart GetFrontBodyPart() {
			return this.frontBodyPart;
		}

		public BodyPart GetBackBodyPart() {
			return this.backBodyPart;
		}

		public BodyPart GetLeftBodyPart() {
			return this.leftBodyPart;
		}

		public BodyPart GetRightBodyPart() {
			return this.rightBodyPart;
		}
	}
}