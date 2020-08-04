using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	[SerializeField] private PlayerMovement player = default;
	[SerializeField] private PlayerInput input = default;
	private Rigidbody rb = default;

	//Forces
	[SerializeField] private float jumpForce = 80f;
	[SerializeField] private float upwardsForce = 80f * .37f * .37f;
	[SerializeField] private float downwardsForce = 20f;

	//Delays (in seconds)
	[SerializeField] private float jumpCooldown = 0.25f;
	[SerializeField] private float timeToApplyUpwardsForce = 0.25f / 2;
	[SerializeField] private float postGroundJumpLeniency = .15f;

	private float timeSinceLastJump = 100000f;
	private bool hasActivatedLeniencyCountdown = false;
	public bool hasJumped = false;
	private bool canJump = false;

	private bool Grounded => player.Grounded;
	private bool JumpInput => input.JumpInput;
	private Vector3 NormalVector => player.NormalVector;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

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

		//initial jump force
		if (canJump && JumpInput && hasJumpCooldownPassed) {
			timeSinceLastJump = 0f;
			hasJumped = true;
			canJump = false;

			//reset y velocity when you jump
			rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

			rb.AddForce(Vector2.up * jumpForce * 1.5f);
			rb.AddForce(NormalVector * jumpForce * .5f);
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

	private void SetJumpLeniency() {
		canJump = false;
	}
}
