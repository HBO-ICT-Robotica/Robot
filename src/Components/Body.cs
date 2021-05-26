using System.Collections.Generic;
using Robot.Serial;
using Robot.Spec;

namespace Robot.Components {
	public class Body {
		private BodyPart frontBodyPart = null;
		private BodyPart backBodyPart = null;
		private BodyPart leftBodyPart = null;
		private BodyPart rightBodyPart = null;

		public Body(BodyPart frontBodyPart, BodyPart backBodyPart, BodyPart leftBodyPart, BodyPart rightBodyPart) {
			this.frontBodyPart = frontBodyPart;
			this.backBodyPart = backBodyPart;
			this.leftBodyPart = leftBodyPart;
			this.rightBodyPart = rightBodyPart;

			// var legSpec = robotSpec.GetLegSpec();
			// var servoDatas = robotSpec.GetServoDatas();

			// this.frontBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetFrontLeftData()),
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetFrontRightData()),
			// });

			// this.backBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetBackLeftData()),
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetBackRightData()),
			// });

			// this.leftBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetFrontLeftData()),
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetBackLeftData()),
			// });

			// this.rightBodyPart = new BodyPart(robotSpec, teensyCommunicator, new List<Leg>() {
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetFrontRightData()),
			// 	new Leg(legSpec, teensyCommunicator, servoDatas.GetBackRightData()),
			// });
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

		public void GoToRoot() {
			this.frontBodyPart.GoToRoot();
			this.backBodyPart.GoToRoot();
		}
	}
}