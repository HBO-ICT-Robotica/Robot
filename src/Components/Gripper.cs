using Robot.Units.Angle;

namespace Robot.Components {
	public class Gripper {
		public enum Pickupable {
			BALL,
			WEIGHT,
			
		}

		private Servo servo = null;
		private bool isOpen = default;
	
		public Gripper(Servo servo) {
			this.servo = servo;
		}

		public void Open() {
			this.servo.SetTargetAngle(new Degrees(135));
			
			isOpen = true;
		}

		public void Close(Pickupable pickupable) {
			if (pickupable == Pickupable.BALL)
				this.servo.SetTargetAngle(new Degrees(105));
			else if (pickupable == Pickupable.WEIGHT)
				this.servo.SetTargetAngle(new Degrees(90));
			
						
			isOpen = false;
		}

		public bool IsOpen() {
			return isOpen;
		}

		public bool IsClosed() {
			return !isOpen;
		}
	}
}