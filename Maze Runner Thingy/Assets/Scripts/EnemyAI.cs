using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public LayerMask mask;
	public float walkSpeed;
	public float rotateSpeed;

	public bool wallToLeft;
	public bool wallToRight;
	public bool wallAhead;
	public bool wallAheadFar;
	public bool turning;
	public bool walking = true;
	public bool searching;
	public bool turningLeft;
	public bool turningRight;
	public bool turningAround;
	public bool canTurnLeft;
	public bool canTurnRight;
	//public bool allowedToTurnLeft = true;
	//public bool allowedToTurnRight = true;
	public float turnCounter;
	public float walkCounter;

	void Update(){
		if (!turning) {
			canTurnLeft = false;
			canTurnRight = false;
			RaycastHit hitFront;
			RaycastHit hitFrontFar;
			RaycastHit hitLeft;
			RaycastHit hitRight;
			RaycastHit backLeft;
			RaycastHit backRight;
			Debug.DrawRay (transform.position - transform.forward * 0.2f, -transform.right, Color.red);
			if (Physics.Raycast (transform.position, -transform.right, out hitLeft, 1, mask)) {
				//Debug.Log ("left");
				wallToLeft = true;
				Debug.Log (hitLeft.distance);
				//allowedToTurnLeft = true;
			} else {
				//Debug.Log ("not left");
				wallToLeft = false;
				if (!Physics.Raycast (transform.position - transform.forward * 0.2f, -transform.right, out backLeft, 1, mask)) {
					canTurnLeft = true;
				} else {
					canTurnLeft = false;
				}
			}
			if (Physics.Raycast (transform.position, transform.right, out hitRight, 1, mask)) {
				//Debug.Log ("right");
				wallToRight = true;
				//allowedToTurnRight = true;
			} else {
				//Debug.Log ("not right");
				wallToRight = false;
				if (!Physics.Raycast (transform.position - transform.forward * 0.2f, transform.right, out backRight, 1, mask)) {
					canTurnRight = true;
				} else {
					canTurnRight = false;
				}
			}
			if (Physics.Raycast (transform.position, transform.forward, out hitFront, 0.5f, mask)) {
				//Debug.Log ("ahead");
				wallAhead = true;
			} else {
				//Debug.Log ("not ahead");
				wallAhead = false;
			}
			if (Physics.Raycast (transform.position, transform.forward, out hitFrontFar, 1, mask)) {
				wallAheadFar = true;
			} else {
				wallAheadFar = false;
			}

			if (wallToLeft) {
				if (hitLeft.distance < 0.3f) {
					transform.position += transform.right * (0.45f - hitLeft.distance);
				}
			} else if (wallToRight) {
				if (hitRight.distance < 0.3f) {
					transform.position -= transform.right * (0.45f - hitRight.distance);
				}
			}

			/*if (canTurnLeft  && walkCounter > 100) {
				if (canTurnRight && walkCounter > 100) {
					if (!wallAheadFar && !wallAhead) { //Can go forward, left, or right
						int randTemp = (int)Random.Range (1, 3);
						if (randTemp == 1) {
							TurnLeft ();
						} else if (randTemp == 2) {
							TurnRight ();
						} else {
							//allowedToTurnLeft = false;
							//allowedToTurnRight = false;
							walkCounter = 0;
						}
					} else { //Can go left or right
						int randTemp = (int)Random.Range (1, 2);
						if (randTemp == 1) {
							TurnLeft ();
						} else {
							TurnRight ();
						}
					}
				} else {
					if (!wallAheadFar && !wallAhead) { //Can go forward or left
						int randTemp = (int)Random.Range (1, 2);
						if (randTemp == 1) {
							TurnLeft ();
						} else {
							//allowedToTurnLeft = false;
							//allowedToTurnRight = false;
							walkCounter = 0;
						}
					} else { //Must go left
						TurnLeft ();
					}
				}
			} else {
				if (canTurnRight && walkCounter > 100) {
					if (!wallAheadFar && !wallAhead) { //Can go forward or right
						int randTemp = (int)Random.Range (1, 2);
						if (randTemp == 1) {
							TurnRight ();
						} else {
							//allowedToTurnLeft = false;
							//allowedToTurnRight = false;
							walkCounter = 0;
						}
					} else { //Must go Right
						TurnRight ();
					}
				} else { //Dead end
					if (wallAheadFar || wallAhead) {
						TurnAround ();
					}
				}
			}*/

			if (!wallAhead && walking) {
				transform.position += transform.forward * walkSpeed * Time.deltaTime;
				walkCounter += walkSpeed;
			}
			if (wallAhead) {
				if (canTurnLeft) {
					if (canTurnRight) {
						int randTemp = (int)Random.Range (1, 3);
						Debug.Log (randTemp);
						if (randTemp == 1) {
							TurnLeft ();
						} else if (randTemp == 2) {
							TurnRight ();
						}
					} else {
						TurnLeft ();
					}
				} else {
					if (canTurnRight) {
						TurnRight ();
					} else {
						TurnAround ();
					}
				}
			} else {
				if (walkCounter > 50 && !wallAheadFar) {
					if (canTurnLeft) {
						if (canTurnRight) {
							int randTemp = (int)Random.Range (1, 4);
							if (randTemp == 1) {
								TurnLeft ();
							} else if (randTemp == 2) {
								TurnRight ();
							} else {
								walkCounter = 0;
							}
						} else {
							int randTemp = (int)Random.Range (1, 3);
							if (randTemp == 1) {
								TurnLeft ();
							} else {
								walkCounter = 0;
							}
						}
					} else {
						if (canTurnRight) {
							int randTemp = (int)Random.Range (1, 3);
							if (randTemp == 1) {
								TurnRight ();
							} else {
								walkCounter = 0;
							}
						}
					}
				}
			}
		} else {
			if (turningLeft) {
				if (turnCounter + rotateSpeed < 90) {
					transform.Rotate (0, -rotateSpeed, 0);
					turnCounter += rotateSpeed;
				} else {
					transform.Rotate (0, -90 + turnCounter, 0);
					turning = false;
					turningLeft = false;
					walking = true;
				}
			}
			if (turningRight) {
				if (turnCounter + rotateSpeed < 90) {
					transform.Rotate (0, rotateSpeed, 0);
					turnCounter += rotateSpeed;
				} else {
					transform.Rotate (0, 90 - turnCounter, 0);
					turning = false;
					turningRight = false;
					walking = true;
				}
			}
			if (turningAround) {
				if (turnCounter + rotateSpeed < 180) {
					transform.Rotate (0, -rotateSpeed, 0);
					turnCounter += rotateSpeed;
				} else {
					transform.Rotate (0, -180 + turnCounter, 0);
					turning = false;
					turningAround = false;
					walking = true;
				}
			}
		}
	}

	void TurnLeft(){
		turning = true;
		turningLeft = true;
		turnCounter = 0;
		//allowedToTurnLeft = false;
		//allowedToTurnRight = false;
		walkCounter = 0;
	}

	void TurnRight(){
		turning = true;
		turningRight = true;
		turnCounter = 0;
		//allowedToTurnLeft = false;
		//allowedToTurnRight = false;
		walkCounter = 0;
	}

	void TurnAround(){
		turning = true;
		turningAround = true;
		turnCounter = 0;
		//allowedToTurnLeft = false;
		//allowedToTurnRight = false;
		walkCounter = 0;
	}

}
