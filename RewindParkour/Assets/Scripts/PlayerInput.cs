using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public float XInput { get; private set; }
	public float YInput { get; private set; }
	public bool JumpInput { get; private set; }


	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		XInput = Input.GetAxisRaw("Horizontal");
		YInput = Input.GetAxisRaw("Vertical");
		JumpInput = Input.GetButton("Jump");
	}
}
