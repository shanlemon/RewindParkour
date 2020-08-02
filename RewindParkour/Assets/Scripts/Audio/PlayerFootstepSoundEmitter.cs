﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSoundEmitter : MonoBehaviour
{
    [SerializeField] private Sound footstepSound = default;
    [SerializeField] private float speed = default;
    [SerializeField] private PlayerMovement pm = default;

    private float stepCycle;
    private float nextStep;

    // Start is called before the first frame update
    void Start()
    {
        stepCycle = 0f;
        nextStep = stepCycle / 2f;
    }

    private void FixedUpdate() {
        ProgressStepCycle();
    }

    private void ProgressStepCycle()
    {
        if (pm.IsMoving && pm.Grounded)
        {
            stepCycle += speed * Time.deltaTime;
        }

        if (stepCycle <= 1/speed)
        {
            return;
        }

        PlayFootStepAudio();
        stepCycle = 0;
    }

     private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        Managers.AudioManager.PlayOneShot(footstepSound.name);

    }

}
