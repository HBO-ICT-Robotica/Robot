using Robot.Utility;
using Robot.Serial;

namespace Robot.Components {
	public class LoadCell {
		private int value = 0;

		public LoadCell() {
			var hardwareInterface = ServiceLocator.Get<IHardwareInterface>();

			hardwareInterface.loadCellValueUpdated += OnLoadCellValueUpdated;
		}

		public int GetWeight() {
			return this.value;
		}

		private void OnLoadCellValueUpdated(int value) {
			this.value = value;
		}
	}
}