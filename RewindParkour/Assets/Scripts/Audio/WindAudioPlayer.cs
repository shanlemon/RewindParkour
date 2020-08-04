using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioPlayer : MonoBehaviour {

	[SerializeField] private PlayerInput playerInput = default;
	[SerializeField] private PlayerLocomotion playerLocomotion = default;
	[SerializeField] private Rigidbody rigidbody = default;
	[SerializeField] private Sound windSound = default;
	[SerializeField] private float minVolume = .1f, maxVolume = .25f, startVelocityMagnitude = 30, maxVelocity = 120;

	private bool playingClip;
	// private bool isMovingInAir {get {return !playerMovement.Grounded && (playerMovement.IsMoving || rigidbody.velocity.magnitude > 0);}}
	private bool isMovingInAir { get { return !playerInput.Grounded && rigidbody.velocity.magnitude > startVelocityMagnitude; } }


	// Update is called once per frame
	void Update() {
		if (!playingClip && isMovingInAir) {
			playingClip = true;
			Managers.AudioManager.Play(windSound.name);
		} else if (playingClip && playerInput.Grounded) {
			playingClip = false;
			Managers.AudioManager.Stop(windSound.name);
			return;
		}

		if (playingClip) {
			Debug.Log(rigidbody.velocity.magnitude + " " + playerLocomotion.MaxSpeed);
			windSound.source.volume = Mathf.Lerp(minVolume, maxVolume, rigidbody.velocity.magnitude / maxVelocity);
		}


	}
}
