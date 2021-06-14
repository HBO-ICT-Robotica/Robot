using System.Device.Gpio;

namespace Robot.Components {
	public class LoadCell {
		public LoadCell(int clockPinNumber, int dataPinNumber) {
			this.clockPinNumber = clockPinNumber;
            this.dataPinNumber = dataPinNumber;

			this.Begin();
		}

		private int clockPinNumber;
        private int dataPinNumber;

		private GpioController gpio;

		/// <summary>
		/// Used to signal that the device is properly initialized and ready to use
		/// </summary>
		private bool available = false;

        /// <summary>
        /// Initialize the load sensing device.
        /// </summary> 
        /// <returns>
        /// Async operation object.
        /// </returns>
        public bool Begin()
        {
            this.gpio = new GpioController();

            if (null == gpio)
            {
                available = false;
                return false;
            }

            gpio.OpenPin(clockPinNumber, PinMode.Output);
            gpio.Write(clockPinNumber, false);
            gpio.OpenPin(dataPinNumber, PinMode.Input);

            available = true;
            return true;
        }

        public float getWeight() {
			if (!available) { return 0.0f; }
			
			return ReadData();
        }

        // Byte:     0        1        2        3
        // Bits:  76543210 76543210 76543210 76543210
        // Data: |--------|--------|--------|--------|
        // Bit#:  33222222 22221111 11111100 00000000
        //        10987654 32109876 54321098 76543210
        private int ReadData()
        {
            uint value = 0;
            byte[] data = new byte[4];

            // Wait for chip to become ready
            for (; false != this.gpio.Read(dataPinNumber) ;);

            // Clock in data
            data[1] = ShiftInByte();
            data[2] = ShiftInByte();
            data[3] = ShiftInByte();

            // Clock in gain of 128 for next reading
			gpio.Write(clockPinNumber, true);
			gpio.Write(clockPinNumber, false);

            // Replicate the most significant bit to pad out a 32-bit signed integer
            if (0x80 == (data[1] & 0x80))
            {
                data[0] = 0xFF;
            } else {
                data[0] = 0x00;
            }

            // Construct a 32-bit signed integer
            value = (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);

            // Datasheet indicates the value is returned as a two's complement value
            // https://cdn.sparkfun.com/datasheets/Sensors/ForceFlex/hx711_english.pdf

            // flip all the bits
            value = ~value;

            // ... and add 1
            return (int)(++value);
        }

        private byte ShiftInByte()
        {
            byte value = 0x00;

            // Convert "GpioPinValue.High" and "GpioPinValue.Low" to 1 and 0, respectively.
            // NOTE: Loop is unrolled for performance
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 7);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 6);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 5);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 4);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 3);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 2);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)((byte)(this.gpio.Read(dataPinNumber)) << 1);
            gpio.Write(clockPinNumber, false);
            gpio.Write(clockPinNumber, true);
            value |= (byte)this.gpio.Read(dataPinNumber);
            gpio.Write(clockPinNumber, false);

            return value;
        }
	}
}