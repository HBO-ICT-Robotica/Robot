using Robot.Serial;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot.Components {
	public class Servo {
		private ILogger logger = null;
		private TeensyCommunicator communicator = null;

		private byte id = default;
		private bool winding = default;

		private int rootDegree = default;
		private int minDegree = default;
		private int maxDegree = default;

		private int targetDegree = default;
		private int degree = default;
		
		private bool ledState = default;
	
		private ushort speed = default;

		public Servo(byte id, bool winding, int rootDegree, int minDegree, int maxDegree) {
			this.logger = ServiceLocator.Get<ILogger>();
			this.communicator = ServiceLocator.Get<TeensyCommunicator>();

			this.id = id;
			this.winding = winding;

			this.rootDegree = rootDegree;
			this.minDegree = minDegree;
			this.maxDegree = maxDegree;

			this.targetDegree = 0;
			this.degree = 0;

			this.ledState = false;

			this.speed = 1023 / 4;

			this.communicator.ServoPositionUpdated += OnServoPositionUpdated;
			this.SetTargetDegree(rootDegree);
			this.FlushState();
		}

		private void OnServoPositionUpdated(byte id, ushort position) {
			if (id != this.id)
				return;
				
			this.degree = position;
		}

		public int GetRootDegree() {
			return this.rootDegree;
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

		public int GetDegree() {
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
		
		public void GoToRoot() {
			this.SetTargetDegree(this.rootDegree);
		}
	}
}