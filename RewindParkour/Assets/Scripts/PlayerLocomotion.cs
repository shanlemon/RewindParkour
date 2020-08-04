using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

	[SerializeField] private PlayerInput input = default;
	private Rigidbody rb = default;

	[SerializeField] private float moveSpeed = 3500;
	[SerializeField] private float turnSpeed = 10f;

	private float XInput => input.XInput;
	private float YInput => input.YInput;

	private Vector3 movementDirection;

	private bool IsMoveInput => !((XInput == 0) && (YInput == 0));

	private void Start() {
		rb = GetComponent<Rigidbody>();
		movementDirection = rb.transform.forward;
	}

	void FixedUpdate() {
		if (IsMoveInput) {
			movementDirection = CalculateMovementDirection();

			rb.AddForce(movementDirection * moveSpeed);
		}

		float oldYVelocity = rb.velocity.y;
		rb.velocity /= 1.4f;
		rb.velocity = new Vector3(rb.velocity.x, oldYVelocity, rb.velocity.z);

	}

	public Vector3 CalculateMovementDirection() {
		Vector3 inputDirection = rb.transform.right * XInput + rb.transform.forward * YInput;
		inputDirection = inputDirection.normalized;

		float xDirection = Mathf.Lerp(movementDirection.x, inputDirection.x, Time.fixedDeltaTime * turnSpeed);
		float zDirection = Mathf.Lerp(movementDirection.z, inputDirection.z, Time.fixedDeltaTime * turnSpeed);

		return new Vector3(xDirection, 0f, zDirection);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(rb.position, rb.position + movementDirection * 5);
	}

}
