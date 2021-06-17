namespace Robot.Components {
	public class Gripper {
		public enum Pickupable {
			BALL,
			WEIGHT,
		}

		private Servo servo = null;
		private bool isOpen = default;
		private LoadCell loadCell = null;

		public Gripper(Servo servo, LoadCell loadCell) {
			this.servo = servo;
			this.loadCell = loadCell;
		}

		public void Open() {
			this.servo.SetTargetAngle(135);
			
			isOpen = true;
		}

		public void Close(Pickupable pickupable) {
			if (pickupable == Pickupable.BALL)
				this.servo.SetTargetAngle(105);
			else if (pickupable == Pickupable.WEIGHT)
				this.servo.SetTargetAngle(90);
			
						
			isOpen = false;
		}

		public bool IsOpen() {
			return isOpen;
		}

		public bool IsClosed() {
			return !isOpen;
		}

		public LoadCell GetLoadCell() {
			return this.loadCell;
		}
	}
}