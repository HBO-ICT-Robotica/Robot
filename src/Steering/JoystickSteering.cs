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

        private float leftSpeed = 0.0f;
        private float rightSpeed = 0.0f;

        private InputCurve inputCurve = InputCurve.CUBIC;

        public JoystickSteering(Joystick xJoystick, Joystick yJoystick, float maxSpeed, float bound = 0.1f, float safetyFactor = 0.5f, float carMaxSpeed = 255, InputCurve inputCurve = InputCurve.CUBIC)
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
            this.rightSpeed = 0.0f;
            this.leftSpeed = 0.0f;

            var yJoystickval = xJoystick.GetRelativeValue();
            var xJoystickval = yJoystick.GetRelativeValue();

            // bound is joystick deadzone
            // prefents wiggle from still joystick
            if (yJoystickval > bound || yJoystickval < -bound)
            {
                // forward and reverse
                leftSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
                rightSpeed += GetEased(yJoystickval) * safetyFactor * carMaxSpeed;
            }

            if (xJoystickval > bound || xJoystickval < -bound)
            {
                // check driving forward or driving backward
                float winding = yJoystickval < bound ? -1 : 1;

                // steering
                leftSpeed += xJoystickval * safetyFactor * carMaxSpeed * winding;
                rightSpeed -= xJoystickval * safetyFactor * carMaxSpeed * winding;
            }
        }

        public int GetLeftSpeed()
        {
            return (int) Math.Round(leftSpeed);
        }

        public int GetRightSpeed()
        {
            return (int) Math.Round(rightSpeed);
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