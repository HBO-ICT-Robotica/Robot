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

		public IReadOnlyList<Leg> GetLegs() {
			var legs = new List<Leg>();

			legs.AddRange(this.GetFrontBodyPart().GetLegs());
			legs.AddRange(this.GetBackBodyPart().GetLegs());

			return legs;
		}
	}
}