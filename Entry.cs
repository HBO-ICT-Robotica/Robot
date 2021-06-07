using System.Globalization;
using System.Threading;
using System;
using Robot.Utility;
using Robot.Utility.Logging;

namespace Robot {
	/// <summary>
	/// Entry point of the application
	/// </summary>
	public class Entry {
		public static void Main() {
			// Initialize logger
			LoggerComposite compositeLogger = new LoggerComposite(LogLevel.DEBUG);
			compositeLogger.AddLogger(new LoggerConsole());

			compositeLogger.LogDebug("Application start");

			// Initialize service locator
			ServiceLocator.Initialize(new ServiceLocator());
			compositeLogger.LogDebug("Started service locator");

			// Register logger
			ServiceLocator.Register<ILogger>(compositeLogger);

			// Create program
			compositeLogger.LogDebug("Initialized program");
			var program = new Program();

			// Step program

			DateTime a = DateTime.Now;
			DateTime b = DateTime.Now;

			while (true) {
				b = DateTime.Now;
				float dt = (b.Ticks - a.Ticks) / 10000000f;

				program.Step(dt);

				a = b;

				Thread.Sleep(100);
			}

			// Cleanup
			// compositeLogger.Flush();

			
		}
	}
}