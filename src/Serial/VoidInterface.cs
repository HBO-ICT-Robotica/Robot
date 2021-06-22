namespace Robot.Serial {
	public class VoidInterface : IHardwareInterface {
#pragma warning disable 67
		public event ServoPositionUpdatedHandler servoPositionUpdated;
		public event JoystickValueReceivedHandler joystickValueReceived;
		public event RemoteTimeoutHandler remoteTimeoutEvent;
		public event LoadCellValueUpdatedHandler loadCellValueUpdated;
#pragma warning restore 67

		public void Open() { }
		public void Close() { }
		public void Reset() { }
		public bool IsReady() { return true;  }

		public void SetServoTargetDegree(byte servoId, ushort position) { }
		public void SetServoSpeed(byte servoId, ushort speed) { }
		public void SetServoLight(byte servoId, bool enabled) { }

		public void SetMotorPwm(byte motorId, byte pwm) { }
		public void SetMotorMode(byte motorId, byte mode) { }

		public void SendWeight(int weight) { }

		public void InvokeServoPositionUpdated(byte id, ushort position) { }
		public void InvokeJoystickValueReceived(byte id, byte value) { }
		public void InvokeLoadCellValueUpdated(int value) { }
		public void InvokeRemoteTimeout() { }
	}
}