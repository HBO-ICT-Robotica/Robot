using System.Collections.Generic;
using System.Linq;

namespace Robot.Components {
	public class BodyPart {
		private List<Leg> legs = null;

		public BodyPart(List<Leg> legs) {
			this.legs = new List<Leg>(legs);
		}

		public IReadOnlyList<Leg> GetLegs() {
			return this.legs;
		}

		public void SetTargetHeight(IDistance height) {
			foreach (var leg in legs) {
				leg.SetHeight(height.GetDistanceInMM());
			}
		}

		public int GetMaxHeight() {
			return this.legs.Max(leg => {
				return leg.GetMaxLength();
			});
		}

		public void GoToRoot() {
			foreach (var leg in legs) {
				leg.GoToRoot();
			}
		}
	}
}