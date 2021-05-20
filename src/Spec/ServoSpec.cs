namespace Robot.Spec {
	/// <summary>
	/// Defines the specification a servo
	/// </summary>
	public class ServoSpec {
		private ushort maxPosition = default;
		private ushort minPosition = default;

		public ServoSpec(ushort minPosition, ushort maxPosition) {
			this.minPosition = minPosition;
			this.maxPosition = maxPosition;
		}

		public ushort GetMinPosition() {
			return this.minPosition;
		}

		public ushort GetMaxPosition() {
			return this.maxPosition;
		}
	}
}