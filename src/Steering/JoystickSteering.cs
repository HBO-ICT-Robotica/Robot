using System;
using Robot.Components;

namespace Robot.Steering
{
    public partial class JoystickSteering
    {

		private Joystick xJoystick; 
        private Joystick yJoystick;

		private float bound;
        private float safetyFactor;
        private float carMaxSpeed;

        private float frontLeftSpeed = 0.0f;
        private float backLeftSpeed = 0.0f;
        private float frontRightSpeed = 0.0f;
        private float backRightSpeed = 0.0f;

        private InputCurve inputCurve = InputCurve.CUBIC;

        public JoystickSteering(Joystick xJoystick, Joystick yJoystick, float maxSpeed, float bound = 0.1f, float safetyFactor = 0.5f, float carMaxSpeed = 130, InputCurve inputCurve = InputCurve.CUBIC)
        {
            this.xJoystick = xJoystick;
			this.yJoystick = yJoystick;
			this.carMaxSpeed = maxSpeed;

            this.bound = bound;
            this.safetyFactor = safetyFactor;
            this.carMaxSpeed = carMaxSpeed;
            this.inputCurve = inputCurve;
        }

        public void UpdateSpeed()
        {
            this.frontLeftSpeed = 0.0f;
            this.backLeftSpeed = 0.0f;
            this.frontRightSpeed = 0.0f;
            this.backRightSpeed = 0.0f;

            var yJoystickval = xJoystick.GetRelativeValue();
            var xJoystickval = yJoystick.GetRelativeValue();

            // bound is joystick deadzone
            // prefents wiggle from still joystick
            if (yJoystickval > bound || yJoystickval < -bound)
            {
                // forward and reverse
                frontLeftSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
                backLeftSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
                frontRightSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
                backRightSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
            }

            if (xJoystickval > bound || xJoystickval < -bound)
            {
                // check driving forward or driving backward
                float winding = xJoystickval < 0.0f ? -1 : 1;

				if (winding > 0.0f) {
                    frontLeftSpeed += xJoystickval * safetyFactor * carMaxSpeed * 2.0f;
                    backRightSpeed -= xJoystickval * safetyFactor * carMaxSpeed * 2.0f;
                } else {
                    frontRightSpeed -= xJoystickval * safetyFactor * carMaxSpeed * 2.0f;
                    backLeftSpeed += xJoystickval * safetyFactor * carMaxSpeed * 2.0f;
                }
            }
		}

        public int GetFrontLeftSpeed()
        {
            return (int) Math.Round(frontLeftSpeed);
        }

        public int GetBackLeftSpeed()
        {
            return (int) Math.Round(backLeftSpeed);
        }

        public int GetFrontRightSpeed()
        {
            return (int) Math.Round(frontRightSpeed);
        }

        public int GetBackRightSpeed()
        {
            return (int) Math.Round(backRightSpeed);
        }

        private float GetEased(float x)
        {
            switch (this.inputCurve)
            {
                case InputCurve.LINEAR:
                    return Linear(x);
                case InputCurve.CUBIC:
                    return CubicIn(x);
            }
            return 0.0f;
        }

        private float CubicIn(float x)
        {
            return (float)Math.Pow(x, 3);
        }

        private float Linear(float x)
        {
            return x;
        }
    }
}