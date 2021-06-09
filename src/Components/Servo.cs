using Robot.Serial;
using Robot.Units.Angle;
using Robot.Utility;

namespace Robot.Components {
	public class Servo {
		private TeensyInterface hardwareInterface = null;

		private byte id = default;
		private bool winding = default;

		private IAngle rootAngle = default;
		private IAngle minAngle = default;
		private IAngle maxAngle = default;

		private IAngle targetAngle = default;
		private IAngle angle = default;

		private bool ledState = default;

		private ushort speed = default;

		public Servo(byte id, bool winding, IAngle rootAngle, IAngle minAngle, IAngle maxAngle) {
			this.hardwareInterface = ServiceLocator.Get<TeensyInterface>();

			this.id = id;
			this.winding = winding;

			this.rootAngle = rootAngle;
			this.minAngle = minAngle;
			this.maxAngle = maxAngle;

			this.targetAngle = new Radians(0.0f);
			this.angle = new Radians(0.0f);

			this.ledState = false;

			this.speed = 1023 / 4;

			this.hardwareInterface.servoPositionUpdated += OnServoPositionUpdated;
			//this.SetTargetAngle(rootAngle);
			//this.FlushState();
		}

		private void OnServoPositionUpdated(byte id, ushort position) {
			if (id != this.id)
				return;

			this.angle = new Degrees(position);
		}

		public IAngle GetRootDegree() {
			return this.rootAngle;
		}

		public void SetTargetAngle(IAngle angle) {
			if (this.winding)
				this.targetAngle = new Degrees(300.0f) - angle;
			else
				this.targetAngle = angle;

			this.FlushTargetAngle();
		}

		private void FlushTargetAngle() {
			this.hardwareInterface.SetServoTargetDegree(this.id, (ushort)(this.targetAngle).AsDegrees());
		}

		public IAngle GetAngle() {
			return this.angle;
		}

		public IAngle GetTargetAngle() {
			return this.targetAngle;
		}

		public void SetLedState(bool enabled) {
			this.ledState = enabled;

			this.FlushLedState();
		}

		public bool GetLedState() {
			return this.ledState;
		}

		private void FlushLedState() {
			this.hardwareInterface.SetServoLight(this.id, this.ledState);
		}

		public void SetSpeed(ushort speed) {
			this.speed = speed;

			this.FlushSpeed();
		}

		public ushort GetSpeed() {
			return this.speed;
		}

		private void FlushSpeed() {
			this.hardwareInterface.SetServoSpeed(this.id, this.speed);
		}

		public void FlushState() {
			this.FlushTargetAngle();
			this.FlushLedState();
			this.FlushSpeed();
		}

		public void GoToRoot() {
			this.SetTargetAngle(this.rootAngle);
		}
	}
}