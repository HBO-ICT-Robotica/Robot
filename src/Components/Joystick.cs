using Robot.Serial;
using Robot.Utility;

namespace Robot.Components {
	public class Joystick {
		private IHardwareInterface hardwareInterface = null;

		private int id = default;

		private int value = default;
		private int minValue = default;
		private int maxValue = default;

		public Joystick(int id, int minValue, int maxValue) {
			this.hardwareInterface = ServiceLocator.Get<IHardwareInterface>();

			this.id = id;

			this.value = (int)this.Map(0.5f, 0.0f, 1.0f, minValue, maxValue);
			this.minValue = minValue;
			this.maxValue = maxValue;

			this.hardwareInterface.joystickValueReceived += OnJoystickValueReceived;
		}

		public int GetValue() {
			return this.value;
		}

		public float GetRelativeValue() {
			return this.Map(this.value, this.minValue, this.maxValue, -1.0f, 1.0f);
		}

		private void OnJoystickValueReceived(byte id, byte value) {
			if (id != this.id)
				return;

			this.value = value;
		}

		private float Map(float x, float inMin, float inMax, float outMin, float outMax) {
			return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}
}