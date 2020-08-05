using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public float XInput { get; private set; }
	public float YInput { get; private set; }
	public bool JumpInputHeld { get; private set; }
	public bool JumpInputDown { get; private set; }

	public bool Grounded { get; private set; }
	public bool IsMoving => XInput != 0 || YInput != 0;

	public Vector3 NormalVector { get; private set; }

	[SerializeField] private LayerMask whatIsGround;
	public LayerMask WhatIsGround => whatIsGround;


	// Start is called before the first frame update
	void Start() {

	}

	private float jumpInputDownTime = 0.15f;
	private bool hasInvokedJumpDownTimer = false;
	void Update() {
		XInput = Input.GetAxisRaw("Horizontal");
		YInput = Input.GetAxisRaw("Vertical");
		JumpInputHeld = Input.GetButton("Jump");
		if (Input.GetButtonDown("Jump")) {
			JumpInputDown = true;
		} else {
			if (!hasInvokedJumpDownTimer) {
				hasInvokedJumpDownTimer = true;
				Invoke(nameof(ResetJumpDown), jumpInputDownTime);
			}
		}
	}

	private void ResetJumpDown() {
		JumpInputDown = false;
		hasInvokedJumpDownTimer = false;
	}


	private readonly float maxSlopeAngle = 35f;
	private bool IsFloor(Vector3 v) {
		float angle = Vector3.Angle(Vector3.up, v);
		return angle < maxSlopeAngle;
	}

	private bool cancellingGrounded;
	private void OnCollisionStay(Collision other) {
		//Make sure we are only checking for walkable layers
		int layer = other.gameObject.layer;
		if (whatIsGround != (whatIsGround | (1 << layer))) return;

		//Iterate through every collision in a physics update
		for (int i = 0; i < other.contactCount; i++) {
			Vector3 normal = other.contacts[i].normal;
			//FLOOR
			if (IsFloor(normal)) {
				Grounded = true;
				cancellingGrounded = false;
				NormalVector = normal;
				CancelInvoke(nameof(StopGrounded));
			}
		}

		float delay = 3f;
		if (!cancellingGrounded) {
			cancellingGrounded = true;
			Invoke(nameof(StopGrounded), Time.deltaTime * delay);
		}
	}

	private void StopGrounded() {
		Grounded = false;
	}
}
