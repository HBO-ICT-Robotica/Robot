using System;
using Robot.Serial;
using Robot.Utility;

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

			this.value = ;
			this.minValue = minValue;
			this.maxValue = maxValue;

			this.communicator.JoystickValueRecevied += OnJoystickValueReceived;
		}

		private void OnJoystickValueReceived(byte value) {
			throw new NotImplementedException();
		}
	}
}