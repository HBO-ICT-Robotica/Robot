namespace Robot.Serial {
	public delegate void ServoPositionUpdatedHandler(byte id, ushort position);
	public delegate void JoystickValueReceivedHandler(byte id, byte value);
	public delegate void RemoteTimeoutHandler();
	public delegate void LoadCellValueUpdatedHandler(int value);

	public interface IHardwareInterface {
		event ServoPositionUpdatedHandler servoPositionUpdated;
		event JoystickValueReceivedHandler joystickValueReceived;
		event RemoteTimeoutHandler remoteTimeoutEvent;
		event LoadCellValueUpdatedHandler loadCellValueUpdated;

		void Open();
		void Close();

		void Reset();
		bool IsReady();

		void SetServoTargetDegree(byte servoId, ushort position);
		void SetServoLight(byte servoId, bool enabled);
		void SetServoSpeed(byte servoId, ushort speed);

		void SetMotorPwm(byte motorId, byte pwm);
		void SetMotorMode(byte motorId, byte mode);

		void SendWeight(int weight);

		void InvokeServoPositionUpdated(byte id, ushort position);
		void InvokeJoystickValueReceived(byte id, byte value);
		void InvokeLoadCellValueUpdated(int value);
		void InvokeRemoteTimeout();
	}
}