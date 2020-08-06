using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSoundEmitter : MonoBehaviour {
	[SerializeField] private Sound footstepSound = default;
	[SerializeField] private float speed = default;
	private StrafeMovement sm = default;

	private float stepCycle;
	private float nextStep;
	// Start is called before the first frame update
	void Start() {
		sm = GetComponent<StrafeMovement>();
		stepCycle = 0f;
		nextStep = stepCycle / 2f;
	}

	private void FixedUpdate() {
		ProgressStepCycle();
	}

	private void ProgressStepCycle() {
		if (sm.IsMoving && sm.onGround) {
			stepCycle += speed * Time.deltaTime;
		}

		if (stepCycle <= 1 / speed) {
			return;
		}

		PlayFootStepAudio();
		stepCycle = 0;
	}

	private void PlayFootStepAudio() {
		Debug.Log("PLAY SOUND");
		// pick & play a random footstep sound from the array,
		Managers.AudioManager.PlayOneShot(footstepSound.name);

	}
}
