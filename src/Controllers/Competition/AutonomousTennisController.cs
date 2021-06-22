using System.Threading;
using System;
using System.Collections.Generic;
using OpenCvSharp;

using Leg = Robot.Components.Leg;

namespace Robot.Controllers {

	public class AutonomousTennisController : IRobotController {

		private enum RobotState {
			MANUAL_TOBALL,
			AUTO_MOVETOBALL,
			AUTO_PICKUPBALL,
			MANUAL_TODEPOSIT,
			AUTO_MOVETODEPOSIT,
			AUTO_DEPOSITBALL
		}

		private Robot.Components.Robot robot = null;
		private List<Leg> legs = new List<Leg>();

		private RobotState robotState;
		private RobotState previousState;

		private int maxLegHeight = 108;
		private int minLegHeight = 0;

		private Robot.Utility.Vision vision;

		private Rect depositRect;
		private Rect ballRect;

		private Scalar lowerRangeBall;
		private Scalar upperRangeBall;

		private Scalar lowerRangeDeposit;
		private Scalar upperRangeDeposit;

		public AutonomousTennisController(Robot.Components.Robot robot) {
			this.robot = robot;
			this.robotState = RobotState.MANUAL_TOBALL;

			this.legs.AddRange(robot.GetBody().GetLegs());
			Robot.Utility.Vision vision = new Utility.Vision();

			// Set Scalar ranges for the ball and for the deposit point
			lowerRangeBall = new Scalar();
			upperRangeBall = new Scalar();

			lowerRangeDeposit = new Scalar();
			upperRangeDeposit = new Scalar();
		}

		private void FindBall() {
			//Look around for the ball
			Tuple<Rect, bool> objectLocation = vision.CalculateLocation();

			if (objectLocation.Item2)
				this.robotState = RobotState.AUTO_MOVETOBALL;
		}

		private void MoveToBall() {
			//Controleer locatie
			Tuple<Rect, bool> objectLocation = vision.CalculateLocation();

			//Controleer of er wel een bal is
			if (!objectLocation.Item2)
				return;

			//verplaats richting locatie
			Move(objectLocation.Item1);

			//Controleer of hij al op de bal erg dichtbij lijkt
			if (CheckLocation(objectLocation.Item1, ballRect)) {
				// Zoja, ga naar volgende state
				this.robotState = RobotState.AUTO_PICKUPBALL;
			}
		}

		private void PickUpBall() {
			//Grijper helemaal dicht
			if (robot.GetGripper().IsClosed()) {
				// Zodra de grijper dicht is, ga naar de volgende state
				this.robotState = RobotState.MANUAL_TODEPOSIT;
			}
		}


		private void FindDeposit() {
			//Look around for the ball
			Tuple<Rect, bool> objectLocation = vision.CalculateLocation();

			if (objectLocation.Item2)
				this.robotState = RobotState.AUTO_MOVETODEPOSIT;
		}


		private void MoveToDeposit() {
			//Controleer locatie
			Tuple<Rect, bool> objectLocation = vision.CalculateLocation();

			//Controleer of er wel een bal is
			if (!objectLocation.Item2)
				return;

			//verplaats richting locatie
			Move(objectLocation.Item1);

			//Controleer of het depositpunt al dichtbij genoeg is
			if (CheckLocation(objectLocation.Item1, depositRect)) {
				// Zoja, ga naar volgende state
				this.robotState = RobotState.AUTO_DEPOSITBALL;
			}
		}


		private void DepositBall() {
			//Zodra helemaal open, ga naar volgende stap
			if (robot.GetGripper().IsOpen()) {
				this.robotState = RobotState.MANUAL_TOBALL;
			}
		}


		private bool CheckLocation(Rect objectRect, Rect targetRect) {
			//Check a certain ROI at the bottom of the screen, for the object, if it is there return true, otherwise return false
			if (targetRect.Contains(objectRect))
				return true;

			return false;
		}

		private void Move(Rect location) {
			Console.WriteLine($"Move towards {location}");
		}


		private bool ChangedRobotState() {
			if (previousState == robotState) {
				return false;
			}
			previousState = robotState;
			Console.WriteLine($"Changed robot state {robotState}");

			return true;
		}

		public void Step(float dt) {
			switch (robotState) {
				case RobotState.MANUAL_TOBALL:
					// Add controller input some way
					if (ChangedRobotState()) {
						vision.SetupSearch(lowerRangeBall, upperRangeBall, Rect.Empty);
					}

					FindBall();
					break;
				case RobotState.AUTO_MOVETOBALL:
					if (ChangedRobotState()) {
						// Verlaag robot naar grond niveau
						foreach (var leg in legs) {
							leg.SetHeight(minLegHeight);
						}

						// Open de gripper alvast
						robot.GetGripper().Open();
					}

					MoveToBall();
					break;
				case RobotState.AUTO_PICKUPBALL:
					if (ChangedRobotState()) {
						//Sluit de grijper
						robot.GetGripper().Close(Components.Gripper.Pickupable.BALL);
					}

					PickUpBall();
					break;
				case RobotState.MANUAL_TODEPOSIT:
					// Add controller input some way
					if (ChangedRobotState()) {
						vision.SetupSearch(lowerRangeDeposit, upperRangeDeposit, Rect.Empty);
					}

					FindDeposit();
					break;
				case RobotState.AUTO_MOVETODEPOSIT:
					if (ChangedRobotState()) {
						//Ga weer omhoog
						foreach (Leg leg in legs) {
							leg.SetHeight(maxLegHeight);
						}
					}

					MoveToDeposit();
					break;
				case RobotState.AUTO_DEPOSITBALL:
					if (ChangedRobotState()) {
						//Open de grijper
						robot.GetGripper().Open();
					}

					DepositBall();
					break;
			}
		}

		public void Dispose() {

		}
	}
}
