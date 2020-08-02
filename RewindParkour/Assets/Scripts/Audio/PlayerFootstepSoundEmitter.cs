using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSoundEmitter : MonoBehaviour
{
    [SerializeField] private Sound footstepSound = default;
    [SerializeField] private float stepInterval = default;
    [SerializeField] private float speed = default;
    [SerializeField] private Rigidbody rb = default;

    private float stepCycle;
    private float nextStep;

    // Start is called before the first frame update
    void Start()
    {
        stepCycle = 0f;
        nextStep = stepCycle / 2f;
    }

    private void FixedUpdate() {
        ProgressStepCycle(speed);
    }

    private void ProgressStepCycle(float speed)
    {
        if (rb.velocity.sqrMagnitude > 0)
        {
            stepCycle += (rb.velocity.magnitude + (speed)) *
                         Time.fixedDeltaTime;
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
        Managers.AudioManager.PlayOneShotAtLocation(footstepSound.name, gameObject);

    }

}
