using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	[SerializeField] private PlayerInput input = default;
	[SerializeField] private Rigidbody rb = default;

	//Forces
	[SerializeField] private float jumpForce = 250f;
	[SerializeField] private float upwardsForce = 30f;
	[SerializeField] private float downwardsForce = 20f;

	//Delays (in seconds)
	[SerializeField] private float jumpCooldown = 0.25f;
	[SerializeField] private float timeToApplyUpwardsForce = 0.25f / 2;
	[SerializeField] private float postGroundJumpLeniency = .15f;

	//Distances
	[SerializeField] private float aboveGroundDistanceToJump = 2f;

	private float timeSinceLastJump = 100000f;
	private bool hasActivatedLeniencyCountdown = false;
	public bool hasJumped = false;
	private bool canJump = false;

	private bool IsAboveGround => Physics.Raycast(rb.position, Vector3.down, aboveGroundDistanceToJump, input.WhatIsGround);
	private bool Grounded => input.Grounded;
	private bool JumpInputHeld => input.JumpInputHeld;
	private bool JumpInputDown => input.JumpInputDown;
	private Vector3 NormalVector => input.NormalVector;

	private bool HasJumpCooldownPassed => timeSinceLastJump > jumpCooldown;
	// Update is called once per frame
	void FixedUpdate() {
		//when grounded
		if (IsAboveGround && HasJumpCooldownPassed) {
			canJump = true;
			hasActivatedLeniencyCountdown = false;
			hasJumped = false;
		}

		//set jump leniency when in air
		if (!IsAboveGround && !JumpInputHeld) {
			if (!hasActivatedLeniencyCountdown) {
				hasActivatedLeniencyCountdown = true;
				Invoke(nameof(SetJumpLeniency), postGroundJumpLeniency);
			}
		}

		if (canJump && JumpInputDown && HasJumpCooldownPassed) {
			InitialJumpForce();
		}

		//add force upwards for amount if holding jump
		if (JumpInputHeld && hasJumped && !(timeSinceLastJump > timeToApplyUpwardsForce)) {
			rb.AddForce(Vector2.up * upwardsForce);
		}


		//extra gravity when not holding jump
		if (!JumpInputHeld && !Grounded && !canJump) {
			rb.AddForce(Vector3.down * downwardsForce);
		}
		// was in player movmenet script for some reason
		rb.AddForce(Vector3.down * Time.fixedDeltaTime * 10);

		timeSinceLastJump += Time.deltaTime;
	}

	private void InitialJumpForce() {
		Debug.Log("jump");
		timeSinceLastJump = 0f;
		hasJumped = true;
		canJump = false;

		//reset y velocity when you jump
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

		rb.AddForce(Vector2.up * jumpForce * 1.5f);
		rb.AddForce(NormalVector * jumpForce * .5f);

	}

	private void SetJumpLeniency() {
		canJump = false;
	}
}
