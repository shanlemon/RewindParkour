using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class FPSCamera : MonoBehaviour {
 
	[SerializeField] private Transform cameraTransform;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
 
	public float minimumY = -60F;
	public float maximumY = 60F;
 
	float rotationX = 0F;
	float rotationY = 0F;
 
	Quaternion playerOriginalRotation, cameraOriginalRotation;
 
	void Update ()
	{
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;

		Quaternion yQuaternion = Quaternion.AngleAxis (ClampAngle(rotationY, minimumY, maximumY), Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX , Vector3.up);

		transform.localRotation = playerOriginalRotation * xQuaternion;
		cameraTransform.localRotation = cameraOriginalRotation * yQuaternion;
		
	}
 
	void Start ()
	{		
        Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		playerOriginalRotation = transform.localRotation;
		cameraOriginalRotation = cameraTransform.localRotation;

		LockCursor();	
	}

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}