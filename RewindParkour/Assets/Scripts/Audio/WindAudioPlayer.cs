using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioPlayer : MonoBehaviour {

	[SerializeField] private Sound windSound = default;
	[SerializeField] private float minVolume = .1f, maxVolume = .25f, startVelocityMagnitude = 30, maxVelocity = 120;
	private StrafeMovement strafeMovement = default;
	private Rigidbody rigidbody = default;

	private bool playingClip;
	// private bool isMovingInAir {get {return !playerMovement.Grounded && (playerMovement.IsMoving || rigidbody.velocity.magnitude > 0);}}
	private bool isMovingInAir { get { return !strafeMovement.onGround && rigidbody.velocity.magnitude > startVelocityMagnitude; } }
	private void Start() {
		rigidbody = GetComponent<Rigidbody>();
		strafeMovement = GetComponent<StrafeMovement>();
	}

	// Update is called once per frame
	void Update() {
		if (!playingClip && isMovingInAir) {
			playingClip = true;
			Managers.AudioManager.Play(windSound.name);
		} else if (playingClip && strafeMovement.onGround) {
			playingClip = false;
			Managers.AudioManager.Stop(windSound.name);
			return;
		}

		if (playingClip) {
			windSound.source.volume = Mathf.Lerp(minVolume, maxVolume, rigidbody.velocity.magnitude / maxVelocity);
		}


	}
}
