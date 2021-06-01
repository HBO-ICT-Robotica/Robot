using Robot.Utility;
using Robot.Serial;

namespace Robot.Components {
	public class Motor {
		private TeensyCommunicator communicator = null;

		private byte id = default;

		private int pwm = default;

		public Motor(byte id) {
			this.communicator = ServiceLocator.Get<TeensyCommunicator>();

			this.id = id;

			this.pwm = 0;
		}

		public void SetPwm(int pwm) {
			this.pwm = pwm;

			this.FlushPwm();
		}

		private void FlushPwm() {
			this.communicator.SetMotorPwm(this.id, (byte)this.pwm);
		}

		public int GetPwm() {
			return this.pwm;
		}
	}
}