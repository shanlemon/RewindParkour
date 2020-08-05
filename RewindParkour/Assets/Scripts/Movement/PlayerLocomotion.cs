using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

	[SerializeField] private PlayerInput input = default;
	[SerializeField] private Transform camera = default;
	private Rigidbody rb = default;

	[SerializeField] public float Acceleration = 10f;
	[SerializeField] public float MaxSpeed = 15f;
	[SerializeField] private float turnSpeed = 10f;


	private float XInput => input.XInput;
	private float YInput => input.YInput;

	private Vector3 movementDirection;

	private bool IsMoveInput => !((XInput == 0) && (YInput == 0));
	private Vector3 MovementVelocity => new Vector3(rb.velocity.x, 0f, rb.velocity.z);
	private Vector3 YVelocity => new Vector3(0f, rb.velocity.y, 0);

	private void Start() {
		rb = GetComponent<Rigidbody>();
		movementDirection = rb.transform.forward;
	}

	void FixedUpdate() {
		if (IsMoveInput) {
			movementDirection = CalculateMovementDirection(XInput, YInput);

			rb.AddForce(movementDirection * Acceleration, ForceMode.VelocityChange);

			if (MovementVelocity.magnitude > MaxSpeed) {
				rb.velocity = MovementVelocity.normalized * MaxSpeed / 120f + YVelocity;
			}
		}

		if (!IsMoveInput) {
			movementDirection = CalculateMovementDirection(0, 0);

			rb.velocity = MovementVelocity / 2f + YVelocity;
		}
	}

	public Vector3 CalculateMovementDirection(float xInput, float yInput) {
		Vector3 inputDirection = camera.right * xInput + camera.forward * yInput;
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
