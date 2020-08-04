// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

	[SerializeField] private Transform playerCam;
	[SerializeField] private float sensitivity = 50f;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update() {
		Look();
	}

	private float desiredX, xRotation;
	private void Look() {
		//if (warpPosition.IsWarping) return;

		float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

		//Find current look rotation
		Vector3 rot = playerCam.transform.localRotation.eulerAngles;
		desiredX = rot.y + mouseX;

		//Rotate, and also make sure we dont over- or under-rotate.
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		//Perform the rotations
		playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
		//orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
	}
}
