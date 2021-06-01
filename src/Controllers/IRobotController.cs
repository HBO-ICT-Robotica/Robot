using System;
namespace Robot.Controllers {
	public interface IRobotController : IDisposable {
		void Step(float dt);
	}
}