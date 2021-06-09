namespace Robot.Serial {
	public class VoidInterface : IHardwareInterface {
#pragma warning disable 67
		public event ServoPositionUpdatedHandler servoPositionUpdated;
		public event JoystickValueReceivedHandler joystickValueReceived;
		public event RemoteTimeoutHandler remoteTimeoutEvent;
#pragma warning restore 67

		public void Open() { }
		public void Close() { }

		public void SetServoTargetDegree(byte servoId, ushort position) { }
		public void SetServoSpeed(byte servoId, ushort speed) { }
		public void SetServoLight(byte servoId, bool enabled) { }

		public void SetMotorPwm(byte motorId, byte pwm) { }
		public void SetMotorMode(byte motorId, byte mode) { }

		public void InvokeJoystickValueReceived(byte id, byte value) { }
	}
}