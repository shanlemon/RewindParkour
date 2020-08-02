using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToOnKeyPress : MonoBehaviour {
	[SerializeField] private KeyCode teleportKey = KeyCode.R;
	private Vector3 savedPosition;
	private Quaternion savedRotation;
	private bool hasSaved = false;

	private Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	private void Update() {
		if (!hasSaved && Input.GetKeyDown(teleportKey)) {
			savedPosition = rb.position;
			savedRotation = rb.rotation;
			hasSaved = true;
		}

		if (hasSaved && Input.GetKeyDown(teleportKey)) {
			rb.position = savedPosition;
			rb.rotation = savedRotation;
		}
	}
}
