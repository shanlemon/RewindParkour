using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

	[SerializeField] private PlayerMovement player = default;
	[SerializeField] private PlayerInput input = default;
	private Rigidbody rb = default;

	private float moveSpeed = 3500;
	private float frictionForce = 500;

	private float XInput => input.XInput;
	private float YInput => input.YInput;
	private Transform Orientation => player.orientation;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		//rb.AddForce(Orientation.forward * YInput * moveSpeed * Time.fixedDeltaTime);
		//rb.AddForce(Orientation.right * XInput * moveSpeed * Time.fixedDeltaTime);

		Vector3 velocityDirection = rb.velocity.normalized;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(rb.position, rb.position + rb.velocity.normalized * 5);
	}

}
