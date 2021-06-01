using System.Diagnostics;
using Robot.Serial;
using Robot.Utility;
using Robot.Utility.Easings;

namespace Robot.Components {
	public class Joystick {
		private TeensyCommunicator communicator = null;

		private int id = default;

		private int value = default;
		private int minValue = default;
		private int maxValue = default;

		public Joystick(int id, int minValue, int maxValue) {
			this.communicator = ServiceLocator.Get<TeensyCommunicator>();

			this.id = id;

			this.value = (int)Easings.LinearClamped(minValue, maxValue, 0.5f);
			this.minValue = minValue;
			this.maxValue = maxValue;

			this.communicator.JoystickValueRecevied += OnJoystickValueReceived;
		}

		public int GetValue() {
			return this.value;
		}

		public float GetRelativeValue() {
			return ((this.value - this.minValue) / (this.maxValue - this.minValue) * 2.0f) - 1.0f;
		}

		private void OnJoystickValueReceived(byte value) {
			this.value = value;
		}
	}
}