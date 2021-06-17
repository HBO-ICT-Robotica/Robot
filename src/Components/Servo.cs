using System;
using Robot.Serial;
using Robot.Units.Angle;
using Robot.Utility;

namespace Robot.Components {
	public class Servo {
		private IHardwareInterface hardwareInterface = null;

		private byte id = default;
		private bool winding = default;

		private IAngle rootAngle = default;
		private IAngle minAngle = default;
		private IAngle maxAngle = default;

		private int targetDegree = 0;

		private IAngle targetAngle = default;
		private IAngle angle = default;

		private bool ledState = default;

		private ushort speed = default;

		public Servo(byte id, bool winding, IAngle rootAngle, IAngle minAngle, IAngle maxAngle) {
			this.hardwareInterface = ServiceLocator.Get<IHardwareInterface>();

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

		public void SetTargetAngle(int targetPosition) {
		if (this.winding)
				this.targetDegree = 300 - targetPosition;
			else
				this.targetDegree = targetPosition;

			this.FlushTargetAngle();
		}

		private void FlushTargetAngle() {
			this.hardwareInterface.SetServoTargetDegree(this.id, (ushort)(this.targetDegree));
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
			this.SetTargetAngle((int)this.maxAngle.AsDegrees());
		}
	}
}