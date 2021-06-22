using System;
using Robot.Components;

namespace Robot.Steering {
	public partial class WheelsControl {
		public struct Speed {
			public readonly float frontLeft;
			public readonly float backLeft;
			public readonly float frontRight;
			public readonly float backRight;

			public Speed(float frontLeft, float backLeft, float frontRight, float backRight) {
				this.frontLeft = frontLeft;
				this.backLeft = backLeft;
				this.frontRight = frontRight;
				this.backRight = backRight;
			}
		}

		private Joystick steeringJoystick;
		private Joystick thrustJoystick;

		private float deadzone = 0.0f;
		private float maxSpeed = 0.0f;

		private float minimumSteeringForce = 150.0f;

		private InputCurve thrustInputCurve = default;
		private InputCurve steeringInputCurve = default;

		public WheelsControl(Joystick steeringJoystick, Joystick thrustJoystick, float deadzone = 0.1f, float maxSpeed = 50, InputCurve thrustInputCurve = InputCurve.CUBIC, InputCurve steeringInputCurve = InputCurve.CUBIC) {
			this.steeringJoystick = steeringJoystick;
			this.thrustJoystick = thrustJoystick;

			this.deadzone = deadzone;
			this.maxSpeed = maxSpeed;

			this.thrustInputCurve = thrustInputCurve;
			this.steeringInputCurve = steeringInputCurve;
		}

		public Speed GetSpeed() {
			float frontLeftSpeed = 0.0f;
			float backLeftSpeed = 0.0f;
			float frontRightSpeed = 0.0f;
			float backRightSpeed = 0.0f;

			var rawThrustInput = steeringJoystick.GetRelativeValue();
			var rawSteeringInput = thrustJoystick.GetRelativeValue();

			if (rawThrustInput > deadzone || rawThrustInput < -deadzone) {
				float thrustInput = ApplyInputCurve(rawThrustInput);

				frontLeftSpeed += thrustInput * maxSpeed;
				backLeftSpeed += thrustInput * maxSpeed;
				frontRightSpeed += thrustInput * maxSpeed;
				backRightSpeed += thrustInput * maxSpeed;
			}

			if (rawSteeringInput > deadzone || rawSteeringInput < -deadzone) {
				float steeringInput = ApplyInputCurve(rawSteeringInput);
				float steeringForce = MathF.Max(steeringInput * maxSpeed, minimumSteeringForce);

				if (rawSteeringInput > 0.0f) {
					// Right steering
					frontRightSpeed -= steeringForce;
					backLeftSpeed += steeringForce;
				} else {
					// Left steering
					frontLeftSpeed -= steeringForce;
					backRightSpeed += steeringForce;
				}
			}

			return new Speed(
					frontLeftSpeed,
					backLeftSpeed,
					frontRightSpeed,
					backRightSpeed
			);
		}

		private float ApplyInputCurve(float x) {
			switch (this.thrustInputCurve) {
				case InputCurve.LINEAR:
					return Linear(x);
				case InputCurve.CUBIC:
					return CubicIn(x);
			}
			return 0.0f;
		}

		private float CubicIn(float x) {
			return (float)Math.Pow(x, 3);
		}

		private float Linear(float x) {
			return x;
		}
	}
}