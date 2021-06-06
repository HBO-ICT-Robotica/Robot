using Robot.Utility;
using Robot.Serial;

namespace Robot.Components {
	public class Motor {
		public enum Mode {
			BRAKE = 0,
			FORWARD = 1,
			REVERSE = 2,
			NEUTRAL = 3,
		}

		private TeensyCommunicator communicator = null;

		private byte id = default;

		private int pwm = default;

		private Mode mode = Mode.NEUTRAL;

		public Motor(byte id) {
			this.communicator = ServiceLocator.Get<TeensyCommunicator>();

			this.id = id;

			this.pwm = 0;
		}

		public void SetPwm(int pwm) {
			this.pwm = pwm;

			this.FlushPwm();
		}

		public void SetMode(Mode mode) {
			this.mode = mode;
		}

		private void FlushPwm() {
			this.communicator.SetMotorPwm(this.id, (byte)this.pwm);
		}

		private void FlushMode() {
			this.communicator.SetMotorMode(this.id, (byte)this.mode);
		}

		public int GetPwm() {
			return this.pwm;
		}
	}
}