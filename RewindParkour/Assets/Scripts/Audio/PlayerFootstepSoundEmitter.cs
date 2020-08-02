using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSoundEmitter : MonoBehaviour
{
    [SerializeField] private Sound footstepSound = default;
    [SerializeField] private float stepInterval = default;
    [SerializeField] private Rigidbody rb = default;
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
        if (rb.velocity.magnitude > 0 && pm.Grounded)
        {
            stepCycle += (rb.velocity.magnitude) * Time.fixedDeltaTime;
        }

        if (!(stepCycle > nextStep))
        {
            return;
        }

        nextStep = stepCycle + stepInterval;
        PlayFootStepAudio();
    }

     private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        Managers.AudioManager.PlayOneShot(footstepSound.name);

    }

}
