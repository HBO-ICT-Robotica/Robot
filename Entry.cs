using System.Threading;
using System;
using Robot.Utility;

namespace Robot {
	/// <summary>
	/// Entry point of the application
	/// </summary>
	public class Entry {
		public static void Main() {
			ServiceLocator.Initialize(new ServiceLocator());

			var program = new Program();

			DateTime a = DateTime.Now;
			DateTime b = DateTime.Now;

			while (true) {
				b = DateTime.Now;
				float dt = (b.Ticks - a.Ticks) / 10000000f; 

				program.Step(dt);

				a = b;

				Thread.Sleep(1);
			}
		}
	}
}