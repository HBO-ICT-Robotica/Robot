using System;
using System.Diagnostics;
using Robot.Serial;
using Robot.Utility;
using Robot.Utility.Easings;

namespace Robot.Components
{
	public class Joystick
	{
		private IHardwareInterface hardwareInterface = null;

		private int id = default;

		private int value = default;
		private int minValue = default;
		private int maxValue = default;

		public Joystick(int id, int minValue, int maxValue)
		{
			this.hardwareInterface = ServiceLocator.Get<IHardwareInterface>();

			this.id = id;

			this.value = (int)Easings.LinearClamped(minValue, maxValue, 0.5f);
			this.minValue = minValue;
			this.maxValue = maxValue;

			this.hardwareInterface.joystickValueReceived += OnJoystickValueReceived;
		}

		public int GetValue()
		{
			return this.value;
		}

		public float GetRelativeValue()
		{
			return this.Map(this.value, this.minValue, this.maxValue, -1.0f, 1.0f);
		}

		private void OnJoystickValueReceived(byte id, byte value)
		{
			if (id != this.id)
				return;

			Console.WriteLine("Received joysitck value: " + value);

			this.value = value;
		}

		private float Map(float x, float inMin, float inMax, float outMin, float outMax)
		{
			return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}
}