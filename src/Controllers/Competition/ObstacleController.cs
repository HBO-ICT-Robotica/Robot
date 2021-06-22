using System.Threading;
using System;
using System.Collections.Generic;
using OpenCvSharp;

using Leg = Robot.Components.Leg;

namespace Robot.Controllers {

	public class ObstacleController : IRobotController {
		private Robot.Components.Robot robot = null;
		private List<Leg> legs = new List<Leg>();

		private enum ObstacleState {
			WAIT,
			STAIRS,
			GAP,
			SLOPE
		}

		private ObstacleState currentObstacle;
		private ObstacleState previousObstacle;

		private int stairSpeed = 0;
		private int gapSpeed = 0;
		private int slopeSpeed = 0;

		private int maxLegHeight = 108;
		private int minLegHeight = 0;

		public ObstacleController(Robot.Components.Robot robot) {
			this.robot = robot;
			this.legs.AddRange(robot.GetBody().GetLegs());

			currentObstacle = ObstacleState.WAIT;
			previousObstacle = ObstacleState.WAIT;
		}

		public void ChangeMode() {
			if (currentObstacle != ObstacleState.SLOPE)
				currentObstacle = currentObstacle + 1;
			else
				currentObstacle = ObstacleState.STAIRS;
		}

		private void WalkStairs() {
			// Extra stair logic
		}

		private void CrossGap() {
			// Extra gap logic
		}

		private void RideSlope() {
			// Extra slope logic

		}

		private bool ChangedObstacleState() {
			if (previousObstacle == currentObstacle) {
				return false;
			}
			previousObstacle = currentObstacle;
			Console.WriteLine($"Changed robot state {currentObstacle}");

			return true;
		}

		public void Step(float dt) {
			switch (currentObstacle) {
				case ObstacleState.STAIRS:
					if (ChangedObstacleState()) {
						//Set height on 108
            //Set speed for the obstacle
						foreach (var leg in legs) {
							leg.SetHeight(maxLegHeight);
							leg.GetWheel().SetSpeed(stairSpeed);
						}
					}

					WalkStairs();
					break;
				case ObstacleState.GAP:
					if (ChangedObstacleState()) {
						// Set height on 0
            //Set speed for the obstacle
						foreach (var leg in legs) {
							leg.SetHeight(minLegHeight);
							leg.GetWheel().SetSpeed(gapSpeed);
						}
					}

					CrossGap();
					break;
				case ObstacleState.SLOPE:
					if (ChangedObstacleState()) {
						//Set height on 0
            //Set speed for the obstacle
						foreach (var leg in legs) {
							leg.SetHeight(minLegHeight);
							leg.GetWheel().SetSpeed(slopeSpeed);
						}
					}

					RideSlope();
					break;
				default:
					break;
			}
		}

		public void Dispose() {

		}
	}
}