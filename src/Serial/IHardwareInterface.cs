namespace Robot.Serial {
	public delegate void ServoPositionUpdatedHandler(byte id, ushort position);
	public delegate void JoystickValueReceivedHandler(byte id, byte value);
	public delegate void RemoteTimeoutHandler();

	public interface IHardwareInterface {
		event ServoPositionUpdatedHandler servoPositionUpdated { add { } remove { } }
		event JoystickValueReceivedHandler joystickValueReceived { add { } remove { } }
		event RemoteTimeoutHandler remoteTimeoutEvent { add { } remove { } }

		void Open();
		void Close();

		void SetServoTargetDegree(byte servoId, ushort position);
		void SetServoLight(byte servoId, bool enabled);
		void SetServoSpeed(byte servoId, ushort speed);

		void SetMotorPwm(byte motorId, byte pwm);
		void SetMotorMode(byte motorId, byte mode);
	}
}