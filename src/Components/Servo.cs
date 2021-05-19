using Robot.Serial;
using System;

namespace Robot.Components {
	public class Servo {
		private TeensyCommunicator communicator = null;

		private byte id = default;
		
		private ushort targetPosition = default;
		private ushort position = default;
		
		private bool ledState = default;
	
		private ushort speed = default;

		public Servo(TeensyCommunicator communicator, byte id, ushort startPosition) {
			this.communicator = communicator;

			this.id = id;

			this.targetPosition = startPosition;
			this.position = 0;

			this.ledState = false;

			this.speed = 1023;

			this.FlushState();

			this.communicator.ServoPositionUpdated += OnServoPositionUpdated;
		}

		private void OnServoPositionUpdated(byte id, ushort position) {
			if (id != this.id)
				return;
				
			this.position = position;
		}

		public void SetTargetPosition(ushort targetPosition) {
			this.targetPosition = targetPosition;

			this.FlushTargetPosition();
		}

		private void FlushTargetPosition() {
			this.communicator.SetServoTargetPosition(this.id, this.targetPosition);
		}

		public ushort GetPosition() {
			return this.position;
		}

		public void SetLedState(bool enabled) {
			this.ledState = enabled;

			this.FlushLedState();
		}

		public bool GetLedState() {
			return this.ledState;
		}

		private void FlushLedState() {
			this.communicator.SetServoLight(this.id, this.ledState);
		}

		public void SetSpeed(ushort speed) {
			this.speed = speed;

			this.FlushSpeed();
		}

		public ushort GetSpeed() {
			return this.speed;
		}

		private void FlushSpeed() {
			this.communicator.SetServoSpeed(this.id, this.speed);
		}

		public void FlushState() {
			this.FlushTargetPosition();
			this.FlushLedState();
			this.FlushSpeed();
		}
	}
}