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

	private bool Grounded => input.Grounded;
	private bool JumpInput => input.JumpInput;
	private Vector3 NormalVector => input.NormalVector;

	// Update is called once per frame
	void FixedUpdate() {
		bool hasJumpCooldownPassed = (timeSinceLastJump > jumpCooldown);

		//when grounded
		if (Grounded && hasJumpCooldownPassed) {
			canJump = true;
			hasActivatedLeniencyCountdown = false;
			hasJumped = false;
		}

		//set jump leniency when in air
		if (!Grounded && !JumpInput) {
			if (!hasActivatedLeniencyCountdown) {
				hasActivatedLeniencyCountdown = true;
				Invoke(nameof(SetJumpLeniency), postGroundJumpLeniency);
			}
		}

		//allow jump if player is certain distance above ground
		if (Physics.Raycast(rb.position, Vector3.down, aboveGroundDistanceToJump, input.WhatIsGround)) {
			if (JumpInput && hasJumpCooldownPassed)
				InitialJumpForce();
			// else, do a normal jump
		} else if (canJump && JumpInput && hasJumpCooldownPassed) {
			InitialJumpForce();
		}

		//add force upwards for amount if holding jump
		if (JumpInput && hasJumped && !(timeSinceLastJump > timeToApplyUpwardsForce)) {
			rb.AddForce(Vector2.up * upwardsForce);
		}


		//extra gravity when not holding jump
		if (!JumpInput && !Grounded && !canJump) {
			rb.AddForce(Vector3.down * downwardsForce);
		}
		// was in player movmenet script for some reason
		rb.AddForce(Vector3.down * Time.fixedDeltaTime * 10);

		timeSinceLastJump += Time.deltaTime;
	}

	private void InitialJumpForce() {
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
