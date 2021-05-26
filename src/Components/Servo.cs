using System.ComponentModel.Design;
using System;
using Robot.Serial;
using Robot.Spec;
using static Robot.Spec.RobotSpec.ServoDatas;
using Robot.Utility;

namespace Robot.Components {
	public class Servo {
		private byte id = default;
		private bool winding = default;

		private int rootDegree = default;
		private int minDegree = default;
		private int maxDegree = default;

		private int targetDegree = default;
		private int degree = default;
		
		private bool ledState = default;
	
		private ushort speed = default;

		public Servo(TeensyCommunicator communicator, byte id, bool winding, int rootDegree, int minDegree, int maxDegree) {
			this.servoSpec = servoSpec;

			this.communicator = communicator;

			this.id = servoData.GetId();
			this.winding = servoData.GetWinding();

			this.SetTargetDegree(servoSpec.GetZeroDegree());
			this.degree = 0;

			this.ledState = false;

			this.speed = 1023;

			this.FlushState();

			this.communicator.ServoPositionUpdated += OnServoPositionUpdated;
		}

		private void OnServoPositionUpdated(byte id, ushort position) {
			if (id != this.id)
				return;
				
			this.degree = position;
		}

		public void SetTargetDegree(int targetPosition) {
			if (this.winding)
				this.targetDegree = 300 - targetPosition;
			else
				this.targetDegree = targetPosition;

			this.FlushTargetDegree();
		}

		private void FlushTargetDegree() {
			this.communicator.SetServoTargetDegree(this.id, (ushort)this.targetDegree);
		}

		public int GetPosition() {
			return this.degree;
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
			this.FlushTargetDegree();
			this.FlushLedState();
			this.FlushSpeed();
		}
	}
}