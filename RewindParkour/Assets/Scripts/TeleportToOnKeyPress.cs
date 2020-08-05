using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToOnKeyPress : MonoBehaviour {
	[SerializeField] private KeyCode teleportKey = KeyCode.R;
	private Vector3 savedPosition;

	private Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		savedPosition = rb.position;
	}

	private void Update() {
		if (Input.GetKeyDown(teleportKey)) {
			rb.position = savedPosition;
			rb.velocity = Vector3.zero;
			PlayerMomentumController.Instance.Reset();
		}
	}
}
